using Plugin.Connectivity;
using PortableApp.Helpers;
using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using System.Linq;

namespace PortableApp
{
    public partial class WetlandPlantsPage : ViewHelpers
    {
        ObservableCollection<Grouping<string, WetlandPlant>> plantsGrouped;
        List<string> jumpList;
        ListView wetlandPlantsList;
        ObservableCollection<WetlandPlant> plants;
        bool cameFromSearch;
        Dictionary<string, string> sortOptions = new Dictionary<string, string> { { "Scientific Name", "scinamenoauthor" }, { "Common Name", "commonname" }, { "Family", "family" }, { "Group", "sections" } };
        Picker sortPicker = new Picker();
        Button sortButton = new Button { Style = Application.Current.Resources["semiTransparentPlantButton"] as Style, Text = "Scientific Name", BorderRadius = Device.OnPlatform(0, 1, 0) };
        Button sortDirection = new Button { Style = Application.Current.Resources["semiTransparentPlantButton"] as Style, Text = "\u25BC", BorderRadius = Device.OnPlatform(0, 1, 0) };
        WetlandSetting sortField;
        Grid plantFilterGroup;
        Button browseFilter;
        Button searchFilter;
        Button favoritesFilter;
        SearchBar searchBar;

        protected override void OnAppearing()
        {
            if (!cameFromSearch)
            {
                plants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepo.GetAllWetlandPlants());
                if (plants.Count > 0) { wetlandPlantsList.ItemsSource = plants; };
                ChangeFilterColors(browseFilter);
                sortPicker.SelectedIndex = (int)App.WetlandSettingsRepo.GetSetting("Sort Field").valueint;
                base.OnAppearing();
            } 
            else
                ChangeFilterColors(searchFilter);
        }

