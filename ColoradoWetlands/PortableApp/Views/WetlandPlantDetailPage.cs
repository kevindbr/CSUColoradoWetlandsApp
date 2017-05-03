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

            Children.Add(new WetlandPlantInfoPage(plant) { Title = "INFO" });
            Children.Add(new WetlandPlantImagesPage(plant) { Title = "IMAGES" });
            BarBackgroundColor = Color.Black;
            BarTextColor = Color.White;
        }
    }
}
