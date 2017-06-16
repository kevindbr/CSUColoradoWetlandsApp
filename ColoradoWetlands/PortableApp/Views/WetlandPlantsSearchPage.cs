using PortableApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace PortableApp
{
    public partial class WetlandPlantsSearchPage : ViewHelpers
    {
        public EventHandler InitRunSearch;
        public EventHandler InitResetSearch;
        public EventHandler InitCloseSearch;

        public ObservableCollection<WetlandSearch> searchCriteria;
        
        public WetlandPlantsSearchPage()
        {
            searchCriteria = new ObservableCollection<WetlandSearch>(App.WetlandSearchRepo.GetAllWetlandSearchCriteria());

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid {
                Padding = new Thickness(20, Device.OnPlatform(30, 20, 20), 20, 20),
                BackgroundColor = Color.FromHex("88000000"),
                RowSpacing = 10
            };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            StackLayout searchFilters = new StackLayout { Spacing = 10 };

            // Add Family of Plant
            Label plantFamilyLabel = new Label { Text = "Family:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(plantFamilyLabel);

            StackLayout typesOfPlantsLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            ImageButton posceaeFamily = new ImageButton { Text = "Poaceae", TextColor = Color.White, BorderColor = Color.White };
            posceaeFamily.Clicked += ProcessSearchFilter;
            typesOfPlantsLayout.Children.Add(posceaeFamily);

            searchFilters.Children.Add(typesOfPlantsLayout);

            // Add Type of Plant
            Label commonnameLabel = new Label { Text = "Common Name:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(commonnameLabel);

            StackLayout commonnameLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            ImageButton bentgrassCommonname = new ImageButton { Text = "Bentgrass", TextColor = Color.White, BorderColor = Color.White };
            bentgrassCommonname.Clicked += ProcessSearchFilter;
            commonnameLayout.Children.Add(bentgrassCommonname);

            searchFilters.Children.Add(commonnameLayout);

            innerContainer.Children.Add(searchFilters, 0, 0);

            // Add Search/Reset button group
            Grid searchButtons = new Grid();
            searchButtons.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            searchButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            searchButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Button resetButton = new Button { Text = "RESET", Style = Application.Current.Resources["semiTransparentWhiteButton"] as Style };
            resetButton.Clicked += ResetSearchFilters;
            searchButtons.Children.Add(resetButton, 0, 0);

            Button searchButton = new Button { Text = "SEARCH", Style = Application.Current.Resources["semiTransparentWhiteButton"] as Style };
            searchButton.Clicked += RunSearch;
            searchButtons.Children.Add(searchButton, 1, 0);

            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(searchButtons, 0, 1);

            // Add Close button
            Button closeButton = new Button
            {
                Style = Application.Current.Resources["outlineButton"] as Style,
                Text = "CLOSE",
                BorderRadius = Device.OnPlatform(0, 1, 0)
            };
            closeButton.Clicked += CloseSearch;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(closeButton, 0, 2);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        private void ResetSearchFilters(object sender, EventArgs e)
        {
            InitResetSearch?.Invoke(this, EventArgs.Empty);
        }

        private async void ProcessSearchFilter(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            WetlandSearch buttonBinding = searchCriteria.Where(X => X.Name == button.Text).First();
            if (buttonBinding.Query == true)
            {
                buttonBinding.Query = false;
                button.BorderWidth = 0;
            }
            else if (buttonBinding.Query == false)
            {
                buttonBinding.Query = true;
                button.BorderWidth = 1;
            }
            await App.WetlandSearchRepo.UpdateSearchCriteriaAsync(buttonBinding);
        }

        private void RunSearch(object sender, EventArgs e)
        {
            InitRunSearch?.Invoke(this, EventArgs.Empty);
        }

        private void CloseSearch(object sender, EventArgs e)
        {
            InitCloseSearch?.Invoke(this, EventArgs.Empty);
        }

    }
}
