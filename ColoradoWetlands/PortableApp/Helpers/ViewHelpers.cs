using PortableApp.Models;
using PortableApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{
    public class ViewHelpers : ContentPage
    {

        public ExternalDBConnection externalConnection = new ExternalDBConnection();
        public bool downloadImages = (bool)App.WetlandSettingsRepo.GetSetting("Download Images").valuebool;

        //
        // VIEWS
        //

        // Construct Page Container as an AbsoluteLayout with a background image
        public AbsoluteLayout ConstructPageContainer()
        {
            AbsoluteLayout pageContainer = new AbsoluteLayout { BackgroundColor = Color.Black };
            Image backgroundImage = new Image {
                Source = ImageSource.FromResource("PortableApp.Resources.Images.background.jpg"),
                Aspect = Aspect.AspectFill,
                Opacity = 0.6
            };
            pageContainer.Children.Add(backgroundImage, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            return pageContainer;
        }

        // Construct Navigation Bar
        public Grid ConstructNavigationBar(HeaderNavigationOptions options)
        {
            Grid gridLayout = new Grid { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, ColumnSpacing = 0 };
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            // Construct back button and add gesture recognizer
            if (options.backButtonVisible)
            {
                Image backImage = new Image
                {
                    Source = ImageSource.FromResource("PortableApp.Resources.Icons.back_arrow.png"),
                    HeightRequest = 20,
                    WidthRequest = 20,
                    Margin = new Thickness(0, 15, 0, 15)
                };
                var backGestureRecognizer = new TapGestureRecognizer();
                backGestureRecognizer.Tapped += async (sender, e) =>
                {
                    backImage.Opacity = .5;
                    await Task.Delay(200);
                    backImage.Opacity = 1;
                    await Navigation.PopAsync();
                };
                backImage.GestureRecognizers.Add(backGestureRecognizer);
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                gridLayout.Children.Add(backImage, 0, 0);
            }
            else
            {
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0) });
            }

            // Construct favorites icon and add gesture recognizer
            if (options.favoritesIconVisible)
            {
                string favoriteIcon = (options.plant.isFavorite) ? "favorite_icon_filled.png" : "favorite_icon_empty.png";
                string favoriteIconOpposite = (!options.plant.isFavorite) ? "favorite_icon_filled.png" : "favorite_icon_empty.png";
                Image favoriteImage = new Image
                {
                    Source = ImageSource.FromResource("PortableApp.Resources.Icons." + favoriteIcon),
                    HeightRequest = 20,
                    WidthRequest = 20,
                    Margin = new Thickness(0, 15, 0, 15)
                };
                var favoriteGestureRecognizer = new TapGestureRecognizer();
                favoriteGestureRecognizer.Tapped += async (sender, e) =>
                {
                    options.plant.isFavorite = (options.plant.isFavorite == true) ? false : true;
                    favoriteImage.Source = ImageSource.FromResource("PortableApp.Resources.Icons." + favoriteIconOpposite);
                    await App.WetlandPlantRepo.UpdatePlantAsync(options.plant);
                };
                favoriteImage.GestureRecognizers.Add(favoriteGestureRecognizer);
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                gridLayout.Children.Add(favoriteImage, 1, 0);
            }
            else
            {
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0) });
            }

            if (options.plantFiltersVisible)
            {
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
                gridLayout.Children.Add(options.plantFilterGroupButtons, 2, 0);
            }
            else
            {
                // Construct title content section
                StackLayout titleContent = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
                Image logo = new Image { Source = ImageSource.FromResource("PortableApp.Resources.Icons.Co_Logo_40.png"), IsVisible = options.logoVisible };
                Label title = new Label { Text = options.titleText, FontFamily = Device.OnPlatform("Montserrat-Medium", "Montserrat-Medium.ttf#Montserrat-Medium", null), TextColor = Color.White, FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
                titleContent.Children.Add(logo);
                titleContent.Children.Add(title);
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
                gridLayout.Children.Add(titleContent, 2, 0);
            }


            // Construct home button and add gesture recognizer
            if (options.homeButtonVisible)
            {
                Image homeImage = new Image
                {
                    Source = ImageSource.FromResource("PortableApp.Resources.Icons.home_icon.png"),
                    HeightRequest = 20,
                    WidthRequest = 20,
                    Margin = new Thickness(0, 15, 0, 15)
                };
                var homeImageGestureRecognizer = new TapGestureRecognizer();
                homeImageGestureRecognizer.Tapped += async (sender, e) =>
                {
                    homeImage.Opacity = .5;
                    await Task.Delay(200);
                    homeImage.Opacity = 1;
                    await Navigation.PopToRootAsync();
                    await Navigation.PushAsync(new MainPage());
                };
                homeImage.GestureRecognizers.Add(homeImageGestureRecognizer);
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                gridLayout.Children.Add(homeImage, 3, 0);
            }
            // if Next and Previous buttons visible
            else if (options.nextAndPreviousVisible && options.plants != null)
            {
                Image previousImage = new Image
                {
                    Source = ImageSource.FromResource("PortableApp.Resources.Icons.previous_icon.png"),
                    HeightRequest = 20,
                    WidthRequest = 20,
                    Margin = new Thickness(0, 15, 0, 15)
                };

                Image nextImage = new Image
                {
                    Source = ImageSource.FromResource("PortableApp.Resources.Icons.next_icon.png"),
                    HeightRequest = 20,
                    WidthRequest = 20,
                    Margin = new Thickness(0, 15, 0, 15)
                };

                int plantIndex = options.plants.IndexOf(options.plant);

                if (plantIndex > 0)
                {
                    WetlandPlant previousPlant = options.plants[plantIndex - 1];
                    
                    var previousImageGestureRecognizer = new TapGestureRecognizer();
                    previousImageGestureRecognizer.Tapped += async (sender, e) =>
                    {
                        previousImage.Opacity = .5;
                        await Task.Delay(200);
                        previousImage.Opacity = 1;
                        await Navigation.PushAsync(new WetlandPlantDetailPage(previousPlant, options.plants));
                        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    };
                    previousImage.GestureRecognizers.Add(previousImageGestureRecognizer);
                }

                if (plantIndex < options.plants.Count - 1)
                {
                    WetlandPlant nextPlant = options.plants[plantIndex + 1];

                    var nextImageGestureRecognizer = new TapGestureRecognizer();
                    nextImageGestureRecognizer.Tapped += async (sender, e) =>
                    {
                        nextImage.Opacity = .5;
                        await Task.Delay(200);
                        nextImage.Opacity = 1;
                        await Navigation.PushAsync(new WetlandPlantDetailPage(nextPlant, options.plants));
                        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    };
                    nextImage.GestureRecognizers.Add(nextImageGestureRecognizer);
                }

                // add to layout
                if (plantIndex > 0 && plantIndex < options.plants.Count - 1)
                {
                    gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    gridLayout.Children.Add(previousImage, 3, 0);
                    gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    gridLayout.Children.Add(nextImage, 4, 0);
                }
                else if (plantIndex > 0)
                {
                    gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    gridLayout.Children.Add(previousImage, 3, 0);
                }
                else if (plantIndex < options.plants.Count - 1)
                {
                    gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    gridLayout.Children.Add(nextImage, 3, 0);
                }
            }
            else
            {
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0) });
            }
            
            return gridLayout;
        }

        // Construct Footer Bar (for use in plants list and maps)
        public Grid ConstructFooterBar(FooterNavigationOptions options)
        {
            Grid gridLayout = new Grid { BackgroundColor = Color.FromHex("44000000"), VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, ColumnSpacing = 0 };
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            if (options.plantsFooter)
            {
                Button terms = new Button
                {
                    Text = "GLOSSARY",
                    Style = Application.Current.Resources["footerBarButton"] as Style,
                    BorderRadius = Device.OnPlatform(0, 1, 0)
                };
                Device.OnPlatform(iOS: () => terms.Margin = new Thickness(5, 5, 5, 5));
                terms.Clicked += ToGlossary;
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
                gridLayout.Children.Add(terms, 1, 0);

                Button maps = new Button
                {
                    Text = "MAPS",
                    Style = Application.Current.Resources["footerBarButton"] as Style,
                    BorderRadius = Device.OnPlatform(0, 1, 0)
                };
                Device.OnPlatform(iOS: () => maps.Margin = new Thickness(5, 5, 5, 5));
                maps.Clicked += ToWetlandMaps;
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
                gridLayout.Children.Add(maps, 2, 0);

                Button help = new Button
                {
                    Text = "?",
                    Style = Application.Current.Resources["footerBarButton"] as Style,
                    BorderRadius = Device.OnPlatform(0, 1, 0)
                };
                Device.OnPlatform(iOS: () => help.Margin = new Thickness(5, 5, 5, 5));
                help.Clicked += ToPlantsHelp;
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                gridLayout.Children.Add(help, 3, 0);
            }

            if (options.mapsFooter)
            {
                Button key = new Button
                {
                    Text = "KEY",
                    Style = Application.Current.Resources["footerBarButton"] as Style,
                    BorderRadius = Device.OnPlatform(0, 1, 0)
                };
                Device.OnPlatform(iOS: () => key.Margin = new Thickness(5, 5, 5, 5));
                key.Clicked += ToMapsKey;
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                gridLayout.Children.Add(key, 1, 0);

                Button legend = new Button
                {
                    Text = "LEGEND",
                    Style = Application.Current.Resources["footerBarButton"] as Style,
                    BorderRadius = Device.OnPlatform(0, 1, 0)
                };
                Device.OnPlatform(iOS: () => legend.Margin = new Thickness(5, 5, 5, 5));
                legend.Clicked += ToMapsLegend;
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(85) });
                gridLayout.Children.Add(legend, 2, 0);

                Button plants = new Button
                {
                    Text = "PLANTS",
                    Style = Application.Current.Resources["footerBarButton"] as Style,
                    BorderRadius = Device.OnPlatform(0, 1, 0)
                };
                Device.OnPlatform(iOS: () => plants.Margin = new Thickness(5, 5, 5, 5));
                plants.Clicked += ToWetlandPlants;
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(85) });
                gridLayout.Children.Add(plants, 3, 0);

                Button help = new Button
                {
                    Text = "?",
                    Style = Application.Current.Resources["footerBarButton"] as Style,
                    BorderRadius = Device.OnPlatform(0, 1, 0)
                };
                Device.OnPlatform(iOS: () => help.Margin = new Thickness(5, 5, 5, 5));
                help.Clicked += ToMapsHelp;
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                gridLayout.Children.Add(help, 4, 0);
            }            

            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            return gridLayout;
        }

        // Creates a flexible method to pass in a URL or file location (for PDFs) to display content
        public WebView HTMLProcessor(string location)
        {
            // Generate WebView container
            var browser = new TransparentWebView();
            string htmlText;

            // Get file locally unless the location is a web address
            if (location.Contains("http"))
            {
                htmlText = location;
                browser.Source = htmlText;
            }
            else if (!location.Contains(".pdf"))
            {
                // Get file from PCL--in order for HTML files to be automatically pulled from the PCL, they need to be in a Views/HTML folder
                var assembly = typeof(HTMLPage).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("PortableApp.Views.HTML." + location);
                htmlText = "";
                using (var reader = new System.IO.StreamReader(stream)) { htmlText = reader.ReadToEnd(); }
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = htmlText;
                browser.Source = htmlSource;
            }
            return browser;
        }

        // Change button collor when a button is clicked
        protected async void ChangeButtonColor(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.BackgroundColor = Color.FromHex("BB105456");
            await Task.Delay(100);
            button.BackgroundColor = Color.FromHex("66000000");
        }

        //
        // NAVIGATION
        //

        public async void ToHowToUse(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("HowToUse.html", "HOW TO USE"));
        }
        public async void ToPlantsHelp(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("HowToUsePlants.html", "HOW TO USE"));
        }
        public async void ToMapsHelp(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("HowToUseMaps.html", "HOW TO USE"));
        }

        public async void ToWetlandPlants(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new WetlandPlantsPage());
        }

        public async void ToGlossary(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new WetlandGlossaryPage());
        }

        public async void ToWetlandMaps(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new WetlandMapsPage());
        }

        public async void ToMapsKey(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushModalAsync(new WetlandMapsKeyPage());
        }

        public async void ToMapsLegend(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushModalAsync(new WetlandMapsLegendPage());
        }

        public async void ToWhatAreWetlands(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("WhatAreWetlands.html", "WHAT ARE WETLANDS?"));
            //await Navigation.PushAsync(new WhatAreWetlandsPage());
        }

        public async void ToWetlandTypes(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new WhatAreWetlandsPage());
        }

        public async void ToAcknowledgements(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("Acknowledgements.html", "ACKNOWLEDGEMENTS"));
        }

    }

    // HeaderNavigationOptions allows for dynamic construction of header based on the parameters passed (and the configuration of the properties below)
    public class HeaderNavigationOptions
    {
        public string titleText { get; set; }
        public bool plantFiltersVisible { get; set; }
        public Grid plantFilterGroupButtons { get; set; }
        public bool backButtonVisible { get; set; }
        public bool favoritesIconVisible { get; set; }
        public bool homeButtonVisible { get; set; }
        public bool logoVisible { get; set; }
        public bool nextAndPreviousVisible { get; set; }
        public WetlandPlant plant { get; set; }
        public ObservableCollection<WetlandPlant> plants { get; set; }
        public bool downloadImages { get; set; }
    }

    // Allows for clear direction on what to render on the Footer, based on whether it's the plants footer or maps footer
    public class FooterNavigationOptions
    {
        public bool plantsFooter { get; set; }
        public bool mapsFooter { get; set; }
    }

    // Sort method used for sorting plants list
    static class Extensions
    {
        public static void Sort<TSource, TKey>(this Collection<TSource> source, Func<TSource, TKey> keySelector, string sortDirection)
        {
            List<TSource> sortedList;
            if (sortDirection == "\u25B2")
            {
                sortedList = source.OrderByDescending(keySelector).ToList();
            }
            else
            {
                sortedList = source.OrderBy(keySelector).ToList();
            }
            source.Clear();
            foreach (var sortedItem in sortedList)
                source.Add(sortedItem);
        }
    }
}
