using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using PCLStorage;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

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
            //await UpdatePlantImages();

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

        // Iterate through plants and update progressbar
        public async Task UpdatePlants()
        {
            try
            {
                downloadLabel.Text = "Downloading Plants...";
                int plantsSaved = 0;
                foreach (var plant in plants)
                {
                    await App.WetlandPlantRepo.AddPlantAsync(plant);
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

        // Iterate through plants and update progressbar
        public async Task UpdatePlantImages()
        {
            string imagesZipFileLocation = PortableApp.ExternalDBConnection.localUrl + "/image_zip_files/";
            long receivedBytes = 0;
            long? totalBytes = 0;

            await progressBar.ProgressTo(0, 0, Easing.Linear);

            try
            {
                downloadLabel.Text = "Downloading Images...";

                IEnumerable<WetlandSetting> imageFileSettingsOnServer = await externalConnection.GetImageZipFileSettings();
                totalBytes = imageFileSettingsOnServer.Sum(x => x.valueint);

                foreach (WetlandSetting imageFile in imageFileSettingsOnServer)
                {
                    using (var stream = await client.GetStreamAsync(imagesZipFileLocation + imageFile.valuetext.Replace(".zip", "")))
                    {
                        byte[] buffer = new byte[4096];

                        for (;;)
                        {
                            long bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                            {
                                await Task.Yield();
                                break;
                            }

                            receivedBytes += bytesRead;
                            await progressBar.ProgressTo((double)receivedBytes / (double)totalBytes, 0, Easing.Linear);
                        }

                        // IFolder interface comes from PCLStorage    
                        IFolder rootFolder = FileSystem.Current.LocalStorage;
                        IFolder folder = await rootFolder.CreateFolderAsync("zipFolder", CreationCollisionOption.OpenIfExists);
                        IFile file = await folder.CreateFileAsync("images.zip", CreationCollisionOption.OpenIfExists);

                        using (Stream newStream = await file.OpenAsync(FileAccess.ReadAndWrite))
                        {
                            await newStream.WriteAsync(buffer, 0, buffer.Length);
                            using (var zf = new ZipFile(stream))
                            {
                                foreach (ZipEntry zipEntry in zf)
                                {
                                    // Get Entry Stream.
                                    Stream zipEntryStream = zf.GetInputStream(zipEntry);

                                    // Create the file in filesystem and copy entry stream to it.
                                    IFile zipEntryFile = await rootFolder.CreateFileAsync(zipEntry.Name, CreationCollisionOption.FailIfExists);
                                    using (Stream outPutFileStream = await zipEntryFile.OpenAsync(FileAccess.ReadAndWrite))
                                    {
                                        await zipEntryStream.CopyToAsync(outPutFileStream);
                                    }
                                }
                            }
                        }

                    }
                    
                }
                
            }
            catch (OperationCanceledException e)
            {
                Debug.WriteLine("Canceled Updating of Images {0}", e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ex {0}", e.Message);
            }
        }
    }
}
