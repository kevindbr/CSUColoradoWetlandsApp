using PortableApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandPlantInfoPage : ViewHelpers
    {

        public WetlandPlantInfoPage(WetlandPlant plant, ObservableCollection<WetlandPlant> plants)
        {

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            NavigationOptions navOptions = new NavigationOptions { titleText = plant.scinamenoauthorstripped, backButtonVisible = true, nextAndPreviousVisible = true, plant = plant, plants = plants };
            Grid navigationBar = ConstructNavigationBar(navOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);
            
            ScrollView contentScrollView = new ScrollView {
                BackgroundColor = Color.FromHex("88000000"),
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(15, 0, 15, 0)
            };

            TransparentWebView browser = ConstructHTMLContent(plant);

            contentScrollView.Content = browser;
            innerContainer.RowDefinitions.Add(new RowDefinition { });
            innerContainer.Children.Add(contentScrollView, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        public TransparentWebView ConstructHTMLContent(WetlandPlant plant)
        {
            var browser = new TransparentWebView();
            var htmlSource = new HtmlWebViewSource();
            string html = "";

            html += "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset = 'utf-8' /><title>Plant Info Page</title></head><body>";
            html += "<style>body { color: white; font-size: 0.8em; } .section_header { font-weight: bold; border-bottom: 1px solid white; margin: 10px 0; }</style>";

            html += "<div class='section_header'>NOMENCLATURE</div>";
            html += "<strong>Scientific Name: </strong>" + plant.scinameauthor + "<br/>";
            html += "<strong>Family: </strong>" + plant.family + "<br/>";
            html += "<strong>Common Name: </strong>" + plant.commonname + "<br/>";
            html += "<strong>Synonyms: </strong>" + plant.synonyms + "<br/>";

            html += "<div class='section_header'>CONSERVATION STATUS</div>";
            html += "<strong>Federal Status: </strong>" + plant.federalstatus + "<br/>";
            html += "<strong>Global Rank: </strong>" + plant.grank + "<br/>";

            html += "</body></html>";

            htmlSource.Html = html;
            browser.Source = htmlSource;
            return browser;


            //contentContainer.Children.Add(InfoPageSectionHeader("NOMENCLATURE"));
            //contentContainer.Children.Add(InfoPageSet("Scientific Name:", plant.scinamenoauthorstripped));
            //contentContainer.Children.Add(InfoPageSet("Family:", plant.family));
            //contentContainer.Children.Add(InfoPageSet("Common Name:", plant.commonname));

            //contentContainer.Children.Add(InfoPageSectionHeader("CONSERVATION STATUS"));
            //contentContainer.Children.Add(InfoPageSet("Federal Status:", plant.federalstatus));
            //contentContainer.Children.Add(InfoPageSet("Global Rank:", plant.grank));

            //return browser;
        }

    }
}