        public WetlandPlantsPage()
        {
            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0), ColumnSpacing = 0 };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Construct filter button group
            plantFilterGroup = new Grid { ColumnSpacing = 3 };
            plantFilterGroup.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            plantFilterGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            plantFilterGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            plantFilterGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add browse filter
            browseFilter = new Button
            {
                Style = Application.Current.Resources["plantFilterButton"] as Style,
                Text = "Browse",
                Margin = new Thickness(2, 5)
            };
            browseFilter.Clicked += FilterPlants;
            plantFilterGroup.Children.Add(browseFilter, 0, 0);

            // Add Search filter
            searchFilter = new Button
            {
                Style = Application.Current.Resources["plantFilterButton"] as Style,
                Text = "Search",
                Margin = new Thickness(2, 5)
            };
            var SearchPage = new WetlandPlantsSearchPage();
            searchFilter.Clicked += async (s, e) => { await Navigation.PushModalAsync(SearchPage); };
            SearchPage.InitRunSearch += HandleRunSearch;
            SearchPage.InitCloseSearch += HandleCloseSearch;
            plantFilterGroup.Children.Add(searchFilter, 1, 0);

            // Add Favorites filter
            favoritesFilter = new Button
            {
                Style = Application.Current.Resources["plantFilterButton"] as Style,
                Text = "Favorites",
                Margin = new Thickness(2, 5)
            };
            favoritesFilter.Clicked += FilterPlants;
            plantFilterGroup.Children.Add(favoritesFilter, 2, 0);

            // Add header to inner container
            HeaderNavigationOptions navOptions = new HeaderNavigationOptions { plantFiltersVisible = true, plantFilterGroupButtons = plantFilterGroup, backButtonVisible = true, homeButtonVisible = true };
            Grid navigationBar = ConstructNavigationBar(navOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);

            // Add button group grid
            Grid searchSortGroup = new Grid();
            searchSortGroup.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            searchSortGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.7, GridUnitType.Star) });
            searchSortGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add search button
            searchBar = new CustomSearchBar
            {
                Placeholder = "Scientific or Common Name...",
                FontSize = 12,
                Margin = new Thickness(Device.OnPlatform(10, 0, 0), 0, 0, 0),
                SearchCommand = new Command(() => {  })
            };
            searchBar.TextChanged += SearchBarOnChange;
            searchSortGroup.Children.Add(searchBar, 0, 0);

            // Add sort container
            Grid sortContainer = new Grid { ColumnSpacing = 0 };
            sortContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.7, GridUnitType.Star) });
            sortContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            sortContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) });
            sortContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });

            sortButton.Clicked += SortPickerTapped;
            sortContainer.Children.Add(sortButton, 0, 0);

            foreach (string option in sortOptions.Keys) { sortPicker.Items.Add(option); }
            sortPicker.IsVisible = false;
            sortPicker.SelectedIndex = 0;
            if (Device.OS == TargetPlatform.iOS)
                sortPicker.Unfocused += SortOnUnfocused;
            else
                sortPicker.SelectedIndexChanged += SortItems;

            sortContainer.Children.Add(sortPicker, 0, 0);

            sortDirection.Clicked += ChangeSortDirection;
            sortContainer.Children.Add(sortDirection, 1, 0);

            searchSortGroup.Children.Add(sortContainer, 1, 0);

            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            innerContainer.Children.Add(searchSortGroup, 0, 1);

            // Create ListView container
            RelativeLayout listViewContainer = new RelativeLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Transparent,
            };

            // Add Plants ListView
            wetlandPlantsList = new ListView { BackgroundColor = Color.Transparent, RowHeight = 100 };
            wetlandPlantsList.ItemTemplate = new DataTemplate(typeof(WetlandPlantsItemTemplate));
            wetlandPlantsList.ItemSelected += OnItemSelected;
            wetlandPlantsList.SeparatorVisibility = SeparatorVisibility.None;

            listViewContainer.Children.Add(wetlandPlantsList,
                Constraint.RelativeToParent((parent) => { return parent.X; }),
                Constraint.RelativeToParent((parent) => { return parent.Y - 105; }),
                Constraint.RelativeToParent((parent) => { return parent.Width * .9; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; })
            );
            
            // Create jump list from termsGrouped
            jumpList = new List<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Select(x => x.ToString()).ToList());

            // Add jump list to right side
            StackLayout jumpListContainer = new StackLayout { Spacing = -1, Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            foreach (string letter in jumpList)
            {
                Label letterLabel = new Label { Text = letter, Style = Application.Current.Resources["jumpListLetter"] as Style };
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) => {
                    var firstRecordMatchingLetter = plants.Where(x => x.scinameauthorstripped[0].ToString() == letter).FirstOrDefault();
                    wetlandPlantsList.ScrollTo(firstRecordMatchingLetter, ScrollToPosition.Start, true);
                };
                letterLabel.GestureRecognizers.Add(tapGestureRecognizer);
                jumpListContainer.Children.Add(letterLabel);
            }

            listViewContainer.Children.Add(jumpListContainer,
                Constraint.RelativeToParent((parent) => { return parent.Width * .9; }),
                Constraint.RelativeToParent((parent) => { return parent.Y - 105; }),
                Constraint.RelativeToParent((parent) => { return parent.Width * .1; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; })
            );

            // Add ListView and Jump List to grid
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            innerContainer.Children.Add(listViewContainer, 0, 2);

            // Add FooterBar
            FooterNavigationOptions footerOptions = new FooterNavigationOptions { plantsFooter = true };
            Grid footerBar = ConstructFooterBar(footerOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });
            innerContainer.Children.Add(footerBar, 0, 3);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        public void FilterPlants(object sender, EventArgs e)
        {
            Button filter = (Button)sender;
            ChangeFilterColors(filter);
            if (filter.Text == "Browse")
                plants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepo.GetAllWetlandPlants());
            else if (filter.Text == "Favorites")
                plants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepo.GetFavoritePlants());
            
            wetlandPlantsList.ItemsSource = plants;
        }

        public void ChangeFilterColors(Button selectedFilter)
        {
            foreach (Button button in plantFilterGroup.Children)
            {
                if (button.Text == selectedFilter.Text)
                    button.BackgroundColor = Color.FromHex("cc000000");
                else
                    button.BackgroundColor = Color.FromHex("66000000");
            }
        }

        private async void HandleRunSearch(object sender, EventArgs e)
        {
            plants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepo.GetPlantsBySearchCriteria());
            wetlandPlantsList.ItemsSource = plants;
            cameFromSearch = true;
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void HandleCloseSearch(object sender, EventArgs e)
        {
            cameFromSearch = true;
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private void SearchBarOnChange(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                wetlandPlantsList.ItemsSource = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepo.GetAllWetlandPlants());
            else
                wetlandPlantsList.ItemsSource = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepo.WetlandPlantsQuickSearch(e.NewTextValue));
        }

        private void SortPickerTapped(object sender, EventArgs e)
        {
            sortPicker.Focus();
        }

        private void SortOnUnfocused(object sender, FocusEventArgs e)
        {
            SortItems(sender, e);
        }

        private async void SortItems(object sender, EventArgs e)
        {
            sortButton.Text = sortPicker.Items[sortPicker.SelectedIndex];
            wetlandPlantsList.ItemsSource = null;
            if (sortButton.Text == "Scientific Name")
                plants.Sort(i => i.scinamenoauthor, sortDirection.Text);
            else if (sortButton.Text == "Common Name")
                plants.Sort(i => i.commonname, sortDirection.Text);
            else if (sortButton.Text == "Family")
                plants.Sort(i => i.family, sortDirection.Text);
            else if (sortButton.Text == "Group")
                plants.Sort(i => i.sections, sortDirection.Text);

            await App.WetlandSettingsRepo.AddOrUpdateSettingAsync(new WetlandSetting { name = "Sort Field", valuetext = sortButton.Text, valueint = sortPicker.SelectedIndex });
            wetlandPlantsList.ItemsSource = plants;
        }

        private void ChangeSortDirection(object sender, EventArgs e)
        {
            if (sortDirection.Text == "\u25BC")
            {
                sortDirection.Text = "\u25B2";
            }
            else
            {
                sortDirection.Text = "\u25BC";
            }
            SortItems(sender, e);
        }
        
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (wetlandPlantsList.SelectedItem != null)
            {
                var selectedItem = e.SelectedItem as WetlandPlant;
                var detailPage = new PortableApp.Views.WetlandPlantDetailPage(selectedItem, plants);
                detailPage.BindingContext = selectedItem;
                wetlandPlantsList.SelectedItem = null;
                await Navigation.PushAsync(detailPage);
            }
        }
    }

    public class WetlandPlantsItemTemplate : ViewCell
    {
        WetlandSetting sortField;

        public WetlandPlantsItemTemplate()
        {
            sortField = App.WetlandSettingsRepo.GetSetting("Sort Field");
            string[] labelValues = GetLabelValues();

            // Construct grid, the cell container
            Grid cell = new Grid
            {
                BackgroundColor = Color.FromHex("88000000"),
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(0, 0, 0, 10)
            };
            cell.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });
            cell.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.7, GridUnitType.Star) });
            cell.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });

            // Add image
            var image = new Image { Aspect = Aspect.AspectFill, Margin = new Thickness(0, 0, 0, 20) };
            image.SetBinding(Image.SourceProperty, new Binding("ThumbnailPath"));
            cell.Children.Add(image, 0, 0);

            // Add text section
            StackLayout textSection = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 2 };

            Label label1 = new Label { TextColor = Color.White, FontSize = 12, FontAttributes = FontAttributes.Bold };
            label1.SetBinding(Label.TextProperty, new Binding(labelValues[0]));
            if (labelValues[0] == "scinamenoauthorstripped") label1.FontAttributes = FontAttributes.Italic;
            textSection.Children.Add(label1);

            var divider = new BoxView { HeightRequest = 1, WidthRequest = 500, BackgroundColor = Color.White };
            textSection.Children.Add(divider);

            Label label2 = new Label { TextColor = Color.White, FontSize = 12 };
            label2.SetBinding(Label.TextProperty, new Binding(labelValues[1]));
            if (labelValues[1] == "scinamenoauthorstripped") label2.FontAttributes = FontAttributes.Italic;
            textSection.Children.Add(label2);

            Label label3 = new Label { TextColor = Color.White, FontSize = 12 };
            label3.SetBinding(Label.TextProperty, new Binding(labelValues[2]));
            textSection.Children.Add(label3);

            Label label4 = new Label { TextColor = Color.White, FontSize = 12 };
            label4.SetBinding(Label.TextProperty, new Binding(labelValues[3]));
            textSection.Children.Add(label4);

            cell.Children.Add(textSection, 1, 0);
            View = cell;
        }

        private string[] GetLabelValues()
        {
            if (sortField.valuetext == "Common Name")
                return new string[] { "commonname", "scinamenoauthorstripped", "family", "sections" };
            if (sortField.valuetext == "Family")
                return new string[] { "family", "scinamenoauthorstripped", "commonname", "sections" };
            if (sortField.valuetext == "Group")
                return new string[] { "sections", "scinamenoauthorstripped", "commonname", "family" };
            else
                return new string[] { "scinamenoauthorstripped", "commonname", "family", "sections" };
        }

    }

    public class CustomSearchBar : SearchBar
    {

    }

}
