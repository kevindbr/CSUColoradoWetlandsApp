using PortableApp.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandPlantRangePage : ViewHelpers
    {

        public WetlandPlantRangePage(WetlandPlant plant, ObservableCollection<WetlandPlant> plants)
        {

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            HeaderNavigationOptions navOptions = new HeaderNavigationOptions { titleText = plant.scinamenoauthorstripped, backButtonVisible = true, nextAndPreviousVisible = true, plant = plant, plants = plants };
            Grid navigationBar = ConstructNavigationBar(navOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);
            
            ScrollView contentScrollView = new ScrollView {
                BackgroundColor = Color.FromHex("88000000"),
                Padding = new Thickness(0, 20, 0, 20),
                Margin = new Thickness(0, 0, 0, 0)
            };
            StackLayout contentContainer = new StackLayout();

            Image rangeImage = new Image { Aspect = Aspect.AspectFill, Margin = new Thickness(10, 0, 10, 0) };
            rangeImage.SetBinding(Image.SourceProperty, new Binding("RangePath"));
            contentContainer.Children.Add(rangeImage);

            Label elevationLabel = new Label {
                Text = "Elevation: " + plant.elevminfeet + "-" + plant.elevmaxfeet + " ft. (" + plant.elevminm + "-" + plant.elevmaxm + " m)",
                FontSize = 12,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center
            };
            contentContainer.Children.Add(elevationLabel);

            contentScrollView.Content = contentContainer;
            innerContainer.RowDefinitions.Add(new RowDefinition { });
            innerContainer.Children.Add(contentScrollView, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

    }
}
