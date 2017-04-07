using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;

using Xamarin.Forms;

namespace PortableApp
{
    public class HTMLPage : ViewHelpers
    {
        public HTMLPage(string file, string titleText)
        {

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            Grid navigationBar = ConstructNavigationBar(titleText, true, true, false);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);



            // Get file from PCL--in order for HTML files to be automatically pulled from the PCL, they need to be in a Views/HTML folder
            var assembly = typeof(HTMLPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("PortableApp.Views.HTML." + file);
            string text = "";
            using (var reader = new System.IO.StreamReader(stream)) { text = reader.ReadToEnd(); }

            // Generate WebView content
            var browser = new TransparentWebView();
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @text;
            browser.Source = htmlSource;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            innerContainer.Children.Add(browser, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }
    }
}