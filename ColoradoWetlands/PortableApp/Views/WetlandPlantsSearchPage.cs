using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace PortableApp
{
    public partial class WetlandPlantsSearchPage : ViewHelpers
    {
        public EventHandler InitRunSearch;
        public EventHandler InitCloseSearch;

        public ObservableCollection<WetlandPlant> plants;
        public ObservableCollection<WetlandSearch> searchCriteriaDB;
        public ObservableCollection<SearchCharacteristic> searchCriteria;

        Button searchButton;

        public WetlandPlantsSearchPage()
        {
            plants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
            searchCriteriaDB = new ObservableCollection<WetlandSearch>(App.WetlandSearchRepo.GetAllWetlandSearchCriteria());
            searchCriteria = SearchCharacteristicsCollection();

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container

            ScrollView contentScrollView = new ScrollView
            {
                Padding = new Thickness(20, Device.OnPlatform(30, 20, 20), 20, 20),
                BackgroundColor = Color.FromHex("88000000"),
            };

            Grid innerContainer = new Grid {
                RowSpacing = 10
            };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            StackLayout searchFilters = new StackLayout { Spacing = 3};

            
            /*// Add Family of Plant
            Label plantFamilyLabel = new Label { Text = "Family:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(plantFamilyLabel);

            StackLayout typesOfPlantsLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            ImageButton posceaeFamily = searchCriteria[0];
            posceaeFamily.Clicked += ProcessSearchFilter;
            typesOfPlantsLayout.Children.Add(posceaeFamily);
            
            searchFilters.Children.Add(typesOfPlantsLayout);*/
            
            // Flower Color
            Label flowerColorLabel = new Label { Text = "Flower Color:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerColorLabel);

            StackLayout flowerColorLayout1 = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0 };

            SearchCharacteristic yellowFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Yellow");
            flowerColorLayout1.Children.Add(yellowFlowerColor);

            SearchCharacteristic blueFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Blue");
            flowerColorLayout1.Children.Add(blueFlowerColor);

            SearchCharacteristic redFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Red");
            flowerColorLayout1.Children.Add(redFlowerColor);

            StackLayout flowerColorLayout2 = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0 };

            SearchCharacteristic orangeFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Orange");
            flowerColorLayout2.Children.Add(orangeFlowerColor);

            SearchCharacteristic pinkFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Pink");
            flowerColorLayout2.Children.Add(pinkFlowerColor);

            SearchCharacteristic greenFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Green");
            flowerColorLayout2.Children.Add(greenFlowerColor);

            StackLayout flowerColorLayout3 = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0 };

            SearchCharacteristic purpleFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Purple");
            flowerColorLayout3.Children.Add(purpleFlowerColor);

            SearchCharacteristic brownFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Brown");
            flowerColorLayout3.Children.Add(brownFlowerColor);

            searchFilters.Children.Add(flowerColorLayout1);
            searchFilters.Children.Add(flowerColorLayout2);
            searchFilters.Children.Add(flowerColorLayout3);
            
            //Leaf Divsion
            Label leafDivisonLabel = new Label { Text = "Leaf Division:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafDivisonLabel);

            StackLayout leafDivisionLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0 };

            SearchCharacteristic simpleDivision = searchCriteria.First(x => x.Characteristic == "leafdivision-Simple");
            leafDivisionLayout.Children.Add(simpleDivision);

            SearchCharacteristic compDivision = searchCriteria.First(x => x.Characteristic == "leafdivision-Compound");
            leafDivisionLayout.Children.Add(compDivision);

            searchFilters.Children.Add(leafDivisionLayout);
            
            //Leaf Shape
            Label leafShapeLabel = new Label { Text = "Leaf Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafShapeLabel);

            StackLayout leafShapeLayout1 = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0 };

            SearchCharacteristic linearShape = searchCriteria.First(x => x.Characteristic == "leafshape-Linear");
            leafShapeLayout1.Children.Add(linearShape);

            SearchCharacteristic roundShape = searchCriteria.First(x => x.Characteristic == "leafshape-Round");
            leafShapeLayout1.Children.Add(roundShape);

            SearchCharacteristic wideBaseShape = searchCriteria.First(x => x.Characteristic == "leafshape-WideBase");
            leafShapeLayout1.Children.Add(wideBaseShape);

            StackLayout leafShapeLayout2 = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0 };

            SearchCharacteristic wideTipShape = searchCriteria.First(x => x.Characteristic == "leafshape-WideTip");
            leafShapeLayout2.Children.Add(wideTipShape);

            SearchCharacteristic lobedShape = searchCriteria.First(x => x.Characteristic == "leafshape-Lobed");
            leafShapeLayout2.Children.Add(lobedShape);

            SearchCharacteristic palmateShape = searchCriteria.First(x => x.Characteristic == "leafshape-Palmate");
            leafShapeLayout2.Children.Add(palmateShape);

            StackLayout leafShapeLayout3 = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0 };

            SearchCharacteristic pinnateShape = searchCriteria.First(x => x.Characteristic == "leafshape-Pinnate");
            leafShapeLayout3.Children.Add(pinnateShape);

            searchFilters.Children.Add(leafShapeLayout1);
            searchFilters.Children.Add(leafShapeLayout2);
            searchFilters.Children.Add(leafShapeLayout3);
            
            //Leaf Arrangement
            Label leafArrangementLabel = new Label { Text = "Leaf Arrangement:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafArrangementLabel);

            StackLayout leafArrangementLayout1 = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0 };

            SearchCharacteristic altArr = searchCriteria.First(x => x.Characteristic == "leafarrangement-Alternate");
            leafArrangementLayout1.Children.Add(altArr);

            SearchCharacteristic oppositeArr = searchCriteria.First(x => x.Characteristic == "leafarrangement-Opposite");
            leafArrangementLayout1.Children.Add(oppositeArr);

            SearchCharacteristic whorledArr = searchCriteria.First(x => x.Characteristic == "leafarrangement-Whorled");
            leafShapeLayout1.Children.Add(whorledArr);

            StackLayout leafArrangementLayout2 = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0 };

            SearchCharacteristic basalArr = searchCriteria.First(x => x.Characteristic == "leafarrangement-Basal");
            leafArrangementLayout2.Children.Add(basalArr);

            searchFilters.Children.Add(leafArrangementLayout1);
            searchFilters.Children.Add(leafArrangementLayout2);

            //Plant Size
            Label plantSizeLabel = new Label { Text = "Plant Size:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(plantSizeLabel);

            StackLayout plantSizeLayout1 = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0 };

            SearchCharacteristic verySmallSize = searchCriteria.First(x => x.Characteristic == "plantsize-VerySmall");
            plantSizeLayout1.Children.Add(verySmallSize);

            SearchCharacteristic smallSize = searchCriteria.First(x => x.Characteristic == "plantsize-Small");
            plantSizeLayout1.Children.Add(smallSize);

            SearchCharacteristic mediumSize = searchCriteria.First(x => x.Characteristic == "plantsize-Medium");
            plantSizeLayout1.Children.Add(mediumSize);

            StackLayout plantSizeLayout2 = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0 };

            SearchCharacteristic largeSize = searchCriteria.First(x => x.Characteristic == "plantsize-Large");
            plantSizeLayout2.Children.Add(largeSize);
            
            searchFilters.Children.Add(plantSizeLayout1);
            searchFilters.Children.Add(plantSizeLayout2);
            
            innerContainer.Children.Add(searchFilters, 0, 0);

            // Add Search/Reset button group
            Grid searchButtons = new Grid();
            searchButtons.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            searchButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            searchButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Button resetButton = new Button { Text = "RESET", Style = Application.Current.Resources["semiTransparentWhiteButton"] as Style };
            resetButton.Clicked += ResetSearchFilters;
            searchButtons.Children.Add(resetButton, 0, 0);

            /*Button searchButton = new Button { Text = "SEARCH", Style = Application.Current.Resources["semiTransparentWhiteButton"] as Style };
            searchButton.Clicked += RunSearch;
            searchButtons.Children.Add(searchButton, 1, 0);*/

            searchButton = new Button { Style = Application.Current.Resources["semiTransparentWhiteButton"] as Style };
            searchButton.Text = "VIEW " + plants.Count() + " RESULTS";
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

            contentScrollView.Content = innerContainer;

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(contentScrollView, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        private async void ResetSearchFilters(object sender, EventArgs e)
        {
            foreach (var searchCrit in searchCriteria)
            {
                searchCrit.BorderWidth = 0;
                searchCrit.Query = false;
                WetlandSearch correspondingDBRecord = searchCriteriaDB.First(x => x.Characteristic == searchCrit.Characteristic);
                correspondingDBRecord.Query = false;
                await App.WetlandSearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord);
            }
            plants = await App.WetlandPlantRepoLocal.GetAllWetlandPlantsAsync();
            searchButton.Text = "VIEW " + plants.Count() + " RESULTS";
        }

        private async void ProcessSearchFilter(object sender, EventArgs e)
        {
            /*
            var button = (SearchCharacteristic)sender;
            var correspondingDBRecord = (WetlandSearch)sender;
            if (button.Query == true)
            {
                correspondingDBRecord.Query = false;
                button.BorderWidth = 0;
            }
            else if (button.Query == false)
            {
                correspondingDBRecord.Query = true;
                button.BorderWidth = 1;
            }
            await App.WetlandSearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord);*/
            // Update record in database and add or remove border
            SearchCharacteristic button = (SearchCharacteristic)sender;
            WetlandSearch correspondingDBRecord = searchCriteriaDB.First(x => x.Characteristic == button.Characteristic);
            if (button.Query == true)
            {
                correspondingDBRecord.Query = button.Query = false;
                button.BorderWidth = 0;
            }
            else if (button.Query == false)
            {
                correspondingDBRecord.Query = button.Query = true;
                button.BorderWidth = 1;
            }
            await App.WetlandSearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord);

            plants = await App.WetlandPlantRepoLocal.FilterPlantsBySearchCriteria();
            searchButton.Text = "VIEW " + plants.Count() + " RESULTS";

        }

        private void RunSearch(object sender, EventArgs e)
        {
            InitRunSearch?.Invoke(this, EventArgs.Empty);
        }

        private void CloseSearch(object sender, EventArgs e)
        {
            InitCloseSearch?.Invoke(this, EventArgs.Empty);
        }

        private ObservableCollection<SearchCharacteristic> SearchCharacteristicsCollection()
        {
            searchCriteria = new ObservableCollection<SearchCharacteristic>();
            foreach (WetlandSearch searchItem in searchCriteriaDB)
            {
                SearchCharacteristic item = new SearchCharacteristic();
                item.BindingContext = searchItem;
                item.SetBinding(SearchCharacteristic.CharacteristicProperty, new Binding("Characteristic"));
                item.SetBinding(SearchCharacteristic.TextProperty, new Binding("Name"));
                item.SetBinding(SearchCharacteristic.ImageProperty, new Binding("IconFileName"));
                item.SetBinding(SearchCharacteristic.QueryProperty, new Binding("Query"));
                item.SetBinding(SearchCharacteristic.Column1Property, new Binding("Column1"));
                item.SetBinding(SearchCharacteristic.SearchString1Property, new Binding("SearchString1"));
                item.Clicked += ProcessSearchFilter;
                item.BorderWidth = item.Query ? 1 : 0;
                searchCriteria.Add(item);
            }            
            return searchCriteria;
        }

    }
}
