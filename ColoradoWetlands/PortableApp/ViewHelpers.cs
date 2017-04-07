using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{
    public class ViewHelpers : ContentPage
    {
        //
        // VIEWS
        //

        // Construct Page Container as an AbsoluteLayout with a background image
        public AbsoluteLayout ConstructPageContainer()
        {
            AbsoluteLayout pageContainer = new AbsoluteLayout { BackgroundColor = Color.Black };
            Image backgroundImage = new Image {
                Source = ImageSource.FromResource("PortableApp.Resources.Images.background.jpg"),
                Aspect = Aspect.AspectFill,
                Opacity = 0.7
            };
            pageContainer.Children.Add(backgroundImage, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            return pageContainer;
        }

        // Construct Navigation Bar
        public Grid ConstructNavigationBar(string titleText, bool backButtonVisible = true, bool menuButtonVisible = true, bool logoVisible = false)
        {
            Grid gridLayout = new Grid { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            // Construct back button and add gesture recognizer
            if (backButtonVisible)
            {
                Image backImage = new Image
                {
                    Source = ImageSource.FromResource("PortableApp.Resources.Icons.back_arrow.png"),
                    HeightRequest = 20,
                    WidthRequest = 20,
                    Margin = new Thickness(0, 15, 0, 15)
                };
                var backGestureRecognizer = new TapGestureRecognizer();
                backGestureRecognizer.Tapped += async (sender, e) =>
                {
                    backImage.Opacity = .5;
                    await Task.Delay(200);
                    backImage.Opacity = 1;
                    await Navigation.PopAsync();
                };
                backImage.GestureRecognizers.Add(backGestureRecognizer);
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                gridLayout.Children.Add(backImage, 0, 0);
            }
            else
            {
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0) });
            }

            // Construct title content section
            StackLayout titleContent = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            Image logo = new Image { Source = ImageSource.FromResource("PortableApp.Resources.Icons.Co_Logo_40.png"), IsVisible = logoVisible };
            Label title = new Label { Text = titleText, FontFamily = Device.OnPlatform("Montserrat-Medium", "Montserrat-Medium.ttf#Montserrat-Medium", null), TextColor = Color.White, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
            titleContent.Children.Add(logo);
            titleContent.Children.Add(title);
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            gridLayout.Children.Add(titleContent, 1, 0);

            // Construct home button and add gesture recognizer
            if (menuButtonVisible)
            {
                Image menuImage = new Image
                {
                    Source = ImageSource.FromResource("PortableApp.Resources.Icons.menu_icon.png"),
                    HeightRequest = 20,
                    WidthRequest = 20,
                    Margin = new Thickness(0, 15, 0, 15)
                };
                var menuImageGestureRecognizer = new TapGestureRecognizer();
                menuImageGestureRecognizer.Tapped += async (sender, e) =>
                {
                    menuImage.Opacity = .5;
                    await Task.Delay(200);
                    menuImage.Opacity = 1;
                    await Navigation.PopToRootAsync();
                    //await Navigation.PushAsync(new IntroPage());
                };
                menuImage.GestureRecognizers.Add(menuImageGestureRecognizer);
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                gridLayout.Children.Add(menuImage, 2, 0);
            }
            else
            {
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0) });
            }
            
            return gridLayout;
        }

        protected async void ChangeButtonColor(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.BackgroundColor = Color.FromHex("BB105456");
            await Task.Delay(100);
            button.BackgroundColor = Color.FromHex("66000000");
        }

        //
        // NAVIGATION
        //

        public async void ToIntroduction(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("Introduction.html", "INTRODUCTION"));
        }

        public async void ToWetlandPlants(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new WetlandPlantsPage());
        }

        public async void ToWetlandMaps(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            //await Navigation.PushAsync(new WetlandMapsPage());
        }

        public async void ToWetlandTypes(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new WetlandTypesPage());
        }

        public async void ToAcknowledgements(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("Acknowledgements.html", "ACKNOWLEDGEMENTS"));
        }

    }
}
