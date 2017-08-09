using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandPlantEcologyPage : ViewHelpers
    {
        WetlandPlant plant;
        ObservableCollection<WetlandPlant> plants;
        TransparentWebView browser;

        protected override async void OnAppearing()
        {
            Content = null;

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            HeaderNavigationOptions navOptions = new HeaderNavigationOptions { titleText = plant.scinamenoauthorstripped, backButtonVisible = true, favoritesIconVisible = true, nextAndPreviousVisible = true, plant = plant, plants = plants };
            Grid navigationBar = ConstructNavigationBar(navOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);

            ScrollView contentScrollView = new ScrollView
            {
                BackgroundColor = Color.FromHex("88000000"),
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(0, 0, 0, 0)
            };

            TransparentWebView browser = ConstructHTMLContent(plant);
            browser.Navigating += ToWetlandType;

            contentScrollView.Content = browser;
            innerContainer.RowDefinitions.Add(new RowDefinition { });
            innerContainer.Children.Add(contentScrollView, 0, 1);

            //var wetlandTypes = ConstructWetlandTypes(plant.ecologicalsystems);
            //innerContainer.RowDefinitions.Add(new RowDefinition { });
            //innerContainer.Children.Add(wetlandTypes, 0, 2);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;

            base.OnAppearing();
        }

        public WetlandPlantEcologyPage(WetlandPlant wetlandPlant, ObservableCollection<WetlandPlant> wetlandPlants)
        {
            plant = wetlandPlant;
            plants = wetlandPlants;

        }
        
        public TransparentWebView ConstructHTMLContent(WetlandPlant plant)
        {
            browser = new TransparentWebView();
            var htmlSource = new HtmlWebViewSource();
            string html = "";

            html += "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset = 'utf-8' /><title>Plant Info Page</title></head><body>";
            html += "<style>body, a { color: white; font-size: 0.9em; } .section_header { font-weight: bold; border-bottom: 1px solid white; margin: 10px 0; } .embedded_table { width: 100%; margin-left: 10px; } .iconImg { height: 40px; }</style>";

            html += "<div class='section_header'>HABITAT & ECOLOGY</div>" + plant.habitat;

            html += "<div class='section_header'>COMMENTS</div>" + plant.comments;

            html += "<div class='section_header'>WETLAND TYPES</div>" + ReconstructWetlandTypes(plant.ecologicalsystems);

            html += "<div class='section_header'>ANIMAL USE</div>" + plant.animaluse.Replace("resources/images/animals/", "");

            html += "</body></html>";

            htmlSource.Html = html;
            browser.Source = htmlSource;
            
            return browser;
        }

        // Prepare wetland type for navigation on click
        private async void ToWetlandType(object sender, WebNavigatingEventArgs e)
        {
            string[] urlArray = e.Url.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            string title = urlArray[urlArray.Length - 1];
            if (!title.Contains("app"))
            {
                WetlandType wetlandType = new WetlandType { Title = title, Description = "WetlandType-" + title.Replace(" ", "").Replace("%20", "") + ".html" };
                var detailPage = new WetlandTypesDetailPage(wetlandType);
                await Navigation.PushModalAsync(detailPage);
            }            
        }
        
        // Reconstruct wetland types ('ecologicalsystems' field) so as to contain an internal link
        private string ReconstructWetlandTypes(string wetlandTypes)
        {
            List<string> allWetlandsArray = new List<string> { "Marsh", "Wet Meadow", "Mesic Meadow", "Fen", "Playa", "Subalpine Riparian Woodland", "Subalpine Riparian Shrubland", "Foothills Riparian", "Plains Riparian", "Plains Floodplain", "Greasewood Flats", "Hanging Garden" };
            foreach (string item in allWetlandsArray)
            {
                if (wetlandTypes.Contains(item))
                {
                    string replacementHTML = "<a href='" + item + "'>" + item + "</a>";
                    wetlandTypes = wetlandTypes.Replace(item, replacementHTML);
                }
            }
            return wetlandTypes;
        }
    }
}
