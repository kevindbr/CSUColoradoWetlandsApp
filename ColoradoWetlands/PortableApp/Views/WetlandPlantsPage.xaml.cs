using System;
using System.Collections.ObjectModel;
using System.Linq;
using PortableApp.Models;
using Xamarin.Forms;

namespace PortableApp
{ 
    public partial class WetlandPlantsPage : ContentPage
    {

        public WetlandPlantsPage()
        {
            InitializeComponent();
            ObservableCollection<WetlandPlant> plants = new ObservableCollection<WetlandPlant>(App.WetlandPlantRepo.GetAllWetlandPlants());
            plantsList.ItemsSource = plants;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (plantsList.SelectedItem != null)
            {
                var selectedItem = e.SelectedItem as WetlandPlant;
                var detailPage = new PortableApp.Views.WetlandPlantDetailPage(selectedItem.Id);
                detailPage.BindingContext = selectedItem;
                plantsList.SelectedItem = null;
                await Navigation.PushAsync(detailPage);
            }
        }

        //public async void OnNewButtonClicked(object sender, EventArgs args)
        //{
        //    statusMessage.Text = "";
        //    await App.PlantRepo.AddNewPlantAsync(newPlant.Text);
        //    statusMessage.Text = App.PlantRepo.StatusMessage;
        //}

        //public async void OnGetButtonClicked(object sender, EventArgs args)
        //{
        //    statusMessage.Text = "";
        //    ObservableCollection<Plant> plants = new ObservableCollection<Plant>(await App.PlantRepo.GetAllPlantsAsync());
        //    plantsList.ItemsSource = plants;
        //}
    }
}
