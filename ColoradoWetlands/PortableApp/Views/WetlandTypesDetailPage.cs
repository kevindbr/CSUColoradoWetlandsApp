using PortableApp.Models;
using System;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WetlandTypesDetailPage : ViewHelpers
    {
        public WetlandTypesDetailPage(WetlandType selectedItem)
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

            // Add description of Wetland Type
            var wetlandTypeDescription = HTMLProcessor(selectedItem.Description);
            //Label wetlandTypeDescription = new Label { Text = selectedItem.Description, TextColor = Color.White, FontSize = 18 };
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            innerContainer.Children.Add(wetlandTypeDescription, 0, 0);

            // Add dismiss button
            Button dismissButton = new Button
            {
                Style = Application.Current.Resources["outlineButton"] as Style,
                Text = "CLOSE",
                BorderRadius = Device.OnPlatform(0, 1, 0)
            };
            dismissButton.Clicked += OnDismissButtonClicked;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(dismissButton, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();   
        }
        
    }
}
