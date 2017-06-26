using PortableApp.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandPlantSimilarPage : ViewHelpers
    {
        ListView wetlandPlantsList;
        ObservableCollection<WetlandPlantSimilarSpecies> similarSpecies;

        public WetlandPlantSimilarPage(WetlandPlant plant, ObservableCollection<WetlandPlant> plants)
        {

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            HeaderNavigationOptions navOptions = new HeaderNavigationOptions { titleText = plant.scinamenoauthorstripped, backButtonVisible = true, nextAndPreviousVisible = true, plant = plant, plants = plants };
            Grid navigationBar = ConstructNavigationBar(navOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);

            ScrollView contentScrollView = new ScrollView
            {
                BackgroundColor = Color.FromHex("88000000"),
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(0, 0, 0, 0)
            };

            Grid contentLayout = new Grid();
            contentLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            if (plant.similarsp != null & plant.similarsp != "")
            {
                TransparentWebView browser = ConstructHTMLContent(plant);
                contentLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                contentLayout.Children.Add(browser, 0, 1);
            }

            similarSpecies = new ObservableCollection<WetlandPlantSimilarSpecies>(plant.SimilarSpecies);
            if (similarSpecies.Count > 0)
            {
                wetlandPlantsList = new ListView { BackgroundColor = Color.Transparent, RowHeight = 100, SeparatorVisibility = SeparatorVisibility.None };
                wetlandPlantsList.ItemTemplate = new DataTemplate(typeof(WetlandPlantsSimilarSpeciesTemplate));
                wetlandPlantsList.ItemsSource = similarSpecies;
                wetlandPlantsList.ItemSelected += OnItemSelected;
                contentLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                int row = (plant.similarsp == null || plant.similarsp == "") ? 1 : 2;
                contentLayout.Children.Add(wetlandPlantsList, 0, row);
            }            

            contentScrollView.Content = contentLayout;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
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

            html += "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset = 'utf-8' /><title>Similar Plants</title></head><body>";
            html += "<style>body { color: white; font-size: 0.9em; } .section_header { font-weight: bold; border-bottom: 1px solid white; margin: 10px 0; }</style>";

            html += "<div class='section_header'>GENERAL COMMENTS</div>" + plant.similarsp;

            html += "</body></html>";

            htmlSource.Html = html;
            browser.Source = htmlSource;
            return browser;
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (wetlandPlantsList.SelectedItem != null)
            {
                await App.WetlandSettingsRepo.AddOrUpdateSettingAsync(new WetlandSetting { name = "SelectedTab", valueint = 0 });
                WetlandPlantSimilarSpecies selectedItem = e.SelectedItem as WetlandPlantSimilarSpecies;
                WetlandPlant selectedPlant = App.WetlandPlantRepo.GetWetlandPlantByAltId(selectedItem.id);
                var detailPage = new PortableApp.Views.WetlandPlantDetailPage(selectedPlant);
                detailPage.BindingContext = selectedItem;
                wetlandPlantsList.SelectedItem = null;
                await Navigation.PushAsync(detailPage);
            }
        }
    }

    public class WetlandPlantsSimilarSpeciesTemplate : ViewCell
    {
        public WetlandPlantsSimilarSpeciesTemplate()
        {
            // Construct grid, the cell container
            Grid cell = new Grid
            {
                BackgroundColor = Color.FromHex("00000000"),
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(0, 0, 0, 10)
            };
            cell.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });
            cell.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.7, GridUnitType.Star) });
            cell.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });

            // Add image
            var image = new Image { Aspect = Aspect.AspectFill, Margin = new Thickness(0, 0, 0, 20) };
            image.SetBinding(Image.SourceProperty, new Binding("ThumbnailPath"));
            cell.Children.Add(image, 0, 0);

            // Add text section
            StackLayout textSection = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 2 };

            Label scientificName = new Label { TextColor = Color.White, FontSize = 12, FontAttributes = FontAttributes.Bold | FontAttributes.Italic };
            scientificName.SetBinding(Label.TextProperty, new Binding("similarspscinameauthorstripped"));
            textSection.Children.Add(scientificName);

            Label reason = new Label { TextColor = Color.White, FontSize = 12 };
            reason.SetBinding(Label.TextProperty, new Binding("reasonstripped"));
            textSection.Children.Add(reason);
            
            cell.Children.Add(textSection, 1, 0);
            View = cell;
        }

    }
}
