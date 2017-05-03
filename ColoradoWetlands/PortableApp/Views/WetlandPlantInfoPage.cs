using PortableApp.Models;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandPlantInfoPage : ViewHelpers
    {

        public WetlandPlantInfoPage(WetlandPlant plant)
        {

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            Grid navigationBar = ConstructNavigationBar(plant.scinamenoauthor, true, true, false);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);
            
            ScrollView contentScrollView = new ScrollView {
                BackgroundColor = Color.FromHex("88000000"),
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(15, 0, 15, 0)
            };
            StackLayout contentContainer = new StackLayout();

            contentContainer.Children.Add(InfoPageSectionHeader("NOMENCLATURE"));
            contentContainer.Children.Add(InfoPageSet("Scientific Name:", plant.scinamenoauthor));
            contentContainer.Children.Add(InfoPageSet("Family:", plant.family));
            contentContainer.Children.Add(InfoPageSet("Common Name:", plant.commonname));

            contentContainer.Children.Add(InfoPageSectionHeader("CONSERVATION STATUS"));
            contentContainer.Children.Add(InfoPageSet("Federal Status:", plant.federalstatus));
            contentContainer.Children.Add(InfoPageSet("Global Rank:", plant.grank));

            contentScrollView.Content = contentContainer;
            innerContainer.RowDefinitions.Add(new RowDefinition { });
            innerContainer.Children.Add(contentScrollView, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

    }
}
