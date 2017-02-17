using PortableApp.Models;
using Xamarin.Forms;

namespace PortableApp.Views
{
    public partial class WetlandPlantDetailPage : TabbedPage
    {
        public WetlandPlantDetailPage(int plantId)
        {
            int PlantId = plantId;

            var imagesPage = new NavigationPage(new WetlandPlantImagesPage(PlantId));
            imagesPage.Title = "Images";

            var infoPage = new NavigationPage(new WetlandPlantInfoPage());
            infoPage.Title = "Info";

            Children.Add(imagesPage);
            Children.Add(infoPage);
            
        }
    }
}
