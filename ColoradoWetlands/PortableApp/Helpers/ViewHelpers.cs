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
                Opacity = 0.7
            };
            pageContainer.Children.Add(backgroundImage, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            return pageContainer;
        }

        // Construct Navigation Bar
        public Grid ConstructNavigationBar(NavigationOptions options)
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

            // Construct title content section
            StackLayout titleContent = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            Image logo = new Image { Source = ImageSource.FromResource("PortableApp.Resources.Icons.Co_Logo_40.png"), IsVisible = options.logoVisible };
            Label title = new Label { Text = options.titleText, FontFamily = Device.OnPlatform("Montserrat-Medium", "Montserrat-Medium.ttf#Montserrat-Medium", null), TextColor = Color.White, FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
            titleContent.Children.Add(logo);
            titleContent.Children.Add(title);
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            gridLayout.Children.Add(titleContent, 1, 0);

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
                gridLayout.Children.Add(homeImage, 2, 0);
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
                    gridLayout.Children.Add(previousImage, 2, 0);
                    gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    gridLayout.Children.Add(nextImage, 3, 0);
                }
                else if (plantIndex > 0)
                {
                    gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    gridLayout.Children.Add(previousImage, 2, 0);
                }
                else if (plantIndex < options.plants.Count - 1)
                {
                    gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    gridLayout.Children.Add(nextImage, 2, 0);
                }
            }
            else
            {
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0) });
            }
            
            return gridLayout;
        }

        public Grid ConstructFooterBar()
        {
            Grid gridLayout = new Grid { BackgroundColor = Color.FromHex("44000000"), VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, ColumnSpacing = 0 };
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Button terms = new Button {
                Text = "TERMS",
                Style = Application.Current.Resources["footerBarButton"] as Style,
                BorderRadius = Device.OnPlatform(0, 1, 0)
            };
            Device.OnPlatform(iOS: () => terms.Margin = new Thickness(5, 5, 5, 5));
            terms.Clicked += ToGlossary;
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(75) });
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

            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            return gridLayout;
        }

        public WebView HTMLProcessor(string location)
        {
            // Generate WebView container
            var browser = new TransparentWebView();
            //var pdfBrowser = new CustomWebView { Uri = location, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
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

        public async void ToIntroduction(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("Introduction.html", "INTRODUCTION"));
        }

        public async void ToWetlandPlants(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new WetlandPlantsPage());
        }

        public async void ToPlantsHelp(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("PlantsHelp.html", "PLANTS HELP"));
        }

        public async void ToWetlandMaps(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new WetlandPlantsPage());
        }

        public async void ToWetlandTypes(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new WetlandTypesPage());
        }

        public async void ToAcknowledgements(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("Acknowledgements.html", "ACKNOWLEDGEMENTS"));
        }

        public async void ToGlossary(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new WetlandGlossaryPage());
        }

    }

    public class NavigationOptions
    {
        public string titleText { get; set; }
        public bool backButtonVisible { get; set; }
        public bool homeButtonVisible { get; set; }
        public bool logoVisible { get; set; }
        public bool nextAndPreviousVisible { get; set; }
        public WetlandPlant plant { get; set; }
        public ObservableCollection<WetlandPlant> plants { get; set; }
    }

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
