using CarouselView.FormsPlugin.Abstractions;
using FFImageLoading.Forms;
using PortableApp.Helpers;
using PortableApp.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandPlantImagesPage : ViewHelpers
    {

        public WetlandPlantImagesPage(WetlandPlant plant, ObservableCollection<WetlandPlant> plants)
        {
            System.GC.Collect();
            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            HeaderNavigationOptions navOptions = new HeaderNavigationOptions { titleText = plant.scinamenoauthorstripped, backButtonVisible = true, favoritesIconVisible = true, nextAndPreviousVisible = true, plant = plant, plants = plants };
            Grid navigationBar = ConstructNavigationBar(navOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);
            
            CarouselViewControl carouselControl = new CarouselViewControl();
            carouselControl.ItemsSource = plant.Images;
            carouselControl.ShowIndicators = true;

            DataTemplate imageTemplate = new DataTemplate(() =>
            {
                Grid cell = new Grid { BackgroundColor = Color.FromHex("88000000") };
                cell.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                cell.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
                cell.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                // Add image credit
                var imageCredit = new Label { FontSize = 14, TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(0, 10, 0, 0) };
                imageCredit.SetBinding(Label.TextProperty, new Binding("ImageCredit"));
                cell.Children.Add(imageCredit, 0, 0);

                // Add image
                var image = new ZoomImage { Margin = new Thickness(10, 0, 10, 0) };
                string imageBinding = downloadImages ? "ImagePathDownloaded" : "ImagePathStreamed";
                var cachedImage = new CachedImage()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 300,
                    HeightRequest = 300,
                    Aspect = Aspect.AspectFill,
                    Margin = new Thickness(10, 0, 10, 0),
                    CacheDuration = System.TimeSpan.FromDays(30),
                    DownsampleToViewSize = true,
                   RetryCount = 0,
                   
                    RetryDelay = 250,
                    TransparencyEnabled = false,
                    LoadingPlaceholder = "loading.png",
                    ErrorPlaceholder = "error.png",
                };
                image.SetBinding(Image.SourceProperty, new Binding(imageBinding));


                if (App.WetlandPlantImageRepoLocal.GetAllWetlandPlantImages() != null)
                    if (App.WetlandPlantImageRepoLocal.GetAllWetlandPlantImages().Count > 0)
                        imageBinding = "ThumbnailPathDownloaded";

                cell.Children.Add(image, 0, 1);

                return cell;
            });

            carouselControl.ItemTemplate = imageTemplate;
            carouselControl.Position = 0;
            carouselControl.InterPageSpacing = 10;
            carouselControl.Orientation = CarouselViewOrientation.Horizontal;
            
            innerContainer.RowDefinitions.Add(new RowDefinition { });
            innerContainer.Children.Add(carouselControl, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            Content = pageContainer;
            System.GC.Collect();

        }
        
    }
    
}
