using PortableApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WhatAreWetlandsPage : ViewHelpers
    {
        ListView wetlandTypesList;

        public WhatAreWetlandsPage()
        {

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            HeaderNavigationOptions navOptions = new HeaderNavigationOptions { titleText = "WETLAND TYPES", backButtonVisible = true, homeButtonVisible = true };
            Grid navigationBar = ConstructNavigationBar(navOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
        
            innerContainer.Children.Add(navigationBar, 0, 0);

            wetlandTypesList = new ListView { BackgroundColor = Color.Transparent };
            var wetlandTypeTemplate = new DataTemplate(typeof(WetlandTypesItemTemplate));
            wetlandTypesList.ItemTemplate = wetlandTypeTemplate;
            wetlandTypesList.ItemsSource = WetlandTypesList();
            wetlandTypesList.ItemSelected += OnItemSelected;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            innerContainer.Children.Add(wetlandTypesList, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        public List<WetlandType> WetlandTypesList()
        {

            var wetlandTypes = new List<WetlandType>();
            wetlandTypes.Add(new WetlandType
            {
                Title = "MARSH",
                Description = "WetlandType-Marsh.html"
            });
            wetlandTypes.Add(new WetlandType
            {
                Title = "WET MEADOW",
                Description = "WetlandType-WetMeadow.html"
            });
            wetlandTypes.Add(new WetlandType
            {
                Title = "MESIC MEADOW",
                Description = "WetlandType-MesicMeadow.html"
            });
            return wetlandTypes;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        { 
            var item = e.SelectedItem as WetlandType;
            if (item != null)
            {
                var detailPage = new WetlandTypesDetailPage(item);
                wetlandTypesList.SelectedItem = null;
                await Navigation.PushModalAsync(detailPage);
            }
        }

    }

    public class WetlandTypesItemTemplate : ViewCell
    {
        public WetlandTypesItemTemplate()
        {
            var title = new Label {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                FontSize = 18
            };
            title.SetBinding(Label.TextProperty, new Binding("Title"));

            Grid grid = new Grid {
                BackgroundColor = Color.FromHex("66000000"),
                Padding = new Thickness(5),
                RowSpacing = 10
            };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });
            grid.Children.Add(title);

            View = grid;
        }
    }

}
