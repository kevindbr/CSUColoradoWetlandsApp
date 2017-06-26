using PortableApp.Models;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandMapsPage : ViewHelpers
    {
        
        public WetlandMapsPage()
        {

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            HeaderNavigationOptions navOptions = new HeaderNavigationOptions { titleText = "WETLAND MAPS", backButtonVisible = true, homeButtonVisible = true };
            Grid navigationBar = ConstructNavigationBar(navOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);

            // Add map to layout
            Label map = new Label { Text = "MAP GOES HERE IN PLACE OF THIS LABEL" };
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            innerContainer.Children.Add(map, 0, 1);

            // Add FooterBar
            FooterNavigationOptions footerOptions = new FooterNavigationOptions { mapsFooter = true };
            Grid footerBar = ConstructFooterBar(footerOptions);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });
            innerContainer.Children.Add(footerBar, 0, 2);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }        

    }
}
