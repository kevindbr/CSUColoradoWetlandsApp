using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using PCLStorage;
using System.Linq;
//using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Threading;

namespace PortableApp
{
    public partial class DownloadWetlandPlantsPage : ViewHelpers
    {
        public EventHandler InitFinishedDownload;
        public EventHandler InitCancelDownload;
        bool updatePlants;
        WetlandSetting datePlantDataUpdatedLocally;
        WetlandSetting datePlantDataUpdatedOnServer;
        List<WetlandSetting> imageFilesToDownload;
        ObservableCollection<WetlandPlant> plants;
        ObservableCollection<WetlandGlossary> terms;
        ProgressBar progressBar = new ProgressBar();
        Label downloadLabel = new Label { Text = "", TextColor = Color.White, HeightRequest = 40, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center };
        Button cancelButton;
        CancellationTokenSource tokenSource;
        CancellationToken token;
        //HttpClient client = new HttpClient();

        protected async override void OnAppearing()
        {
            // Initialize CancellationToken
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            // Initialize progressbar and page
            progressBar.Progress = 0;
            base.OnAppearing();

            // Get all plants from external API call, store them in a collection
            plants = new ObservableCollection<WetlandPlant>(await externalConnection.GetAllPlants());
            terms = new ObservableCollection<WetlandGlossary>(await externalConnection.GetAllTerms());

            // Save plants to the database
            if (updatePlants && !token.IsCancellationRequested)
                await UpdatePlants(token);

            // Save images to the database
         //   if (imageFilesToDownload.Count > 0 && downloadImages && !token.IsCancellationRequested)
         //       await UpdatePlantImages(token);



            if (token.IsCancellationRequested)
                CancelDownload();

            else
                FinishDownload();
        }

