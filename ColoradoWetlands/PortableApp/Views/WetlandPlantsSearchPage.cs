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
        Entry minElev;
        Entry maxElev;
        Entry gRank;
        Entry sRank;
        ObservableCollection<WetlandPlant> allPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
        ObservableCollection<WetlandPlant> elevOverlap = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
        ObservableCollection<WetlandPlant> minElevPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
        ObservableCollection<WetlandPlant> maxElevPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
        ObservableCollection<WetlandPlant> countyPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
        ObservableCollection<WetlandPlant> wetlandTypePlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
        ObservableCollection<WetlandPlant> rankOverlap = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
        ObservableCollection<WetlandPlant> gRankPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
        ObservableCollection<WetlandPlant> sRankPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());

        Button searchButton;

        Picker countyPicker = new Picker();
        Button countyButton = new Button { Style = Application.Current.Resources["semiTransparentPlantButton"] as Style, Text = "All Counties", BorderRadius = Device.OnPlatform(0, 1, 0) };
        List<String> countyOptions = new List<string>() { "Adams", "Alamosa", "Arapahoe", "Archuleta", "Baca", "Bent", "Boulder", "Broomfield", "Chaffee", "Cheyenne", "Clear Creek",
            "Conejos", "Costilla", "Crowley", "Custer", "Delta", "Denver", "Dolores", "Douglas", "Eagle", "Elbert", "El Paso", "Fremont", "Garfield", "Gilpin", "Grand", "Gunnison",
            "Hinsdale", "Huerfano", "Jackson", "Jefferson", "Kiowa", "Kit Carson", "La Plata", "Lake", "Larimer", "Las Animas", "Lincoln", "Logan", "Mesa ", "Mineral", "Moffat",
            "Montezuma", "Montrose", "Morgan", "Otero", "Ouray", "Park", "Phillips", "Pitkin", "Prowers", "Pueblo", "Rio Blanco", "Rio Grande", "Routt", "Saguache", "San Juan",
            "San Miguel", "Sedgwick", "Summit", "Teller", "Washington", "Weld", "Yuma" };

        Picker wetlandTypePicker = new Picker();
        Button wetlandTypeButton = new Button { Style = Application.Current.Resources["semiTransparentPlantButton"] as Style, Text = "All Types", BorderRadius = Device.OnPlatform(0, 1, 0) };
        List<String> wetlandTypeOptions = new List<string>() { "Marsh", "Wet Meadow", "Mesic Meadow", "Fen", "Playa", "Subalpine Riparian Woodland", "Subalpine Riparian Shrubland", "Foothills Riparian", "Plains Riparian", "Plains Floodplain", "Greasewood Flats", "Hanging Garden"};



        public WetlandPlantsSearchPage()
        {
            plants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
            searchCriteriaDB = new ObservableCollection<WetlandSearch>(App.WetlandSearchRepo.GetAllWetlandSearchCriteria());
            searchCriteria = SearchCharacteristicsCollection();

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            /*
            ScrollView contentScrollView = new ScrollView
            {
                Padding = new Thickness(20, Device.OnPlatform(30, 20, 20), 20, 20),
                BackgroundColor = Color.FromHex("88000000"),
            };*/

            Grid innerContainer = new Grid {
                RowSpacing = 10,
                BackgroundColor = Color.FromHex("88000000"),
            };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            StackLayout searchFilters = new StackLayout
            {
                Spacing = 3,
                Orientation = StackOrientation.Vertical
            };

            //Plant Size
            Label groupLabel = new Label { Text = "Plant Group:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(groupLabel);

            StackLayout groupLayout1 = new StackLayout { Orientation = StackOrientation.Horizontal };

            SearchCharacteristic woodyGroup = searchCriteria.First(x => x.Characteristic == "group-Woody");
            groupLayout1.Children.Add(woodyGroup);

            SearchCharacteristic dicotGroup = searchCriteria.First(x => x.Characteristic == "group-Dicot");
            groupLayout1.Children.Add(dicotGroup);

            StackLayout groupLayout2 = new StackLayout { Orientation = StackOrientation.Horizontal };

            SearchCharacteristic monocotGroup = searchCriteria.First(x => x.Characteristic == "group-Monocot");
            groupLayout2.Children.Add(monocotGroup);

            SearchCharacteristic aquaticGroup = searchCriteria.First(x => x.Characteristic == "group-Aquatic");
            groupLayout2.Children.Add(aquaticGroup);

            StackLayout groupLayout3 = new StackLayout { Orientation = StackOrientation.Horizontal };

            SearchCharacteristic rushesGroup = searchCriteria.First(x => x.Characteristic == "group-Rushes");
            groupLayout3.Children.Add(rushesGroup);

            SearchCharacteristic grassesGroup = searchCriteria.First(x => x.Characteristic == "group-Grasses");
            groupLayout3.Children.Add(grassesGroup);

            StackLayout groupLayout4 = new StackLayout { Orientation = StackOrientation.Horizontal };

            SearchCharacteristic fernsGroup = searchCriteria.First(x => x.Characteristic == "group-Ferns");
            groupLayout4.Children.Add(fernsGroup);

            SearchCharacteristic sedgesGroup = searchCriteria.First(x => x.Characteristic == "group-Sedges");
            groupLayout4.Children.Add(sedgesGroup);

            searchFilters.Children.Add(groupLayout1);
            searchFilters.Children.Add(groupLayout2);
            searchFilters.Children.Add(groupLayout3);
            searchFilters.Children.Add(groupLayout4);

            // Flower Color
            Label flowerColorLabel = new Label { Text = "Flower Color:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerColorLabel);

            WrapLayout flowerColorLayout1 = new WrapLayout
            {
                Orientation = StackOrientation.Horizontal

            };

            SearchCharacteristic yellowFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Yellow");
            yellowFlowerColor.BackgroundColor = Color.Yellow;
            yellowFlowerColor.TextColor = Color.Black;
            flowerColorLayout1.Children.Add(yellowFlowerColor);

            SearchCharacteristic blueFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Blue");
            blueFlowerColor.BackgroundColor = Color.Blue;
            blueFlowerColor.TextColor = Color.Black;
            flowerColorLayout1.Children.Add(blueFlowerColor);

            SearchCharacteristic redFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Red");
            redFlowerColor.BackgroundColor = Color.Red;
            redFlowerColor.TextColor = Color.Black;
            flowerColorLayout1.Children.Add(redFlowerColor);

            SearchCharacteristic orangeFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Orange");
            orangeFlowerColor.BackgroundColor = Color.Orange;
            orangeFlowerColor.TextColor = Color.Black;
            flowerColorLayout1.Children.Add(orangeFlowerColor);    

            SearchCharacteristic pinkFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Pink");
            pinkFlowerColor.BackgroundColor = Color.Pink;
            pinkFlowerColor.TextColor = Color.Black;
            flowerColorLayout1.Children.Add(pinkFlowerColor);

            SearchCharacteristic greenFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Green");
            greenFlowerColor.BackgroundColor = Color.Green;
            greenFlowerColor.TextColor = Color.Black;
            flowerColorLayout1.Children.Add(greenFlowerColor);

            SearchCharacteristic purpleFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Purple");
            purpleFlowerColor.BackgroundColor = Color.Purple;
            purpleFlowerColor.TextColor = Color.Black;
            flowerColorLayout1.Children.Add(purpleFlowerColor);

            SearchCharacteristic brownFlowerColor = searchCriteria.First(x => x.Characteristic == "color-Brown");
            brownFlowerColor.BackgroundColor = Color.SaddleBrown;
            brownFlowerColor.TextColor = Color.Black;
            flowerColorLayout1.Children.Add(brownFlowerColor);

            searchFilters.Children.Add(flowerColorLayout1);
                    

            //Leaf Arrangement
            Label leafArrangementLabel = new Label { Text = "Leaf Arrangement:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafArrangementLabel);

            WrapLayout leafArrangementLayout1 = new WrapLayout { Orientation = StackOrientation.Horizontal };

            SearchCharacteristic altArr = searchCriteria.First(x => x.Characteristic == "leafarrangement-Alternate");
            leafArrangementLayout1.Children.Add(altArr);

            SearchCharacteristic oppositeArr = searchCriteria.First(x => x.Characteristic == "leafarrangement-Opposite");
            leafArrangementLayout1.Children.Add(oppositeArr);

            SearchCharacteristic whorledArr = searchCriteria.First(x => x.Characteristic == "leafarrangement-Whorled");
            leafArrangementLayout1.Children.Add(whorledArr);

            SearchCharacteristic basalArr = searchCriteria.First(x => x.Characteristic == "leafarrangement-Basal");
            leafArrangementLayout1.Children.Add(basalArr);

            searchFilters.Children.Add(leafArrangementLayout1);


            //Leaf Divsion
            Label leafDivisonLabel = new Label { Text = "Leaf Division:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafDivisonLabel);

            WrapLayout leafDivisionLayout = new WrapLayout { Orientation = StackOrientation.Horizontal };

            SearchCharacteristic simpleDivision = searchCriteria.First(x => x.Characteristic == "leafdivision-Simple");
            leafDivisionLayout.Children.Add(simpleDivision);

            SearchCharacteristic compDivision = searchCriteria.First(x => x.Characteristic == "leafdivision-Compound");
            leafDivisionLayout.Children.Add(compDivision);

            searchFilters.Children.Add(leafDivisionLayout);

            //Leaf Shape
            Label leafShapeLabel = new Label { Text = "Leaf Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafShapeLabel);

            WrapLayout leafShapeLayout1 = new WrapLayout { Orientation = StackOrientation.Horizontal};

            SearchCharacteristic linearShape = searchCriteria.First(x => x.Characteristic == "leafshape-Linear");
            leafShapeLayout1.Children.Add(linearShape);

            SearchCharacteristic roundShape = searchCriteria.First(x => x.Characteristic == "leafshape-Round");
            leafShapeLayout1.Children.Add(roundShape);

            SearchCharacteristic wideBaseShape = searchCriteria.First(x => x.Characteristic == "leafshape-WideBase");
            leafShapeLayout1.Children.Add(wideBaseShape);

            SearchCharacteristic wideTipShape = searchCriteria.First(x => x.Characteristic == "leafshape-WideTip");
            leafShapeLayout1.Children.Add(wideTipShape);

            SearchCharacteristic lobedShape = searchCriteria.First(x => x.Characteristic == "leafshape-Lobed");
            leafShapeLayout1.Children.Add(lobedShape);

            SearchCharacteristic palmateShape = searchCriteria.First(x => x.Characteristic == "leafshape-Palmate");
            leafShapeLayout1.Children.Add(palmateShape);

            SearchCharacteristic pinnateShape = searchCriteria.First(x => x.Characteristic == "leafshape-Pinnate");
            leafShapeLayout1.Children.Add(pinnateShape);

            searchFilters.Children.Add(leafShapeLayout1);
            
                    
            //Plant Size
            Label plantSizeLabel = new Label { Text = "Plant Size:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(plantSizeLabel);

            WrapLayout plantSizeLayout1 = new WrapLayout { Orientation = StackOrientation.Horizontal};

            SearchCharacteristic verySmallSize = searchCriteria.First(x => x.Characteristic == "plantsize-VerySmall");
            plantSizeLayout1.Children.Add(verySmallSize);

            SearchCharacteristic smallSize = searchCriteria.First(x => x.Characteristic == "plantsize-Small");
            plantSizeLayout1.Children.Add(smallSize);

            SearchCharacteristic mediumSize = searchCriteria.First(x => x.Characteristic == "plantsize-Medium");
            plantSizeLayout1.Children.Add(mediumSize);
           
            SearchCharacteristic largeSize = searchCriteria.First(x => x.Characteristic == "plantsize-Large");
            plantSizeLayout1.Children.Add(largeSize);
            
            searchFilters.Children.Add(plantSizeLayout1);


            //Plant Size
            Label regionLabel = new Label { Text = "Plant Region:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(regionLabel);

            WrapLayout plantRegionLayout = new WrapLayout { Orientation = StackOrientation.Horizontal };

            SearchCharacteristic plainRegion = searchCriteria.First(x => x.Characteristic == "region-Plains");
            plantRegionLayout.Children.Add(plainRegion);

            SearchCharacteristic mountainRegion = searchCriteria.First(x => x.Characteristic == "region-Mountains");
            plantRegionLayout.Children.Add(mountainRegion);

            SearchCharacteristic plateauRegion = searchCriteria.First(x => x.Characteristic == "region-Plateau");
            plantRegionLayout.Children.Add(plateauRegion);

            searchFilters.Children.Add(plantRegionLayout);



            Label countyLabel = new Label { Text = "County:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(countyLabel);

            StackLayout countyLayout = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand };

            countyButton.Clicked += CountyPickerTapped;
            countyLayout.Children.Add(countyButton);


            countyOptions.Sort();
            countyOptions.Insert(0, "All Counties");
            foreach (string option in countyOptions) { countyPicker.Items.Add(option); }
            countyPicker.IsVisible = false;
            if (Device.OS == TargetPlatform.iOS)
                countyPicker.Unfocused += SortOnUnfocused;
            else
                countyPicker.SelectedIndexChanged += CountyItems;

            searchFilters.Children.Add(countyPicker);

            searchFilters.Children.Add(countyLayout);


            Label elevationLabel = new Label { Text = "Elevation (meters):", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(elevationLabel);

            StackLayout elavtionLayout = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand };

            minElev = new Entry
            {
                Placeholder = "Minimum",
                TextColor = Color.Black,
                BackgroundColor = Color.White
            };
            elavtionLayout.Children.Add(minElev);
            minElev.TextChanged += ProcessElevation;


            maxElev = new Entry
            {
                Placeholder = "Maximum",
                TextColor = Color.Black,
                BackgroundColor = Color.White
            };
            elavtionLayout.Children.Add(maxElev);
            maxElev.TextChanged += ProcessElevation;

            searchFilters.Children.Add(elavtionLayout);

            //Plant Size
            Label wetlandTypeLabel = new Label { Text = "Wetland Type:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(wetlandTypeLabel);

            StackLayout wetLandTypeLayout = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand };

            wetlandTypeButton.Clicked += WetlandTypePickerTapped;
            wetLandTypeLayout.Children.Add(wetlandTypeButton);

            wetlandTypeOptions.Insert(0, "All Types");
            foreach (string option in wetlandTypeOptions) { wetlandTypePicker.Items.Add(option); }
            wetlandTypePicker.IsVisible = false;
            if (Device.OS == TargetPlatform.iOS)
                wetlandTypePicker.Unfocused += SortOnUnfocused;
            else
                wetlandTypePicker.SelectedIndexChanged += WetlandTypeItems;

            searchFilters.Children.Add(wetlandTypePicker);

            searchFilters.Children.Add(wetLandTypeLayout);

         
            //Plant Size
            Label nativeLabel = new Label { Text = "Native Status:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(nativeLabel);

            WrapLayout nativityLayout = new WrapLayout { Orientation = StackOrientation.Horizontal };

            SearchCharacteristic nativePlants = searchCriteria.First(x => x.Characteristic == "nativity-Native");
            nativityLayout.Children.Add(nativePlants);

            SearchCharacteristic nonNativePlants = searchCriteria.First(x => x.Characteristic == "nativity-Non");
            nativityLayout.Children.Add(nonNativePlants);
            
            SearchCharacteristic watchListPlants = searchCriteria.First(x => x.Characteristic == "noxiousweed-WatchList");
            nativityLayout.Children.Add(watchListPlants);

            SearchCharacteristic listAPlants = searchCriteria.First(x => x.Characteristic == "noxiousweed-ListA");
            nativityLayout.Children.Add(listAPlants);

            SearchCharacteristic listBPlants = searchCriteria.First(x => x.Characteristic == "noxiousweed-ListB");
            nativityLayout.Children.Add(listBPlants);

            SearchCharacteristic listCPlants = searchCriteria.First(x => x.Characteristic == "noxiousweed-ListC");
            nativityLayout.Children.Add(listCPlants);
            
            searchFilters.Children.Add(nativityLayout);


            Label statusLabel = new Label { Text = "Wetland Status:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(statusLabel);

            WrapLayout statusLayout = new WrapLayout { Orientation = StackOrientation.Horizontal };

            SearchCharacteristic statusFAC = searchCriteria.First(x => x.Characteristic == "status-FAC");
            statusLayout.Children.Add(statusFAC);

            SearchCharacteristic statusFACW = searchCriteria.First(x => x.Characteristic == "status-FACW");
            statusLayout.Children.Add(statusFACW);

            SearchCharacteristic statusFACU = searchCriteria.First(x => x.Characteristic == "status-FACU");
            statusLayout.Children.Add(statusFACU);

            SearchCharacteristic statusNI = searchCriteria.First(x => x.Characteristic == "status-NI");
            statusLayout.Children.Add(statusNI);

            SearchCharacteristic statusOBL = searchCriteria.First(x => x.Characteristic == "status-OBL");
            statusLayout.Children.Add(statusOBL);

            SearchCharacteristic statusUPL = searchCriteria.First(x => x.Characteristic == "status-UPL");
            statusLayout.Children.Add(statusUPL);

            searchFilters.Children.Add(statusLayout);


            Label rankLabel = new Label { Text = "Global and State Rank:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(rankLabel);

            StackLayout rankLayout = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand };

            gRank = new Entry
            {
                Placeholder = "Global Rank (ie G1)",
                TextColor = Color.Black,
                BackgroundColor = Color.White
            };
            rankLayout.Children.Add(gRank);
            gRank.TextChanged += ProcessRank;


            sRank = new Entry
            {
                Placeholder = "State Rank (ie S1)",
                TextColor = Color.Black,
                BackgroundColor = Color.White
            };
            rankLayout.Children.Add(sRank);
            sRank.TextChanged += ProcessRank;

            searchFilters.Children.Add(rankLayout);


            Label federalLabel = new Label { Text = "Federal Status:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(federalLabel);

            WrapLayout federalLayout = new WrapLayout { Orientation = StackOrientation.Horizontal };

            SearchCharacteristic usfsFederal = searchCriteria.First(x => x.Characteristic == "federal-USFS");
            federalLayout.Children.Add(usfsFederal);

            SearchCharacteristic threatenedFederal= searchCriteria.First(x => x.Characteristic == "federal-Threatened");
            federalLayout.Children.Add(threatenedFederal);

            SearchCharacteristic blmFederal = searchCriteria.First(x => x.Characteristic == "federal-BLM");
            federalLayout.Children.Add(blmFederal);

            searchFilters.Children.Add(federalLayout);

                 

            Label animalUseLabel = new Label { Text = "Animal Use:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(animalUseLabel);

            WrapLayout animalUseLayout = new WrapLayout { Orientation = StackOrientation.Horizontal };
          
            SearchCharacteristic amphibUse = searchCriteria.First(x => x.Characteristic == "animaluse-Amphibs");
            animalUseLayout.Children.Add(amphibUse);

            SearchCharacteristic insectUse= searchCriteria.First(x => x.Characteristic == "animaluse-Insects");
            animalUseLayout.Children.Add(insectUse);

            SearchCharacteristic waterfowlUse= searchCriteria.First(x => x.Characteristic == "animaluse-Waterfowl");
            animalUseLayout.Children.Add(waterfowlUse);

            SearchCharacteristic beaverUse= searchCriteria.First(x => x.Characteristic == "animaluse-Beaver");
            animalUseLayout.Children.Add(beaverUse);

            SearchCharacteristic deerUse= searchCriteria.First(x => x.Characteristic == "animaluse-Deer");
            animalUseLayout.Children.Add(deerUse);

            SearchCharacteristic passerineUse= searchCriteria.First(x => x.Characteristic == "animaluse-Passerines");
            animalUseLayout.Children.Add(passerineUse);

            SearchCharacteristic gameUse= searchCriteria.First(x => x.Characteristic == "animaluse-Game");
            animalUseLayout.Children.Add(gameUse);

            SearchCharacteristic craneUse= searchCriteria.First(x => x.Characteristic == "animaluse-Cranes");
            animalUseLayout.Children.Add(craneUse);

            SearchCharacteristic gullUse= searchCriteria.First(x => x.Characteristic == "animaluse-Gulls");
            animalUseLayout.Children.Add(gullUse);

            SearchCharacteristic grebeUse= searchCriteria.First(x => x.Characteristic == "animaluse-Grebes");
            animalUseLayout.Children.Add(grebeUse);

            searchFilters.Children.Add(animalUseLayout);

            ScrollView scrollView = new ScrollView()
            {
                Content = searchFilters,
                Orientation = ScrollOrientation.Vertical,
            };

            innerContainer.Children.Add(scrollView, 0, 0);

            // Add Search/Reset button group
            Grid searchButtons = new Grid();
            searchButtons.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            searchButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            searchButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Button resetButton = new Button { Text = "RESET", Style = Application.Current.Resources["semiTransparentWhiteButton"] as Style };
            resetButton.Clicked += ResetSearchFilters;
            searchButtons.Children.Add(resetButton, 0, 0);

            searchButton = new Button { Style = Application.Current.Resources["semiTransparentWhiteButton2"] as Style };
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

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        private async void ResetSearchFilters(object sender, EventArgs e)
        {
            foreach (var searchCrit in searchCriteria)
            {
                searchCrit.BorderColor = Color.White;
                searchCrit.Query = false;
                WetlandSearch correspondingDBRecord = searchCriteriaDB.First(x => x.Characteristic == searchCrit.Characteristic);
                correspondingDBRecord.Query = false;
                await App.WetlandSearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord);
            }
            minElev.Text = "";
            maxElev.Text = "";
            gRank.Text = "";
            sRank.Text = "";
            countyButton.Text = "All Counties";
            wetlandTypeButton.Text = "All Types";
            plants = await App.WetlandPlantRepoLocal.GetAllWetlandPlantsAsync();
            elevOverlap = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
            minElevPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
            maxElevPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
            rankOverlap = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
            sRankPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
            gRankPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
            countyPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
            wetlandTypePlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
            searchButton.Text = "VIEW " + plants.Count() + " RESULTS";
        }

        private void CountyPickerTapped(object sender, EventArgs e)
        {
            countyPicker.Focus();
        }

        private void WetlandTypePickerTapped(object sender, EventArgs e)
        {
            wetlandTypePicker.Focus();
        }

        private void SortOnUnfocused(object sender, FocusEventArgs e)
        {
            CountyItems(sender, e);
        }

        private async void CountyItems(object sender, EventArgs e)
        {
            countyButton.Text = countyPicker.Items[countyPicker.SelectedIndex];
            string county = countyButton.Text;
            if (!county.Equals("All Counties"))
            {
                countyPlants = await App.WetlandPlantRepoLocal.FilterPlantsByCounty(county);
            }
            else
            {
                countyPlants = allPlants;
            }   


            ObservableCollection<WetlandPlant> searchPlants = await App.WetlandPlantRepoLocal.FilterPlantsBySearchCriteria();
            ObservableCollection<WetlandPlant> searchCombined = new ObservableCollection<WetlandPlant>();

            foreach (var plant in allPlants)
                    if (elevOverlap.Contains(plant) && searchPlants.Contains(plant) && countyPlants.Contains(plant) && wetlandTypePlants.Contains(plant) && rankOverlap.Contains(plant))
                        searchCombined.Add(plant);

            plants = searchCombined;
            searchButton.Text = "VIEW " + plants.Count() + " RESULTS";

        }

        private async void WetlandTypeItems(object sender, EventArgs e)
        {
            wetlandTypeButton.Text = wetlandTypePicker.Items[wetlandTypePicker.SelectedIndex];
            string type = wetlandTypeButton.Text;
            if (!type.Equals("All Types"))
            {
                wetlandTypePlants = await App.WetlandPlantRepoLocal.FilterPlantsByWetlandType(type);
            }
            else
            {
                wetlandTypePlants = allPlants;
            }


            ObservableCollection<WetlandPlant> searchPlants = await App.WetlandPlantRepoLocal.FilterPlantsBySearchCriteria();
            ObservableCollection<WetlandPlant> searchCombined = new ObservableCollection<WetlandPlant>();

            foreach (var plant in allPlants)
                if (elevOverlap.Contains(plant) && searchPlants.Contains(plant) && countyPlants.Contains(plant) && wetlandTypePlants.Contains(plant) && rankOverlap.Contains(plant))
                    searchCombined.Add(plant);

            plants = searchCombined;
            searchButton.Text = "VIEW " + plants.Count() + " RESULTS";

        }

        private async void ProcessElevation(object sender, TextChangedEventArgs e)
        {
            if (!(minElev.Text == "" && maxElev.Text == ""))
            {
                try
                {
                    if (minElev.Equals(sender))
                    {
                        minElevPlants = await App.WetlandPlantRepoLocal.FilterPlantsByElevation(Int32.Parse(minElev.Text), "min");
                    }
                }
                catch (FormatException error)
                {
                    minElevPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());

                    plants = await App.WetlandPlantRepoLocal.FilterPlantsBySearchCriteria();
                }
                try
                {
                    if (maxElev.Equals(sender))
                    {
                        maxElevPlants = await App.WetlandPlantRepoLocal.FilterPlantsByElevation(Int32.Parse(maxElev.Text), "max");
                    }
                }
                catch (FormatException error)
                {
                    maxElevPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());

                    plants = await App.WetlandPlantRepoLocal.FilterPlantsBySearchCriteria();
                }

                elevOverlap = new ObservableCollection<WetlandPlant>(minElevPlants.Intersect(maxElevPlants));
                ObservableCollection<WetlandPlant> searchPlants = await App.WetlandPlantRepoLocal.FilterPlantsBySearchCriteria();
                ObservableCollection<WetlandPlant> searchCombined = new ObservableCollection<WetlandPlant>();


                foreach (var plant in allPlants)
                    if (elevOverlap.Contains(plant) && searchPlants.Contains(plant) && countyPlants.Contains(plant) && wetlandTypePlants.Contains(plant) && rankOverlap.Contains(plant))
                        searchCombined.Add(plant);

                plants = searchCombined;

                searchButton.Text = "VIEW " + plants.Count() + " RESULTS";
            }
        }

        private async void ProcessRank(object sender, TextChangedEventArgs e)
        {
            if (!(sRank.Text == "" && gRank.Text == ""))
            {
                try
                {
                    if (gRank.Equals(sender))
                    {
                        gRankPlants = await App.WetlandPlantRepoLocal.FilterPlantsByRank(gRank.Text.ToUpper(), "g");
                    }
                }
                catch (FormatException error)
                {
                    gRankPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
                    plants = await App.WetlandPlantRepoLocal.FilterPlantsBySearchCriteria();
                }
                try
                {
                    if (sRank.Equals(sender))
                    {
                        sRankPlants = await App.WetlandPlantRepoLocal.FilterPlantsByRank(sRank.Text.ToUpper(), "s");
                    }
                }
                catch (FormatException error)
                {
                    sRankPlants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepoLocal.GetAllWetlandPlants());
                    plants = await App.WetlandPlantRepoLocal.FilterPlantsBySearchCriteria();
                }

                rankOverlap = new ObservableCollection<WetlandPlant>(gRankPlants.Intersect(sRankPlants));
                ObservableCollection<WetlandPlant> searchPlants = await App.WetlandPlantRepoLocal.FilterPlantsBySearchCriteria();
                ObservableCollection<WetlandPlant> searchCombined = new ObservableCollection<WetlandPlant>();


                foreach (var plant in allPlants)
                    if (elevOverlap.Contains(plant) && searchPlants.Contains(plant) && countyPlants.Contains(plant) && wetlandTypePlants.Contains(plant) && rankOverlap.Contains(plant))
                        searchCombined.Add(plant);

                plants = searchCombined;

                searchButton.Text = "VIEW " + plants.Count() + " RESULTS";
            }
        }

        private async void ProcessSearchFilter(object sender, EventArgs e)
        {
            SearchCharacteristic button = (SearchCharacteristic)sender;
            WetlandSearch correspondingDBRecord = searchCriteriaDB.First(x => x.Characteristic == button.Characteristic);
            if (button.Query == true)
            {
                correspondingDBRecord.Query = button.Query = false;
                button.BorderWidth = 2;
                button.BorderColor = Color.White;
            }
            else if (button.Query == false)
            {
                correspondingDBRecord.Query = button.Query = true;
                button.BorderWidth = 2;
                button.BorderColor = Color.Green;
            }
            await App.WetlandSearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord);

            ObservableCollection<WetlandPlant> searchPlants = await App.WetlandPlantRepoLocal.FilterPlantsBySearchCriteria();
            ObservableCollection<WetlandPlant> searchCombined = new ObservableCollection<WetlandPlant>();


            foreach (var plant in allPlants)
                if (elevOverlap.Contains(plant) && searchPlants.Contains(plant) && countyPlants.Contains(plant) && wetlandTypePlants.Contains(plant) && rankOverlap.Contains(plant))
                    searchCombined.Add(plant);

            plants = searchCombined;

            searchButton.Text = "VIEW " + plants.Count() + " RESULTS";
        }

        private void RunSearch(object sender, EventArgs e)
        {
           App.WetlandPlantRepoLocal.setSearchPlants(plants.ToList());
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
                item.BorderWidth = 2;
                searchCriteria.Add(item);
            }            
            return searchCriteria;
        }

    }
}
