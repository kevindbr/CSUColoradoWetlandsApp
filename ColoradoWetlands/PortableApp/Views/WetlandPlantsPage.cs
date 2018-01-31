using PortableApp.Helpers;
using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using System.Linq;
using System.Reflection;
using FFImageLoading;
using FFImageLoading.Forms;
using PortableApp.Views;

namespace PortableApp
{
    public partial class WetlandPlantsPage : ViewHelpers
    {
        ObservableCollection<Grouping<string, WetlandPlant>> plantsGrouped;
        ListView wetlandPlantsList;
        List<string> jumpList;
        StackLayout jumpListContainer;
        ObservableCollection<WetlandPlant> plants;
        bool cameFromSearch;
        Dictionary<string, string> sortOptions = new Dictionary<string, string> { { "Scientific Name", "scinamenoauthor" }, { "Common Name", "commonname" }, { "Family", "family" }, { "Group", "sections" } };
        Picker sortPicker = new Picker();
        Button sortButton = new Button { Style = Application.Current.Resources["semiTransparentPlantButton"] as Style, Text = "Sort", BorderRadius = Device.OnPlatform(0, 1, 0) };
        Button sortDirection = new Button { Style = Application.Current.Resources["semiTransparentPlantButton"] as Style, Text = "\u25BC", BorderRadius = Device.OnPlatform(0, 1, 0) };
        WetlandSetting sortField;
        Grid plantFilterGroup;
        string[] labelValues;
        Button browseFilter;
        Button searchFilter;
        Button favoritesFilter;
        SearchBar searchBar;

        protected async override void OnAppearing()
        {
            List<WetlandPlantImage> imageList= new List<WetlandPlantImage> (App.WetlandPlantImageRepoLocal.GetAllWetlandPlantImages() );
            List<WetlandPlantImage> imageListTemp = imageList.OrderBy(item => item.PlantId).ToList();

            IsLoading = true;
            // Get filtered plant list if came from search
            if (!cameFromSearch)
            {
                //changed this to local
                if (App.WetlandPlantRepoLocal.GetAllWetlandPlants().Count > 0)
                {
                    plants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
                    if (plants.Count > 0) { wetlandPlantsList.ItemsSource = plants; };
                    ChangeFilterColors(browseFilter);
                    base.OnAppearing();
                }
                else
                {
                    plants = new ObservableCollection<WetlandPlant>(await externalConnection.GetAllPlants());
                }
            } 
            else
                ChangeFilterColors(searchFilter);

            // Set sort settings and filter jump list
            GetSortField();

            if (sortField.valuetext == "Sort")
            {
                sortPicker.SelectedIndex = 0;
                FilterJumpList("Scientific Name");
            } 
            else
            {
                sortPicker.SelectedIndex = (int)sortField.valueint;
                FilterJumpList(sortButton.Text);
            }
            IsLoading = false;
        }

