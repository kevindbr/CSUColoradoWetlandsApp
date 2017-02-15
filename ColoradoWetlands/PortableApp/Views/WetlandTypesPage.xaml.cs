using System;
using System.Collections.ObjectModel;
using System.Linq;
using PortableApp.Models;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandTypesPage : ContentPage
    {
    
        public WetlandTypesPage()
        {
            InitializeComponent();
            ObservableCollection<WetlandType> wetlandTypes = new ObservableCollection<WetlandType>(App.WetlandTypeRepo.GetAllWetlandTypes());
            wetlandTypesList.ItemsSource = wetlandTypes;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (wetlandTypesList.SelectedItem != null)
            {
                var detailPage = new WetlandTypesDetailPage();
                detailPage.BindingContext = e.SelectedItem as WetlandType;
                wetlandTypesList.SelectedItem = null;
                await Navigation.PushModalAsync(detailPage);
            }
        }

        //public async void OnNewButtonClicked(object sender, EventArgs args)
        //{
        //    statusMessage.Text = "";
        //    await App.PlantTypeRepo.AddNewPlantTypeAsync(newPlantType.Text);
        //    statusMessage.Text = App.PlantTypeRepo.StatusMessage;
        //}

        //public async void OnGetButtonClicked(object sender, EventArgs args)
        //{
        //    statusMessage.Text = "";
        //    ObservableCollection<PlantType> plantTypes = new ObservableCollection<PlantType>(await App.PlantTypeRepo.GetAllPlantTypesAsync());
        //    plantTypesList.ItemsSource = plantTypes;
        //}
    }
}
