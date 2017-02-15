using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PortableApp
{
    public class App : Application
    {
        public static WetlandPlantRepository WetlandPlantRepo { get; private set; }
        public static WetlandTypeRepository WetlandTypeRepo { get; private set; }

        public App(string dbPath)
        {
            WetlandPlantRepo = new WetlandPlantRepository(dbPath);
            WetlandTypeRepo = new WetlandTypeRepository(dbPath);
            this.MainPage = new NavigationPage (new MainPage(dbPath));
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
