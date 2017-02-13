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
            //var plants = conn.GetAllPlants();
            //plantsList.ItemsSource = plants;
            InitializeComponent();
        }

        public async void OnNewButtonClicked(object sender, EventArgs args)
        {
            statusMessage.Text = "";
            await App.PlantRepo.AddNewPlantAsync(newPlant.Text);
            statusMessage.Text = App.PlantRepo.StatusMessage;
        }

        public async void OnGetButtonClicked(object sender, EventArgs args)
        {
            statusMessage.Text = "";
            ObservableCollection<Plant> plants = new ObservableCollection<Plant>(await App.PlantRepo.GetAllPlantsAsync());
            plantsList.ItemsSource = plants;
        }
    }
}
