using PortableApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class DownloadWetlandPlantsPage : ViewHelpers
    {
        ObservableCollection<WetlandPlant> plants;
        ProgressBar progressBar = new ProgressBar();
        Button cancelButton;

        protected async override void OnAppearing()
        {
            // Initialize progressbar and page
            progressBar.Progress = 0;
            base.OnAppearing();

            // Get all plants from external API call, store them in a collection
            plants = new ObservableCollection<WetlandPlant>(await externalConnection.GetAll());

            // Save plants to the database
            await UpdatePlants();

        }

        public DownloadWetlandPlantsPage()
        {
            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid {
                Padding = new Thickness(20, Device.OnPlatform(30, 20, 20), 20, 20),
                BackgroundColor = Color.FromHex("88000000")
            };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Add label
            var downloadLabel = new Label { Text = "Downloading Data...", TextColor = Color.White, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center };
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            innerContainer.Children.Add(downloadLabel, 0, 1);

            // Add progressbar
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(progressBar, 0, 2);

            // Add dismiss button
            cancelButton = new Button
            {
                Style = Application.Current.Resources["outlineButton"] as Style,
                Text = "CANCEL",
                BorderRadius = Device.OnPlatform(0, 1, 0)
            };
            //cancelButton.Clicked += OnCancelButtonClicked;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(cancelButton, 0, 3);

            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        //async void OnCancelButtonClicked(object sender, EventArgs args)
        //{
            
        //}

        // Iterate through plants and update progressbar
        public async Task UpdatePlants()
        {
            try
            {
                int plantsSaved = 0;
                foreach (var plant in plants)
                {
                    await App.WetlandPlantRepo.AddPlantAsync(plant);
                    plantsSaved += 1;
                    await progressBar.ProgressTo(plantsSaved / plants.Count, 500, Easing.Linear);
                }
                await App.WetlandSettingsRepo.AddOrUpdateSettingAsync(new WetlandSetting { name = "DatePlantsDownloaded", valuetimestamp = DateTime.Now });
                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (OperationCanceledException e)
            {
                Debug.WriteLine("Canceled UpdatePlants {0}", e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ex {0}", e.Message);
            }
        }
    }
}
