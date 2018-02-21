using PortableApp.Data;
using PortableApp.Models;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Diagnostics;
using PCLStorage;

namespace PortableApp
{
    public partial class MainPage : ViewHelpers, INotifyPropertyChanged
    {
    
        private bool isConnected;
        private bool isConnectedToWiFi;
        private Grid innerContainer;
        // private Switch downloadImagesSwitch;
        private Button downloadImagesButton = new Button { Style = Application.Current.Resources["semiTransparentButton"] as Style, Text = "Trying To Connect To Server..." };

        private WetlandSetting downloadImagesSetting;
        private int numberOfPlants;
        private bool updatePlants = false;
        private bool resyncPlants = false;
        private bool clearDatabase = false;

        IFolder rootFolder = FileSystem.Current.LocalStorage;

        DownloadWetlandPlantsPage downloadPage;
        private bool finishedDownload = false;
        private bool canceledDownload = false;
        private WetlandSetting datePlantDataUpdatedLocally;
        private WetlandSetting datePlantDataUpdatedOnServer;
        private List<WetlandSetting> imageFilesToDownload = new List<WetlandSetting>();
        private IEnumerable<WetlandSetting> imageFileSettingsOnServer;
        private Label downloadImagesLabel = new Label {TextColor = Color.White, BackgroundColor = Color.Transparent };
        private Label streamingLabel = new Label { Text = "You Are Streaming Plants", Style = Application.Current.Resources["sectionHeader"] as Style, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(0, 0, 0, 0) };

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        private string downloadButtonText = "Download Plant DB";
        public string DownloadButtonText
        {
            get
            {
                return this.downloadButtonText;
            }

            set
            {
                this.downloadButtonText = value;
                downloadImagesLabel.Text = this.downloadButtonText;
                OnPropertyChanged(new PropertyChangedEventArgs("DownloadButtonText"));
            }
        }


        protected override async void OnAppearing()
        {
            if (!canceledDownload)
            {
                // Initiate variables
                isConnected = Connectivity.checkConnection();
                isConnectedToWiFi = Connectivity.checkWiFiConnection();
                downloadImagesSetting = await App.WetlandSettingsRepo.GetSettingAsync("Download Images");
                downloadImages = (bool)downloadImagesSetting.valuebool;

                //numberOfPlants = new List<WetlandPlant>(App.WetlandPlantRepo.GetAllWetlandPlants()).Count;
                


                // if connected to WiFi and updates are needed
                if (isConnected)
                {
                    datePlantDataUpdatedLocally = App.WetlandSettingsRepo.GetSetting("Date Plants Downloaded");
                    try
                    {
                        datePlantDataUpdatedOnServer = await externalConnection.GetDateUpdatedDataOnServer();
                        imageFileSettingsOnServer = await externalConnection.GetImageZipFileSettings();
                        ImageFilesToDownload();

                        if (datePlantDataUpdatedLocally.valuetimestamp == datePlantDataUpdatedOnServer.valuetimestamp)
                        {
                            DownloadButtonText = "Plant DB Up To Date";
                            downloadImagesButton.Text = "Clear Local Database And Stream Plants";
                            streamingLabel.Text = "You Are Using Your Local Plant Database";
                            downloadImagesLabel.TextColor = Color.Green;
                            updatePlants = false;
                            resyncPlants = false;
                            clearDatabase = true;
                        }
                        else
                        {
                            if (datePlantDataUpdatedLocally.valuetimestamp == null)
                            {
                                DownloadButtonText = "Download Plant DB";
                                downloadImagesButton.Text = "Download (No Local Database)";
                                streamingLabel.Text = "You Are Streaming Plants";
                                downloadImagesLabel.TextColor = Color.Red;
                                updatePlants = true;
                                resyncPlants = false;
                                clearDatabase = false;
                            }
                            else if((datePlantDataUpdatedLocally.valuetimestamp < datePlantDataUpdatedOnServer.valuetimestamp) && datePlantDataUpdatedLocally.valuetimestamp != null)
                            {
                                DownloadButtonText = "New Plant DB Available";
                                downloadImagesButton.Text = "Re-Sync (New Database Available)";
                                streamingLabel.Text = "You Are Using Your Local Plant Database";
                                downloadImagesLabel.TextColor = Color.Yellow;
                                updatePlants = true;
                                resyncPlants = true;
                                clearDatabase = false;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Canceled UpdatePlants {0}", e.Message);
                    }
                }
                else
                {
                    if (numberOfPlants == 0)
                    {
                        await DisplayAlert("No Local Database Detected", "Please connect to WiFi or cell network to download or use CO Wetlands App", "OK");
                        updatePlants = false;
                        resyncPlants = false;
                        clearDatabase = false;
                    }
                    else
                    {
                        downloadImagesButton.Text = "No Internet Connection";
                    }
                }
            }
            else {
                canceledDownload = false;
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

            Button wetlandTypesButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "WETLAND TYPES"
            };
            wetlandTypesButton.Clicked += ToWetlandTypes;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(wetlandTypesButton, 0, 4);

            Button wetlandPlantsButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "WETLAND PLANTS"
            };
            wetlandPlantsButton.Clicked += ToWetlandPlants;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(wetlandPlantsButton, 0, 5);

            Button wetlandMapsButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "WETLAND MAPS"
            };
            wetlandMapsButton.Clicked += ToWetlandMaps;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(wetlandMapsButton, 0, 6);

            Button acknowledgementsButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "ACKNOWLEDGEMENTS"
            };
            acknowledgementsButton.Clicked += ToAcknowledgements;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(acknowledgementsButton, 0, 7);