        public DownloadWetlandPlantsPage(bool updatePlantsNow, WetlandSetting dateLocalPlantDataUpdated, WetlandSetting datePlantDataUpdated, List<WetlandSetting> imageFilesNeedingDownloaded, bool downloadImagesFromServer)
        {
            updatePlants = updatePlantsNow;
            datePlantDataUpdatedLocally = dateLocalPlantDataUpdated;
            datePlantDataUpdatedOnServer = datePlantDataUpdated;
            imageFilesToDownload = (imageFilesNeedingDownloaded == null) ? new List<WetlandSetting>() : imageFilesNeedingDownloaded;
            downloadImages = downloadImagesFromServer;

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
            cancelButton.Clicked += CancelDownload;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(cancelButton, 0, 3);

            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        private void FinishDownload()
        {
            InitFinishedDownload?.Invoke(this, EventArgs.Empty);
        }

        private void CancelDownload(object sender, EventArgs e)
        {
            tokenSource.Cancel();
            InitCancelDownload?.Invoke(this, EventArgs.Empty);
        }

        private void CancelDownload()
        {
            tokenSource.Cancel();
            InitCancelDownload?.Invoke(this, EventArgs.Empty);
        }

        // Get plants from MobileApi server and save locally
        public async Task UpdatePlants(CancellationToken token)
        {
            try
            {

                downloadLabel.Text = "Connecting ...";

                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //var allImages = App.WetlandPlantImageRepoLocal.GetAllWetlandPlantImages();

                //Clear Local Database
                App.WetlandPlantRepo.ClearWetlandPlants();

                await UpdatePlantImages(token);
                App.WetlandPlantImageRepoLocal = new WetlandPlantImageRepositoryLocal(App.WetlandPlantImageRepo.GetAllWetlandPlantImages());
            }
            catch (OperationCanceledException e)
            {
                downloadLabel.Text = "Download Canceled!";
                Debug.WriteLine("Canceled UpdatePlants {0}", e.Message);
            }            
            catch (Exception e)
            {
                downloadLabel.Text = "Error While Downloading Database!";
                Debug.WriteLine("ex {0}", e.Message);
            }
        }

        // Get plant images from MobileApi server and save locally
        public async Task UpdatePlantImages(CancellationToken token)
        {     // Download needed image files
            if (imageFilesToDownload.Count == 0)
            {
                int plantsSaved = 0;
                foreach (var plant in plants)
                {
                    if (token.IsCancellationRequested) { token.ThrowIfCancellationRequested(); };
                    await App.WetlandPlantRepo.AddOrUpdatePlantAsync(plant);
                    plantsSaved += 1;
                    await progressBar.ProgressTo((double)plantsSaved / (plants.Count + terms.Count), 1, Easing.Linear);
                    Double percent = (double)plantsSaved / (plants.Count + terms.Count);
                    downloadLabel.Text = "Downloading Plant Data..." + Math.Round(percent * 100) + "%";
                }


                //await App.WetlandPlantRepo.AddOrUpdateAllPlantsAsync(plants);
                plantsSaved = plants.Count;

                foreach (var term in terms)
                {
                    if (token.IsCancellationRequested) { token.ThrowIfCancellationRequested(); };
                    await App.WetlandGlossaryRepo.AddOrUpdateTermAsync(term);
                    plantsSaved += 1;
                    await progressBar.ProgressTo((double)plantsSaved / (plants.Count + terms.Count), 1, Easing.Linear);
                    Double percent = (double)plantsSaved / (plants.Count + terms.Count);
                    downloadLabel.Text = "Downloading Plant Data..." + Math.Round(percent * 100) + "%";
                }

                //await App.WetlandGlossaryRepo.AddOrUpdateAllTermsAsync(terms);


                datePlantDataUpdatedLocally.valuetimestamp = datePlantDataUpdatedOnServer.valuetimestamp;
                await App.WetlandSettingsRepo.AddOrUpdateSettingAsync(datePlantDataUpdatedLocally);
                App.WetlandPlantRepoLocal = new WetlandPlantRepositoryLocal(App.WetlandPlantRepo.GetAllWetlandPlants());
                App.WetlandPlantFruitsRepoLocal = new WetlandPlantFruitsRepositoryLocal(App.WetlandPlantFruitsRepo.GetAllWetlandFruits());
                App.WetlandPlantDivisionRepoLocal = new WetlandPlantDivisionRepositoryLocal(App.WetlandPlantDivisionRepo.GetAllDivisions());
                App.WetlandPlantShapeRepoLocal = new WetlandPlantShapeRepositoryLocal(App.WetlandPlantShapeRepo.GetAllShapes());
                App.WetlandPlantLeafArrangementRepoLocal = new WetlandPlantLeafArrangementRepositoryLocal(App.WetlandPlantLeafArrangementRepo.GetAllArrangements());
                App.WetlandPlantSizeRepoLocal = new WetlandPlantSizeRepositoryLocal(App.WetlandPlantSizeRepo.GetAllPlantSizes());
                App.WetlandCountyPlantRepoLocal = new WetlandCountyPlantRepositoryLocal(App.WetlandCountyPlantRepo.GetAllCounties());
                App.WetlandRegionRepoLocal = new WetlandPlantRegionRepositoryLocal(App.WetlandRegionRepo.GetAllWetlandRegions());
            }


            // Download needed image files
            if (imageFilesToDownload.Count > 0)
            {
                try
                {
                    long receivedBytes = 0;
                    long? totalBytes =  imageFilesToDownload.Sum(x => x.valueint);

                    int plantsSaved = 0;
                    foreach (var plant in plants)
                    {
                        if (token.IsCancellationRequested) { token.ThrowIfCancellationRequested(); };
                        await App.WetlandPlantRepo.AddOrUpdatePlantAsync(plant);
                        plantsSaved += 1;
                        Double percent = (((double)plantsSaved * 100000) + (double)receivedBytes)/ (((plants.Count + terms.Count) * 100000) + (double)totalBytes);
                        await progressBar.ProgressTo(percent, 1, Easing.Linear);
                        downloadLabel.Text = "Downloading Plant Data..." + Math.Round(percent * 100) + "%";
                    }


                    //await App.WetlandPlantRepo.AddOrUpdateAllPlantsAsync(plants);
                    plantsSaved = plants.Count;

                    foreach (var term in terms)
                    {
                        if (token.IsCancellationRequested) { token.ThrowIfCancellationRequested(); };
                        await App.WetlandGlossaryRepo.AddOrUpdateTermAsync(term);
                        plantsSaved += 1;
                        Double percent = (((double)plantsSaved * 100000) + (double)receivedBytes) / (((plants.Count + terms.Count) * 100000) + (double)totalBytes);
                        await progressBar.ProgressTo(percent, 1, Easing.Linear);
                        downloadLabel.Text = "Downloading Plant Data..." + Math.Round(percent * 100) + "%";
                    }

                    //await App.WetlandGlossaryRepo.AddOrUpdateAllTermsAsync(terms);


                    datePlantDataUpdatedLocally.valuetimestamp = datePlantDataUpdatedOnServer.valuetimestamp;
                    await App.WetlandSettingsRepo.AddOrUpdateSettingAsync(datePlantDataUpdatedLocally);
                    App.WetlandPlantRepoLocal = new WetlandPlantRepositoryLocal(App.WetlandPlantRepo.GetAllWetlandPlants());
                    App.WetlandPlantFruitsRepoLocal = new WetlandPlantFruitsRepositoryLocal(App.WetlandPlantFruitsRepo.GetAllWetlandFruits());
                    App.WetlandPlantDivisionRepoLocal = new WetlandPlantDivisionRepositoryLocal(App.WetlandPlantDivisionRepo.GetAllDivisions());
                    App.WetlandPlantShapeRepoLocal = new WetlandPlantShapeRepositoryLocal(App.WetlandPlantShapeRepo.GetAllShapes());
                    App.WetlandPlantLeafArrangementRepoLocal = new WetlandPlantLeafArrangementRepositoryLocal(App.WetlandPlantLeafArrangementRepo.GetAllArrangements());
                    App.WetlandPlantSizeRepoLocal = new WetlandPlantSizeRepositoryLocal(App.WetlandPlantSizeRepo.GetAllPlantSizes());
                    App.WetlandCountyPlantRepoLocal = new WetlandCountyPlantRepositoryLocal(App.WetlandCountyPlantRepo.GetAllCounties());
                    App.WetlandRegionRepoLocal = new WetlandPlantRegionRepositoryLocal(App.WetlandRegionRepo.GetAllWetlandRegions());

                    // Set progressBar to 0 and downloadLabel text to "Downloading Images..."
                    downloadLabel.Text = "Downloading Plant Data...";

               
                    // IFolder interface from PCLStorage; create or open imagesZipped folder (in Library/Images)    
                    IFolder rootFolder = FileSystem.Current.LocalStorage;
                    IFolder folder = await rootFolder.CreateFolderAsync("Images", CreationCollisionOption.OpenIfExists);
                    string folderPath = rootFolder.Path;

                    // Get image file setting records from MobileApi to determine which files to download
                    // TODO: Limit this to only the files needed, not just every file
                    totalBytes = imageFilesToDownload.Sum(x => x.valueint);

                    // For each setting, get the corresponding zip file and save it locally
                    foreach (WetlandSetting imageFileToDownload in imageFilesToDownload)
                    {
                        Stream webStream = await externalConnection.GetImageZipFiles(imageFileToDownload.valuetext.Replace(".zip", ""));
                        ZipInputStream zipInputStream = new ZipInputStream(webStream);
                        ZipEntry zipEntry = zipInputStream.GetNextEntry();
                        while (zipEntry != null)
                        {
                            if (token.IsCancellationRequested) { token.ThrowIfCancellationRequested(); };
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
                            Double percentage = (((double)plantsSaved * 100000) + (double)receivedBytes) / (((plants.Count + terms.Count) * 100000) + (double)totalBytes);
                            await progressBar.ProgressTo(percentage, 1, Easing.Linear);
                            zipEntry = zipInputStream.GetNextEntry();
                            
                            if(Math.Round(percentage * 100) < 100)
                            {
                                downloadLabel.Text = "Downloading Plant Data..." + Math.Round(percentage * 100) + "%";
                            }
                            else
                            {
                                downloadLabel.Text = "Downloading Plant Data...100%";
                            }
                            
                        }

                        downloadImages = true;
                        await App.WetlandSettingsRepo.AddSettingAsync(new WetlandSetting { name = "ImagesZipFile", valuebool=true });

                        await App.WetlandSettingsRepo.AddSettingAsync(new WetlandSetting { name = "ImagesZipFile", valuetimestamp = imageFileToDownload.valuetimestamp, valuetext = imageFileToDownload.valuetext });
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
}
