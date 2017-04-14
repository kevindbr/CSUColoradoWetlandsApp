using PortableApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandPlantsPage : ViewHelpers
    {
        ListView wetlandPlantsList;

        public WetlandPlantsPage()
        {
            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            Grid navigationBar = ConstructNavigationBar("WETLAND PLANTS", true, true, false);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            innerContainer.Children.Add(navigationBar, 0, 0);

            wetlandPlantsList = new ListView { BackgroundColor = Color.Transparent, RowHeight = 100 };
            var wetlandTypeTemplate = new DataTemplate(typeof(WetlandPlantsItemTemplate));
            wetlandPlantsList.ItemTemplate = wetlandTypeTemplate;
            wetlandPlantsList.ItemsSource = WetlandPlantsList();
            //wetlandPlantsList.ItemSelected += OnItemSelected;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            innerContainer.Children.Add(wetlandPlantsList, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        public List<WetlandPlant> WetlandPlantsList()
        {

            var wetlandPlants = new List<WetlandPlant>();
            wetlandPlants.Add(new WetlandPlant
            {
                CommonName = "ACER NEGUNDO",
                Description = "Boxelder",
                Description2 = "Aceraceae",
                Description3 = "Woody Plants",
                FileName = "ACNE2_3.jpg"
            });
            wetlandPlants.Add(new WetlandPlant
            {
                CommonName = "ACONITUM COLUMBIANUM",
                Description = "Columbian Monkshood",
                Description2 = "Ranunculaceae (Helleboraceae)",
                Description3 = "Dicot Herbs",
                FileName = "ACCO4_1.jpg"
            });
            wetlandPlants.Add(new WetlandPlant
            {
                CommonName = "ACORUS CALAMUS",
                Description = "Calamus",
                Description2= "Acoraceae",
                Description3 = "Monocot Herbs",
                FileName = "ACCA4_2.jpg"
            });
            wetlandPlants.Add(new WetlandPlant
            {
                CommonName = "ADIANTUM CAPILLUS-VENERIS",
                Description = "Common Maidenhair",
                Description2 = "Pteridaceae (Adiantaceae)",
                Description3 = "Ferns and Ferns Allies",
                FileName = "ADCA_1.jpg"
            });
            wetlandPlants.Add(new WetlandPlant
            {
                CommonName = "AGALINIS TENUIFOLIA",
                Description = "Slenderleaf False Foxglove",
                Description2 = "Scrophulariaceae (Orobanchaceae)",
                Description3 = "Dicot Herbs",
                FileName = "AGTE3_1.jpg"
            });
            wetlandPlants.Add(new WetlandPlant
            {
                CommonName = "AGROPYRON DESERTORUM",
                Description = "Desert Wheatgrass",
                Description2 = "Poaceae",
                Description3 = "Grasses",
                FileName = "AGDE2_icon.jpg"
            });
            return wetlandPlants;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (wetlandPlantsList.SelectedItem != null)
            {
                var selectedItem = e.SelectedItem as WetlandPlant;
                var detailPage = new PortableApp.Views.WetlandPlantDetailPage(selectedItem.Id);
                detailPage.BindingContext = selectedItem;
                wetlandPlantsList.SelectedItem = null;
                await Navigation.PushAsync(detailPage);
            }
        }
    }

    public class WetlandPlantsItemTemplate : ViewCell
    {
        public WetlandPlantsItemTemplate()
        {
            // Construct grid, the cell container
            Grid cell = new Grid
            {
                BackgroundColor = Color.FromHex("66000000"),
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(0, 0, 0, 10)
            };
            cell.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });
            cell.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.7, GridUnitType.Star) });
            cell.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });

            // Add image
            var image = new Image { Aspect = Aspect.AspectFill, Margin = new Thickness(0, 0, 0, 20) };
            image.SetBinding(Image.SourceProperty, new Binding("Thumbnail"));
            cell.Children.Add(image, 0, 0);

            // Add text section
            StackLayout textSection = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 2 };

            var commonName = new Label
            {
                TextColor = Color.White,
                FontSize = 12,
                FontAttributes = FontAttributes.Bold
            };
            commonName.SetBinding(Label.TextProperty, new Binding("CommonName"));
            textSection.Children.Add(commonName);

            var divider = new BoxView { HeightRequest = 1, WidthRequest = 500, BackgroundColor = Color.White };
            textSection.Children.Add(divider);

            var description = new Label
            {
                TextColor = Color.White,
                FontSize = 12
            };
            description.SetBinding(Label.TextProperty, new Binding("Description"));
            textSection.Children.Add(description);

            var description2 = new Label
            {
                TextColor = Color.White,
                FontSize = 12
            };
            description2.SetBinding(Label.TextProperty, new Binding("Description2"));
            textSection.Children.Add(description2);

            var description3 = new Label
            {
                TextColor = Color.White,
                FontSize = 12
            };
            description3.SetBinding(Label.TextProperty, new Binding("Description3"));
            textSection.Children.Add(description3);

            cell.Children.Add(textSection, 1, 0);
            View = cell;
        }
    }
}
