﻿using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using PCLStorage;
using System.Linq;
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
        bool resyncPlants;
        bool clearDatabase;
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
        IFolder rootFolder = FileSystem.Current.LocalStorage;
        //HttpClient client = new HttpClient();

        bool finished = false;

        protected async override void OnAppearing()
        {
            downloadLabel.Text = "Connecting ...";
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
            {
                /*while (finished == false)
                {
                   
                }*/
                FinishDownload();
            }
        }

        public DownloadWetlandPlantsPage(bool updatePlantsNow, WetlandSetting dateLocalPlantDataUpdated, WetlandSetting datePlantDataUpdated, List<WetlandSetting> imageFilesNeedingDownloaded, bool downloadImagesFromServer, bool resyncplants, bool clearDatabase)
        {
            updatePlants = updatePlantsNow;
            this.resyncPlants = resyncplants;
            this.clearDatabase = clearDatabase;
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
            progressBar.IsVisible = false;
            downloadLabel.Text = "Canceling Download, One Moment...";

            tokenSource.Cancel();
            //InitCancelDownload?.Invoke(this, EventArgs.Empty);
        }

        private void CancelDownload()
        {
            while (true)
            {
                try
                {
                    ClearRepositories();
                    ClearLocalRepositories();
                    break;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("ex {0}", e.Message);
                }
            }
            InitCancelDownload?.Invoke(this, EventArgs.Empty);
        }

       

        // Get plants from MobileApi server and save locally
        public async Task UpdatePlants(CancellationToken token)
        {
            try
            {
                if (resyncPlants)
                {
                    ClearLocalRepositories();
                }
                ClearRepositories();

                try
                {
                    IFolder folder = await rootFolder.GetFolderAsync("Images");
                    await folder.DeleteAsync();
                }catch (Exception e) { }
             

                await UpdatePlantImages(token);
                //App.WetlandPlantImageRepoLocal = new WetlandPlantImageRepositoryLocal(App.WetlandPlantImageRepo.GetAllWetlandPlantImages());
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
        {   
            // Download needed image files
            if (imageFilesToDownload.Count > 0)
            {
                try
                {
                    long receivedBytes = 0;
                    long? totalBytes =  imageFilesToDownload.Sum(x => x.valueint);

                    await progressBar.ProgressTo(0, 1, Easing.Linear);
                    downloadLabel.Text = "Beginning Download...";

                    //Downlod Plant Data
                    //Task.Run(() => { UpdatePlantConcurrently(token); });
                    UpdatePlantConcurrently(token);                


                    // IFolder interface from PCLStorage; create or open imagesZipped folder (in Library/Images)    

                    IFolder folder = await rootFolder.CreateFolderAsync("Images", CreationCollisionOption.OpenIfExists);                   
                    string folderPath = rootFolder.Path;

                    // Get image file setting records from MobileApi to determine which files to download
                    // TODO: Limit this to only the files needed, not just every file
                    totalBytes = imageFilesToDownload.Sum(x => x.valueint);

                    // For each setting, get the corresponding zip file and save it locally
                    foreach (WetlandSetting imageFileToDownload in imageFilesToDownload)
                    {
                        if (token.IsCancellationRequested) { break; };
                        Stream webStream = await externalConnection.GetImageZipFiles(imageFileToDownload.valuetext.Replace(".zip", ""));
                        ZipInputStream zipInputStream = new ZipInputStream(webStream);
                        ZipEntry zipEntry = zipInputStream.GetNextEntry();
                        while (zipEntry != null)
                        {
                            if (token.IsCancellationRequested)
                            {
                                break;
                            };
                            //token.ThrowIfCancellationRequested();
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
                            //Double percentage = (((double)plantsSaved * 100000) + (double)receivedBytes) / (((plants.Count + terms.Count) * 100000) + (double)totalBytes);
                            Double percentage = ((double)receivedBytes /  (double)totalBytes);
                            await progressBar.ProgressTo(percentage, 1, Easing.Linear);
                            zipEntry = zipInputStream.GetNextEntry();
                            
                            if(Math.Round(percentage * 100) < 100)
                            {
                                downloadLabel.Text = "Downloading Plant Data..." + Math.Round(percentage * 100) + "%";
                            }
                            else
                            {
                                downloadLabel.Text = "Finishing Download...";
                            }
                            
                        }

                        if (!token.IsCancellationRequested)
                        {
                            downloadImages = true;
                            await App.WetlandSettingsRepo.AddSettingAsync(new WetlandSetting { name = "ImagesZipFile", valuebool = true });
                            await App.WetlandSettingsRepo.AddSettingAsync(new WetlandSetting { name = "ImagesZipFile", valuetimestamp = imageFileToDownload.valuetimestamp, valuetext = imageFileToDownload.valuetext });
                            App.WetlandPlantImageRepoLocal = new WetlandPlantImageRepositoryLocal(App.WetlandPlantImageRepo.GetAllWetlandPlantImages());
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

        public async void UpdatePlantConcurrently(CancellationToken token)
        {
            try
            {
                if (!token.IsCancellationRequested)
                {
                   await App.WetlandPlantRepo.AddOrUpdateAllPlantsAsync(plants);
                }
                if (!token.IsCancellationRequested)
                {
                    await App.WetlandGlossaryRepo.AddOrUpdateAllTermsAsync(terms);
                }

                if (!token.IsCancellationRequested)
                {
                    await App.WetlandSettingsRepo.AddOrUpdateSettingAsync(datePlantDataUpdatedLocally);
                }
                if (!token.IsCancellationRequested)
                {
                    datePlantDataUpdatedLocally.valuetimestamp = datePlantDataUpdatedOnServer.valuetimestamp;
                    App.WetlandPlantRepoLocal = new WetlandPlantRepositoryLocal(App.WetlandPlantRepo.GetAllWetlandPlants());
                    App.WetlandPlantFruitsRepoLocal = new WetlandPlantFruitsRepositoryLocal(App.WetlandPlantFruitsRepo.GetAllWetlandFruits());
                    App.WetlandPlantDivisionRepoLocal = new WetlandPlantDivisionRepositoryLocal(App.WetlandPlantDivisionRepo.GetAllDivisions());
                    App.WetlandPlantShapeRepoLocal = new WetlandPlantShapeRepositoryLocal(App.WetlandPlantShapeRepo.GetAllShapes());
                    App.WetlandPlantLeafArrangementRepoLocal = new WetlandPlantLeafArrangementRepositoryLocal(App.WetlandPlantLeafArrangementRepo.GetAllArrangements());
                    App.WetlandPlantSizeRepoLocal = new WetlandPlantSizeRepositoryLocal(App.WetlandPlantSizeRepo.GetAllPlantSizes());
                    App.WetlandCountyPlantRepoLocal = new WetlandCountyPlantRepositoryLocal(App.WetlandCountyPlantRepo.GetAllCounties());
                    App.WetlandRegionRepoLocal = new WetlandPlantRegionRepositoryLocal(App.WetlandRegionRepo.GetAllWetlandRegions());
                    App.WetlandGlossaryRepoLocal = new WetlandGlossaryRepositoryLocal(App.WetlandGlossaryRepo.GetAllWetlandTerms());
                }

            }           
            catch (Exception e) { Debug.WriteLine("ex {0}", e.Message); };

            finished = true;
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
            //SimilarSpeciesLocal
        }

    }
}
