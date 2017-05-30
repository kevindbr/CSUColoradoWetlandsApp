using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using PCLStorage;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace PortableApp
{
    public partial class DownloadWetlandPlantsPage : ViewHelpers
    {
        ObservableCollection<WetlandPlant> plants;
        ProgressBar progressBar = new ProgressBar();
        Label downloadLabel = new Label { Text = "", TextColor = Color.White, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center };
        Button cancelButton;
        HttpClient client = new HttpClient();

        protected async override void OnAppearing()
        {
            // Initialize progressbar and page
            progressBar.Progress = 0;
            base.OnAppearing();

            // Get all plants from external API call, store them in a collection
            plants = new ObservableCollection<WetlandPlant>(await externalConnection.GetAllPlants());

            // Save plants to the database
            await UpdatePlants();

            // Save images to the database
            await UpdatePlantImages();

            // Pop modal off stack (and return to WetlandPlantsPage)
            await App.Current.MainPage.Navigation.PopAsync();

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

        // Get plants from MobileApi server and save locally
        public async Task UpdatePlants()
        {
            try
            {
                downloadLabel.Text = "Downloading Plants...";
                int plantsSaved = 0;
                foreach (var plant in plants)
                {
                    App.WetlandPlantRepo.AddPlant(plant);
                    plantsSaved += 1;
                    await progressBar.ProgressTo(plantsSaved / plants.Count, 500, Easing.Linear);
                }
                await App.WetlandSettingsRepo.AddOrUpdateSettingAsync(new WetlandSetting { name = "DatePlantsDownloaded", valuetimestamp = DateTime.Now });
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

        // Get plant images from MobileApi server and save locally
        public async Task UpdatePlantImages()
        {
            // Declare variables
            string imagesZipFileLocation = PortableApp.ExternalDBConnection.localUrl + "/image_zip_files/";
            IEnumerable<WetlandSetting> imageFileSettingsOnServer = await externalConnection.GetImageZipFileSettings();
            long receivedBytes = 0;
            long? totalBytes = 0;

            // Set progressBar to 0 and downloadLabel text to "Downloading Images..."
            await progressBar.ProgressTo(0, 0, Easing.Linear);
            downloadLabel.Text = "Downloading Images...";

            try
            {
                // IFolder interface from PCLStorage; create or open imagesZipped folder (in Library/imagesZipped)    
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                IFolder folder = await rootFolder.CreateFolderAsync("Images", CreationCollisionOption.OpenIfExists);
                string folderPath = rootFolder + "Images";

                // Get image file setting records from MobileApi to determine which files to download
                // TODO: Limit this to only the files needed, not just every file
                totalBytes = imageFileSettingsOnServer.Sum(x => x.valueint);

                // For each setting, get the corresponding zip file and save it locally
                foreach (WetlandSetting imageFile in imageFileSettingsOnServer)
                {
                    Stream webStream = await client.GetStreamAsync(imagesZipFileLocation + imageFile.valuetext.Replace(".zip", ""));
                    ZipInputStream zipInputStream = new ZipInputStream(webStream);
                    ZipEntry zipEntry = zipInputStream.GetNextEntry();
                    while (zipEntry != null)
                    {
                        String entryFileName = zipEntry.Name;
                        // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                        // Optionally match entrynames against a selection list here to skip as desired.

                        byte[] buffer = new byte[4096];      

                        IFile file = await folder.CreateFileAsync(entryFileName, CreationCollisionOption.OpenIfExists);
                        using (Stream localStream = await file.OpenAsync(FileAccess.ReadAndWrite))
                        {
                            StreamUtils.Copy(zipInputStream, localStream, buffer);
                        }
                        receivedBytes += zipEntry.Size;
                        double percentage = (double)receivedBytes / (double)totalBytes;
                        await progressBar.ProgressTo(percentage, 0, Easing.Linear);
                        zipEntry = zipInputStream.GetNextEntry();
                    }
                }
            }
            catch (OperationCanceledException e)
            {
                Debug.WriteLine("Canceled Downloading of Images {0}", e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ex {0}", e.Message);
            }
            
        }
    }
}
