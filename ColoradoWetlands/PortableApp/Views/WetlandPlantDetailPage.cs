using PortableApp.Models;
using PortableApp.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp.Views
{
    public partial class WetlandPlantDetailPage : TabbedPage
    {
        public WetlandSetting selectedTabSetting = App.WetlandSettingsRepo.GetSetting("SelectedTab");

        public WetlandPlantDetailPage(WetlandPlant plant, ObservableCollection<WetlandPlant> plants)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            var helpers = new ViewHelpers();

            Children.Add(new WetlandPlantImagesPage(plant, plants) { Title = "IMAGES", Icon = "images.png" });
            Children.Add(new WetlandPlantInfoPage(plant, plants) { Title = "INFO", Icon = "info.png" });
            Children.Add(new WetlandPlantEcologyPage(plant, plants) { Title = "ECOLOGY", Icon = "ecology.png" });
            Children.Add(new WetlandPlantRangePage(plant, plants) { Title = "RANGE", Icon = "range.png" });
            Children.Add(new WetlandPlantSimilarPage(plant, plants) { Title = "SIMILAR", Icon = "similar.png" });
            BarBackgroundColor = Color.Black;
            BarTextColor = Color.White;

            if (selectedTabSetting != null)
                SelectedItem = Children[Convert.ToInt32(selectedTabSetting.valueint)];
            else
                SelectedItem = Children[0];

            this.CurrentPageChanged += RememberPageChange;
        }

        private async void RememberPageChange(object sender, EventArgs e)
        {
            int index = this.Children.IndexOf(this.CurrentPage);
            await App.WetlandSettingsRepo.AddOrUpdateSettingAsync(new WetlandSetting { name = "SelectedTab", valueint = (long?)index } );
        }
    }
}
