using PortableApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandPlantsPage : ViewHelpers
    {
        ListView wetlandPlantsList;
        ObservableCollection<WetlandPlant> plants;

        protected override async void OnAppearing()
        {
            // Get all Pumas from external API call, store them in a collection
            plants = new ObservableCollection<WetlandPlant>(await externalConnection.GetAll());
            wetlandPlantsList.ItemsSource = plants;
            base.OnAppearing();
        }

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
            wetlandPlantsList.ItemSelected += OnItemSelected;
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
                scinameauthor = "ACER NEGUNDO",
                commonname = "Boxelder",
                family = "Aceraceae",
                FileName = "ACNE2_3.jpg"
            });
            wetlandPlants.Add(new WetlandPlant
            {
                scinameauthor = "ACONITUM COLUMBIANUM",
                commonname = "Columbian Monkshood",
                family = "Ranunculaceae (Helleboraceae)",
                FileName = "ACCO4_1.jpg"
            });
            wetlandPlants.Add(new WetlandPlant
            {
                scinameauthor = "ACORUS CALAMUS",
                commonname = "Calamus",
                family = "Acoraceae",
                FileName = "ACCA4_2.jpg"
            });
            wetlandPlants.Add(new WetlandPlant
            {
                scinameauthor = "ADIANTUM CAPILLUS-VENERIS",
                commonname = "Common Maidenhair",
                family = "Pteridaceae (Adiantaceae)",
                FileName = "ADCA_1.jpg"
            });
            wetlandPlants.Add(new WetlandPlant
            {
                scinameauthor = "AGALINIS TENUIFOLIA",
                commonname = "Slenderleaf False Foxglove",
                family = "Scrophulariaceae (Orobanchaceae)",
                FileName = "AGTE3_1.jpg"
            });
            return wetlandPlants;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (wetlandPlantsList.SelectedItem != null)
            {
                var selectedItem = e.SelectedItem as WetlandPlant;
                var detailPage = new PortableApp.Views.WetlandPlantDetailPage(selectedItem);
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
                BackgroundColor = Color.FromHex("88000000"),
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
                FontAttributes = FontAttributes.Bold | FontAttributes.Italic
            };
            commonName.SetBinding(Label.TextProperty, new Binding("scinamenoauthor"));
            textSection.Children.Add(commonName);

            var divider = new BoxView { HeightRequest = 1, WidthRequest = 500, BackgroundColor = Color.White };
            textSection.Children.Add(divider);

            var description = new Label
            {
                TextColor = Color.White,
                FontSize = 12
            };
            description.SetBinding(Label.TextProperty, new Binding("commonname"));
            textSection.Children.Add(description);

            var description2 = new Label
            {
                TextColor = Color.White,
                FontSize = 12
            };
            description2.SetBinding(Label.TextProperty, new Binding("family"));
            textSection.Children.Add(description2);

            cell.Children.Add(textSection, 1, 0);
            View = cell;
        }
    }
}
