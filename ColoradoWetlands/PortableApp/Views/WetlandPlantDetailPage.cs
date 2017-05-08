using PortableApp.Models;
using PortableApp.Views;
using Xamarin.Forms;

namespace PortableApp.Views
{
    public partial class WetlandPlantDetailPage : TabbedPage
    {
        public WetlandPlantDetailPage(WetlandPlant plant)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            var helpers = new ViewHelpers();

            Children.Add(new WetlandPlantInfoPage(plant) { Title = "INFO", Icon = "info.png" });
            Children.Add(new WetlandPlantImagesPage(plant) { Title = "IMAGES", Icon = "images.png" });
            Children.Add(new WetlandPlantImagesPage(plant) { Title = "SIMILAR", Icon = "similar.png" });
            Children.Add(new WetlandPlantImagesPage(plant) { Title = "ECOLOGY", Icon = "ecology.png" });
            Children.Add(new WetlandPlantImagesPage(plant) { Title = "RANGE", Icon = "range.png" });
            BarBackgroundColor = Color.Black;
            BarTextColor = Color.White;
        }
    }
}
