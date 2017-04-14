using PortableApp.Views;
using Xamarin.Forms;

namespace PortableApp.Views
{
    public partial class WetlandPlantDetailPage : TabbedPage
    {
        public WetlandPlantDetailPage(int plantId)
        { 
            //var helpers = new ViewHelpers();
            var navigationPage = new NavigationPage(new WetlandPlantImagesPage(plantId));
            navigationPage.Title = "IMAGES";

            Children.Add(navigationPage);
            Children.Add(new WetlandPlantInfoPage { Title = "INFO" });

            //// Turn off navigation bar and initialize pageContainer
            //NavigationPage.SetHasNavigationBar(this, false);
            //AbsoluteLayout pageContainer = helpers.ConstructPageContainer();

            //// Initialize grid for inner container
            //Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            //innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //// Add header to inner container
            //Grid navigationBar = helpers.ConstructNavigationBar("WETLAND TYPES", true, true, false);
            //innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            //innerContainer.Children.Add(navigationBar, 0, 0);

            //// Add inner container to page container and set as page content
            //pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            //ItemTemplate = pageContainer;
        }
    }
}
