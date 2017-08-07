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
        private bool isConnected;
        private bool isConnectedToWiFi;
        private Grid innerContainer;
        private Switch downloadImagesSwitch;
        private WetlandSetting downloadImagesSetting;
        private int numberOfPlants;
        private bool updatePlants = false;
        DownloadWetlandPlantsPage downloadPage;
        private bool finishedDownload = false;
        private bool canceledDownload = false;
        private WetlandSetting datePlantDataUpdatedLocally;
        private WetlandSetting datePlantDataUpdatedOnServer;
        private List<WetlandSetting> imageFilesToDownload = new List<WetlandSetting>();
        private IEnumerable<WetlandSetting> imageFileSettingsOnServer;

        protected override async void OnAppearing()
        {
            // Initiate variables
            isConnected = Connectivity.checkConnection();
            isConnectedToWiFi = Connectivity.checkWiFiConnection();
            downloadImagesSetting = await App.WetlandSettingsRepo.GetSettingAsync("Download Images");
            downloadImages = (bool)downloadImagesSetting.valuebool;
            downloadImagesSwitch.IsToggled = downloadImages;
            numberOfPlants = new List<WetlandPlant>(App.WetlandPlantRepo.GetAllWetlandPlants()).Count;

            // if connected to WiFi and updates are needed, show download button
            if (isConnected && !canceledDownload && !finishedDownload)
            {
                datePlantDataUpdatedLocally = App.WetlandSettingsRepo.GetSetting("Date Plants Downloaded");
                try
                {
                    datePlantDataUpdatedOnServer = await externalConnection.GetDateUpdatedDataOnServer();
                    imageFileSettingsOnServer = await externalConnection.GetImageZipFileSettings();
                    ImageFilesToDownload();
                }
                catch { }

                // If valid date comparison and date on server is more recent than local date, show download button
                if (datePlantDataUpdatedLocally != null && datePlantDataUpdatedOnServer != null)
                {
                    if (datePlantDataUpdatedLocally.valuetimestamp < datePlantDataUpdatedOnServer.valuetimestamp || numberOfPlants == 0)
                    {
                        updatePlants = true;
                        ToDownloadPage();
                    }
                }

                // If there are image zip files to download and the user has selected to downloaded images (and updatePlants has not triggered adding of download page to the stack)
                if (imageFilesToDownload != null && downloadImages == true && !updatePlants)
                {
                    updatePlants = false;
                    if (imageFilesToDownload.Count > 0)
                        ToDownloadPage();
                }

                //// If can't get setting on server, add connection error message
                //else if (datePlantDataUpdatedOnServer == null)
                //{
                //    innerContainer.Children.Add(new Label { Text = "Could not connect to server, please try again at a later time.", TextColor = Color.White, FontSize = 13, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(20, 0, 20, 0) }, 0, 2);
                //}
                //// If in doubt, add button to ensure data is updated
                //else
                //{
                //    if (datePlantDataUpdatedLocally == null) { updatePlants = true; }
                //    innerContainer.Children.Add(downloadDataButton, 0, 2);
                //}

                // If image data needs to be downloaded, show button
            }
            else
            {
                if (numberOfPlants == 0)
                    await DisplayAlert("Connection Needed", "Please connect to WiFi or cell network to download plant data and images", "OK");
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
            HeaderNavigationOptions navOptions = new HeaderNavigationOptions { titleText = "COLORADO WETLANDS", logoVisible = true };
            Grid navigationBar = ConstructNavigationBar(navOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);

            // Add empty space
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            
            // Add navigation buttons
            Button howToUseButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "HOW TO USE"
            };
            howToUseButton.Clicked += ToHowToUse;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(howToUseButton, 0, 2);

            Button whatAreWetlandsButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "WHAT ARE WETLANDS?"
            };
            whatAreWetlandsButton.Clicked += ToWhatAreWetlands;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(whatAreWetlandsButton, 0, 3);

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

            Button acknowledgementsButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "ACKNOWLEDGEMENTS"
            };
            acknowledgementsButton.Clicked += ToAcknowledgements;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(acknowledgementsButton, 0, 6);

            // Switch for downloading images
            StackLayout downloadImagesLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Margin = new Thickness(20, 0, 20, 0), HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            downloadImagesSwitch = new Switch { BackgroundColor = Color.FromHex("66000000") };
            downloadImagesSwitch.Toggled += ToggleDownloadImagesSwitch;
            Label downloadImagesLabel = new Label { Text = "Download Images", TextColor = Color.White };
            downloadImagesLayout.Children.Add(downloadImagesLabel);
            downloadImagesLayout.Children.Add(downloadImagesSwitch);            
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(downloadImagesLayout, 0, 7);

            // Add empty space
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Add bottom icons
            Grid bottomIcons = new Grid();
            bottomIcons.RowDefinitions.Add(new RowDefinition { Height = new GridLength(75) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(75) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(75) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(75) });
            bottomIcons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Image CNHPButton = new Image { Source = ImageSource.FromResource("PortableApp.Resources.Icons.CNHP.png") };
            bottomIcons.Children.Add(CNHPButton, 1, 0);
            Image EPAButton = new Image { Source = ImageSource.FromResource("PortableApp.Resources.Icons.EPA.png") };
            bottomIcons.Children.Add(EPAButton, 3, 0);
            Image CSUButton = new Image { Source = ImageSource.FromResource("PortableApp.Resources.Icons.CSU.png") };
            bottomIcons.Children.Add(CSUButton, 5, 0);

            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(75) });
            innerContainer.Children.Add(bottomIcons, 0, 9);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        private async void ToDownloadPage()
        {
            downloadPage = new DownloadWetlandPlantsPage(updatePlants, datePlantDataUpdatedLocally, datePlantDataUpdatedOnServer, imageFilesToDownload, downloadImages);
            downloadPage.InitCancelDownload += HandleCancelDownload;
            downloadPage.InitFinishedDownload += HandleFinishedDownload;
            await Navigation.PushModalAsync(downloadPage);
        }

        private async void HandleFinishedDownload(object sender, EventArgs e)
        {
            finishedDownload = true;
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void HandleCancelDownload(object sender, EventArgs e)
        {
            canceledDownload = true;
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
        
        public void ImageFilesToDownload()
        {
            foreach (WetlandSetting imageFile in imageFileSettingsOnServer)
            {
                WetlandSetting imageFileLocalSetting = App.WetlandSettingsRepo.GetImageZipFileSetting(imageFile.valuetext);
                if (imageFileLocalSetting == null)
                    imageFilesToDownload.Add(imageFile);
            }
        }

        private async void ToggleDownloadImagesSwitch(object sender, ToggledEventArgs e)
        {
            if (downloadImagesSwitch.IsToggled == true)
            {
                downloadImagesSetting.valuebool = true;
                if (imageFilesToDownload.Count > 0)
                    ToDownloadPage();
            }                
            else
                downloadImagesSetting.valuebool = false;
            
            await App.WetlandSettingsRepo.AddOrUpdateSettingAsync(downloadImagesSetting);
        }
    }
}
