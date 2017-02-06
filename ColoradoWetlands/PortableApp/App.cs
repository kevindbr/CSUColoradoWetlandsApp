using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PortableApp
{
    public class App : Application
    {
        //public static PlantRepository PlantRepo { get; private set; }

        public App(string dbPath)
        {
            // The root page of your application
            //var content = new ContentPage
            //{
            //    Title = "Colorado Wetlands",
            //    Content = new StackLayout
            //    {
            //        VerticalOptions = LayoutOptions.Center,
            //        Children = {
            //            new Label {
            //                HorizontalTextAlignment = TextAlignment.Center,
            //                Text = "Colorado Wetlands App"
            //            }
            //        }
            //    }
            //};
            //PlantRepo = new PlantRepository(dbPath);

            this.MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
