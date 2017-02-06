using PortableApp.Models;
using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace PortableApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();            
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