            var semiTransparentBlack = new Color(0, 0, 0, 0.4);
            StackLayout downloadImagesLayout = new StackLayout { BackgroundColor = Color.Transparent, Orientation = StackOrientation.Vertical, Padding = new Thickness(5, 5, 5, 0), HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
    
                     //Button to download images
        
            downloadImagesButton.Clicked += DownloadImagesPressed;
            downloadImagesLayout.Children.Add(downloadImagesButton);

            //  downloadImagesLayout.Children.Add(downloadImagesLabel);
            
            downloadImagesLayout.Children.Add(streamingLabel);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(85) });
            innerContainer.Children.Add(downloadImagesLayout, 0, 8);


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
            innerContainer.Children.Add(bottomIcons, 0, 10);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
            System.GC.Collect();
        }

        private async void ToDownloadPage()
        {
            downloadPage = new DownloadWetlandPlantsPage(updatePlants, datePlantDataUpdatedLocally, datePlantDataUpdatedOnServer, imageFilesToDownload, downloadImages, resyncPlants, clearDatabase);
            downloadPage.InitCancelDownload += HandleCancelDownload;
            downloadPage.InitFinishedDownload += HandleFinishedDownload;
            await Navigation.PushModalAsync(downloadPage);
        }

        private async void HandleFinishedDownload(object sender, EventArgs e)
        {
            finishedDownload = true;
            datePlantDataUpdatedLocally.valuetimestamp = datePlantDataUpdatedOnServer.valuetimestamp;
            await App.WetlandSettingsRepo.AddOrUpdateSettingAsync(datePlantDataUpdatedLocally);
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void HandleCancelDownload(object sender, EventArgs e)
        {
            canceledDownload = true;
            //ClearRepositories();
            //ClearLocalRepositories();
            try
            {
                IFolder folder = await rootFolder.GetFolderAsync("Images");
                await folder.DeleteAsync();
            }
            catch (Exception exception) { }

            await App.Current.MainPage.Navigation.PopModalAsync();
        }
        
        //Im screwing with this
        public void ImageFilesToDownload()
        {
            foreach (WetlandSetting imageFile in imageFileSettingsOnServer)
            {
                WetlandSetting imageFileLocalSetting = App.WetlandSettingsRepo.GetImageZipFileSetting(imageFile.valuetext);
                if (imageFileLocalSetting == null)
                    imageFilesToDownload.Add(imageFile);
            }
        }

        private async void DownloadImagesPressed(object sender, EventArgs e)
        {
            if(clearDatabase)
            {             
                var answer = await DisplayAlert("Warning", "Are you sure you want to clear your database and stream plants?", "Yes", "No");
                if(answer)
                {
                    DownloadButtonText = "Plant DB Up To Date";
                    downloadImagesButton.Text = "Clearing Local Database...";
                    try
                    {
                        IFolder folder = await rootFolder.GetFolderAsync("Images");
                        await folder.DeleteAsync();
                    }catch (Exception exception) { }

                    ClearRepositories();
                    ClearLocalRepositories();                   
                    updatePlants = true;
                    resyncPlants = false;
                    clearDatabase = false;

                    datePlantDataUpdatedLocally.valuetimestamp = null;
                    await App.WetlandSettingsRepo.AddOrUpdateSettingAsync(datePlantDataUpdatedLocally);

                    DownloadButtonText = "Download Plant DB";
                    downloadImagesButton.Text = "Download (No Local Database)";
                    streamingLabel.Text = "You Are Streaming Plants";
                    downloadImagesLabel.TextColor = Color.Red;
                }
            }
            // If valid date comparison and date on server is more recent than local date, show download button
            else if (datePlantDataUpdatedOnServer.valuetimestamp != null)
            {
                if ((datePlantDataUpdatedLocally.valuetimestamp < datePlantDataUpdatedOnServer.valuetimestamp) || numberOfPlants == 0 || datePlantDataUpdatedLocally.valuetimestamp == null)
                {
                    updatePlants = true;
                    ToDownloadPage();
                }
            }
            await App.WetlandSettingsRepo.AddOrUpdateSettingAsync(downloadImagesSetting);
           
        }

        private void ClearRepositories()
        {
            //Clear Repositories
            App.WetlandPlantRepo.ClearWetlandPlants();
            App.WetlandGlossaryRepo.ClearWetlandGlossary();
            App.WetlandPlantImageRepo.ClearWetlandImages();
            App.WetlandPlantReferenceRepo.ClearWetlandPlantsReferences();
            App.WetlandPlantLeafArrangementRepo.ClearWetlandArrangements();
            App.WetlandPlantDivisionRepo.ClearWetlandDivisions();
            App.WetlandPlantFruitsRepo.ClearWetlandFruits();
            App.WetlandPlantShapeRepo.ClearWetlandShapes();
            App.WetlandPlantSizeRepo.ClearWetlandSizes();
            App.WetlandRegionRepo.ClearWetlandRegions();
            App.WetlandCountyPlantRepo.ClearWetlandCounties();
            App.WetlandPlantSimilarSpeciesRepo.ClearWetlandSimilarSpecies();
            App.WetlandSettingsRepo.ClearWetlandSettings();
        }

        private void ClearLocalRepositories()
        {
            try { App.WetlandPlantRepoLocal.ClearWetlandPlantsLocal(); } catch (Exception e) { }
            try { App.WetlandGlossaryRepoLocal.ClearWetlandGlossaryLocal(); } catch (Exception e) { }
            try { App.WetlandPlantImageRepoLocal.ClearWetlandImagesLocal(); } catch (Exception e) { }
            try { App.WetlandPlantReferenceRepoLocal.ClearWetlandReferencesLocal(); } catch (Exception e) { }
            try { App.WetlandPlantLeafArrangementRepoLocal.ClearWetlandArrangementsLocal(); } catch (Exception e) { }
            try { App.WetlandPlantFruitsRepoLocal.ClearWetlandFruitsLocal(); } catch (Exception e) { }
            try { App.WetlandPlantShapeRepoLocal.ClearWetlandShapesLocal(); } catch (Exception e) { }
            try { App.WetlandPlantSizeRepoLocal.ClearWetlandSizesLocal(); } catch (Exception e) { }
            try { App.WetlandRegionRepoLocal.ClearWetlandRegionsLocal(); } catch (Exception e) { }
            try { App.WetlandCountyPlantRepoLocal.ClearWetlandCountiesLocal(); } catch (Exception e) { }
            try { App.WetlandPlantDivisionRepoLocal.ClearWetlandDivisionsLocal(); } catch (Exception e) { } 
        }
    }
}
