using Xamarin.Forms;

namespace PortableApp
{
    public class ViewHelpers : ContentPage
    {

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

        public Image ConstructHeader()
        {
            Image header = new Image
            {
                Source = ImageSource.FromResource("PortableApp.Resources.Images.wetlands_logo.png"),
                Margin = new Thickness(0, 15, 0, 15)
            };
            return header;
            //    Grid Container = new Grid { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
            //    Container.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            //    Container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //    StackLayout innerLayout = new StackLayout();
            //    Image logo = new Image
            //    {
            //        Source = ImageSource.FromResource("PortableApp.Images.wetlands_logo.png"),
            //        HeightRequest = 20,
            //        WidthRequest = 20,
            //        Margin = new Thickness(0, 15, 0, 15)
            //    };
            //    innerLayout.Children.Add(logo);

            //    Container.Children.Add(innerLayout, 0, 0);
            //    return Container;
        }         

    }
}
