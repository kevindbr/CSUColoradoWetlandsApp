using System;
using System.Collections.Generic;
using PortableApp.Models;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace PortableApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage(string dbPath)
        {
            InitializeComponent();            
        }

        public async void ToIntroduction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HTMLPage("Introduction.html"));
        }

        public async void ToWetlandPlants(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WetlandPlantsPage());
        }

        public async void ToWetlandTypes(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WetlandTypesPage());
        }

        public async void ToAcknowledgements(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HTMLPage("Acknowledgements.html"));
        }

        //public void OnNewButtonClicked(object sender, EventArgs args)
        //{
        //    statusMessage.Text = "";

        //    App.PlantRepo.AddNewPlant(newPlant.Text);
        //    statusMessage.Text = App.PlantRepo.StatusMessage;
        //}

        //public void OnGetButtonClicked(object sender, EventArgs args)
        //{
        //    statusMessage.Text = "";

        //    ObservableCollection<Plant> plants = new ObservableCollection<Plant>(App.PlantRepo.GetAllPlants());
        //    plantList.ItemsSource = plants;
        //}
    }
}
