using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;

using Xamarin.Forms;

namespace PortableApp
{
    public class HTMLPage : ContentPage
    {
        public HTMLPage(string file)
        {
            // Get file from PCL--in order for HTML files to be automatically pulled from the PCL, they need to be in a Views/HTML folder
            var assembly = typeof(HTMLPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("PortableApp.Views.HTML." + file);
            string text = "";
            using (var reader = new System.IO.StreamReader(stream)) { text = reader.ReadToEnd(); }

            // Generate WebView content
            var browser = new WebView();
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @text;
            browser.Source = htmlSource;
            Content = browser;
        }
    }
}