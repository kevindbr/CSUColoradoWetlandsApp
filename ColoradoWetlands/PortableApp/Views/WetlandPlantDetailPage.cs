using PortableApp.Models;
using PortableApp.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp.Views
{
    public partial class WetlandPlantDetailPage : TabbedPage
    {
        public WetlandPlantDetailPage(WetlandPlant plant, ObservableCollection<WetlandPlant> plants)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            var helpers = new ViewHelpers();

            Children.Add(new WetlandPlantInfoPage(plant, plants) { Title = "INFO", Icon = "info.png" });
            Children.Add(new WetlandPlantImagesPage(plant, plants) { Title = "IMAGES", Icon = "images.png" });
            Children.Add(new WetlandPlantInfoPage(plant, plants) { Title = "SIMILAR", Icon = "similar.png" });
            Children.Add(new WetlandPlantInfoPage(plant, plants) { Title = "ECOLOGY", Icon = "ecology.png" });
            Children.Add(new WetlandPlantInfoPage(plant, plants) { Title = "RANGE", Icon = "range.png" });
            BarBackgroundColor = Color.Black;
            BarTextColor = Color.White;
        }
    }
}