        public WetlandPlantsPage()
        {
            GC.Collect();
            // Initialize variables
            sortField = new WetlandSetting();

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0), ColumnSpacing = 0 };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Construct filter button group
            plantFilterGroup = new Grid { ColumnSpacing = -1, Margin = new Thickness(0, 8, 0, 5) };
            plantFilterGroup.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });

            // Add browse filter
            browseFilter = new Button
            {
                Style = Application.Current.Resources["plantFilterButton"] as Style,
                Text = "Browse"
            };
            browseFilter.Clicked += FilterPlants;
            plantFilterGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            plantFilterGroup.Children.Add(browseFilter, 0, 0);

            BoxView divider = new BoxView { HeightRequest = 40, WidthRequest = 1, BackgroundColor = Color.White };
            plantFilterGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1) });
            plantFilterGroup.Children.Add(divider, 1, 0);

            // Add Search filter
            searchFilter = new Button
            {
                Style = Application.Current.Resources["plantFilterButton"] as Style,
                Text = "Search"
            };
            
            var SearchPage = new WetlandPlantsSearchPage();
            searchFilter.Clicked += async (s, e) => 
            {
                if (App.WetlandPlantRepoLocal.GetAllWetlandPlants().Count > 0)
                {
                    await Navigation.PushModalAsync(SearchPage);
                }
                else
                {
                    await DisplayAlert("Alert", "Please Download Plants To Use 'Search By Characteristic'", "OK");
                }
               
            };
            SearchPage.InitRunSearch += HandleRunSearch;
            SearchPage.InitCloseSearch += HandleCloseSearch;
            plantFilterGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            plantFilterGroup.Children.Add(searchFilter, 2, 0);
            
            BoxView divider2 = new BoxView { HeightRequest = 40, WidthRequest = 1, BackgroundColor = Color.White };
            plantFilterGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1) });
            plantFilterGroup.Children.Add(divider2, 3, 0);

            // Add Favorites filter
            favoritesFilter = new Button
            {
                Style = Application.Current.Resources["plantFilterButton"] as Style,
                Text = "Favorites"
            };
            favoritesFilter.Clicked += FilterPlants;
            plantFilterGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            plantFilterGroup.Children.Add(favoritesFilter, 4, 0);

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

            // Add search bar
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
            wetlandPlantsList = new ListView(ListViewCachingStrategy.RecycleElement) { BackgroundColor = Color.Transparent, RowHeight = 100 };
           
            wetlandPlantsList.ItemTemplate = CellTemplate();
            wetlandPlantsList.ItemSelected += OnItemSelected;
            wetlandPlantsList.SeparatorVisibility = SeparatorVisibility.None;

            listViewContainer.Children.Add(wetlandPlantsList,
                Constraint.RelativeToParent((parent) => { return parent.X; }),
                Constraint.RelativeToParent((parent) => { return parent.Y - 105; }),
                Constraint.RelativeToParent((parent) => { return parent.Width * .9; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; })
            );
            

            // Add jump list to right side
            jumpListContainer = new StackLayout { Spacing = -1, Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

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
            GC.Collect();
        }

        private DataTemplate CellTemplate()
        {
            // Get correct order of labels on each plant
            GetSortField();
            labelValues = GetLabelValues();

            var cellTemplate = new DataTemplate(() => {                 

                // Construct grid, the cell container
                Grid cell = new Grid
                {
                    BackgroundColor = Color.FromHex("DD000000"),
                    Padding = new Thickness(5, 5, 5, 5),
                    Margin = new Thickness(0, 0, 0, 2)
                };
                cell.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.6, GridUnitType.Star) });
                cell.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
                cell.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });

                // Add image
                 // var image = new Image { Aspect = Aspect.AspectFill, Margin = new Thickness(0, 0, 0, 20) };
                string imageBinding = downloadImages ? "ThumbnailPathDownloaded" : "ThumbnailPathStreamed";
                var cachedImage = new CachedImage()
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 300,
                    HeightRequest = 300,
                    Aspect = Aspect.AspectFill,
                    Margin = new Thickness(0, 0, 0, 10),
                    CacheDuration = TimeSpan.FromDays(30),
                    DownsampleToViewSize = true,
                    RetryCount = 0,
                    RetryDelay = 250,
                    TransparencyEnabled = false,
                    FadeAnimationEnabled = false,
                    LoadingPlaceholder = "loading.png",
                    ErrorPlaceholder = "error.png",
                };

                        
                if (App.WetlandPlantImageRepoLocal.GetAllWetlandPlantImages() != null)
                    if (App.WetlandPlantImageRepoLocal.GetAllWetlandPlantImages().Count > 0)
                        imageBinding = "ThumbnailPathDownloaded";

                
                cachedImage.SetBinding(CachedImage.SourceProperty, new Binding(imageBinding));
                cell.Children.Add(cachedImage, 0, 0);

                // Add text section
                StackLayout textSection = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 2};

                Label label1 = new Label { TextColor = Color.White, FontSize = 12, FontAttributes = FontAttributes.Bold, FontFamily = "sans serif" };
                label1.SetBinding(Label.TextProperty, new Binding(labelValues[0]));
                if (labelValues[0] == "scinamenoauthorstripped") label1.FontAttributes = FontAttributes.Italic;
                textSection.Children.Add(label1);

                var headerDivider = new BoxView { HeightRequest = 1, WidthRequest = 500, BackgroundColor = Color.White };
                textSection.Children.Add(headerDivider);

                Label label2 = new Label { TextColor = Color.White, FontSize = 12, FontFamily = "sans serif" };
                label2.SetBinding(Label.TextProperty, new Binding(labelValues[1]));
                if (labelValues[1] == "scinamenoauthorstripped") label2.FontAttributes = FontAttributes.Italic;
                textSection.Children.Add(label2);

                Label label3 = new Label { TextColor = Color.White, FontSize = 12, FontFamily = "sans serif" };
                label3.SetBinding(Label.TextProperty, new Binding(labelValues[2]));
                textSection.Children.Add(label3);

                Label label4 = new Label { TextColor = Color.White, FontSize = 12, FontFamily = "sans serif" };
                label4.SetBinding(Label.TextProperty, new Binding(labelValues[3]));
                textSection.Children.Add(label4);

                cell.Children.Add(textSection, 1, 0);
                return new ViewCell { View = cell };
            });
              GC.Collect();
            return cellTemplate;
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

        private void GetSortField()
        { 
            sortField = App.WetlandSettingsRepo.GetSetting("Sort Field");
            sortButton.Text = sortField.valuetext;
        }

        public void FilterPlants(object sender, EventArgs e)
        {
            Button filter = (Button)sender;
            ChangeFilterColors(filter);
            if (filter.Text == "Browse")
                plants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
            else if (filter.Text == "Favorites")
                plants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetFavoritePlants());
            
            wetlandPlantsList.ItemsSource = plants;
            FilterJumpList(sortButton.Text);
        }

        public void ChangeFilterColors(Button selectedFilter)
        {
            foreach (var element in plantFilterGroup.Children)
            {
                if (element.GetType() == typeof(Button))
                {
                    Button button = (Button)element;
                    if (button.Text == selectedFilter.Text)
                        button.BackgroundColor = Color.FromHex("cc000000");
                    else
                        button.BackgroundColor = Color.FromHex("66000000");
                }
            }
        }

        private async void HandleRunSearch(object sender, EventArgs e)
        {
            plants = await App.WetlandPlantRepoLocal.GetAllSearchPlants();
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
                plants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
            else
                plants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.WetlandPlantsQuickSearch(e.NewTextValue));

            wetlandPlantsList.ItemsSource = plants;
        }

        private void SortPickerTapped(object sender, EventArgs e)
        {
            sortPicker.Focus();
        }

        private void SortOnUnfocused(object sender, FocusEventArgs e)
        {
            SortItems(sender, e);
        }

        private void SortItems(object sender, EventArgs e)
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

            App.WetlandSettingsRepo.AddOrUpdateSetting(new WetlandSetting { name = "Sort Field", valuetext = sortButton.Text, valueint = sortPicker.SelectedIndex });
            wetlandPlantsList.ItemTemplate = CellTemplate();
            wetlandPlantsList.ItemsSource = plants;
            FilterJumpList(sortButton.Text);
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

        public void FilterJumpList(string sortTerm)
        {
            if (plants.Count > 20)
            { 
                string fieldName = sortOptions.FirstOrDefault(x => x.Key == sortTerm).Value;
                var field = plants[0].GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == fieldName);
                var fieldFirstInitial = plants[0].GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == (fieldName + "FirstInitial"));

                var sortedPlants = from plant in plants orderby field.GetValue(plant).ToString() group plant by fieldFirstInitial.GetValue(plant).ToString() into plantGroup select new Grouping<string, WetlandPlant>(plantGroup.Key, plantGroup);
                plantsGrouped = new ObservableCollection<Grouping<string, WetlandPlant>>(sortedPlants);

                // Create jump list from termsGrouped
                jumpList = new List<string>();
                foreach (Grouping<string, WetlandPlant> index in plantsGrouped) { jumpList.Add(fieldFirstInitial.GetValue(index[0]).ToString()); };

                jumpListContainer.Children.Clear();
                foreach (string letter in jumpList)
                {
                    Label letterLabel = new Label { Text = letter, Style = Application.Current.Resources["jumpListLetter"] as Style };
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, e) => {
                        var firstRecordMatchingLetter = plants.FirstOrDefault(x => fieldFirstInitial.GetValue(x).ToString() == letter);
                        wetlandPlantsList.ScrollTo(firstRecordMatchingLetter, ScrollToPosition.Start, false);
                    };
                    letterLabel.GestureRecognizers.Add(tapGestureRecognizer);
                    jumpListContainer.Children.Add(letterLabel);
                }
            }
            else
                jumpListContainer.Children.Clear();
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

    public class CustomSearchBar : SearchBar
    {

    }

}
