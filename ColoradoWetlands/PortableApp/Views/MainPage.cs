using Plugin.Connectivity;
using PortableApp.Data;
using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Enums;
using XLabs.Forms.Controls;

namespace PortableApp
{
    public partial class MainPage : ViewHelpers
    {
        private bool isConnectedToWiFi;
        private Grid innerContainer;
        private Button downloadDataButton;
        private int numberOfPlants;
        private WetlandSetting datePlantDataUpdatedLocally;
        private WetlandSetting datePlantDataUpdatedOnServer;

        protected override async void OnAppearing()
        {
            isConnectedToWiFi = Connectivity.checkWiFiConnection();
            numberOfPlants = new List<WetlandPlant>(App.WetlandPlantRepo.GetAllWetlandPlants()).Count;

            // if connected to WiFi and updates are needed, show download button
            if (isConnectedToWiFi)
            {
                datePlantDataUpdatedLocally = await App.WetlandSettingsRepo.GetSettingAsync("DatePlantsDownloaded");
                datePlantDataUpdatedOnServer = await externalConnection.GetDateUpdatedDataOnServer();
                if (datePlantDataUpdatedLocally != null && datePlantDataUpdatedOnServer != null)
                {
                    if (datePlantDataUpdatedLocally.valuetimestamp < datePlantDataUpdatedOnServer.valuetimestamp || numberOfPlants == 0)
                    {
                        innerContainer.Children.Add(downloadDataButton, 0, 2);
                    }
                    else
                    {
                        innerContainer.Children.Remove(downloadDataButton);
                    }
                }
                else if (datePlantDataUpdatedOnServer == null)
                {
                    innerContainer.Children.Add(new Label { Text = "Could not connect to server, please try again at a later time.", TextColor = Color.White, FontSize = 13, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(20, 0, 20, 0) }, 0, 2);
                }
                else
                {
                    innerContainer.Children.Add(downloadDataButton, 0, 2);
                }                
            }
            else
            {
                if (numberOfPlants == 0)
                    await DisplayAlert("Connect to WiFi", "Please connect to WiFi to download plant data and images", "OK");
            }

            base.OnAppearing();
        }

        public MainPage()
        {
            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            NavigationOptions navOptions = new NavigationOptions { titleText = "COLORADO WETLANDS", logoVisible = true };
            Grid navigationBar = ConstructNavigationBar(navOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);

            // Add empty space
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.6, GridUnitType.Star) });

            // Add download button
            downloadDataButton = new ImageButton
            {
                Text = "DOWNLOAD DATA",
                FontSize = 15,
                TextColor = Color.Black,
                BackgroundColor = Color.FromHex("ccc9d845"),
                WidthRequest = 300,
                Margin = new Thickness(50, 0, 50, 0),
                Orientation = ImageOrientation.ImageToRight,
                Image = "download.png"
            };
            downloadDataButton.Clicked += DownloadData;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });

            // Add navigation buttons
            Button introductionButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "INTRODUCTION"
            };
            introductionButton.Clicked += ToIntroduction;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(introductionButton, 0, 3);

            Button wetlandPlantsButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "WETLAND PLANTS"
            };
            wetlandPlantsButton.Clicked += ToWetlandPlants;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(wetlandPlantsButton, 0, 4);

            Button wetlandMapsButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "WETLAND MAPS"
            };
            wetlandMapsButton.Clicked += ToWetlandMaps;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(wetlandMapsButton, 0, 5);

            Button wetlandTypesButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "WETLAND TYPES"
            };
            wetlandTypesButton.Clicked += ToWetlandTypes;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(wetlandTypesButton, 0, 6);

            Button acknowledgementsButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "ACKNOWLEDGEMENTS"
            };
            acknowledgementsButton.Clicked += ToAcknowledgements;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(acknowledgementsButton, 0, 7);

            // Add empty space
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Add bottom icons
            Grid bottomIcons = new Grid();
            bottomIcons.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Image CNHPButton = new Image { Source = ImageSource.FromResource("PortableApp.Resources.Icons.CNHP.png") };
            bottomIcons.Children.Add(CNHPButton, 1, 0);
            Image EPAButton = new Image { Source = ImageSource.FromResource("PortableApp.Resources.Icons.EPA.png") };
            bottomIcons.Children.Add(EPAButton, 3, 0);
            Image CSUButton = new Image { Source = ImageSource.FromResource("PortableApp.Resources.Icons.CSU.png") };
            bottomIcons.Children.Add(CSUButton, 5, 0);

            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(bottomIcons, 0, 9);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        public async void DownloadData(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DownloadWetlandPlantsPage(datePlantDataUpdatedOnServer.valuetimestamp));
        }
    }
}
