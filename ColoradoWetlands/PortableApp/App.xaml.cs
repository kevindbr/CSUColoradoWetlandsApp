using PortableApp.Models;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using System;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class App : Application
    {
        public static double ScreenHeight;
        public static double ScreenWidth;

        public static WetlandPlantRepository WetlandPlantRepo { get; private set; }
        public static WetlandPlantImageRepository WetlandPlantImageRepo { get; private set; }
        public static WetlandPlantSimilarSpeciesRepository WetlandPlantSimilarSpeciesRepo { get; private set; }
        public static WetlandPlantReferenceRepository WetlandPlantReferenceRepo { get; private set; }
        public static WetlandMapOverlayCoordinateRepository WetlandMapOverlayCoordinateRepo { get; private set; }
        public static WetlandMapOverlayRepository WetlandMapOverlayRepo { get; private set; }
        public static WetlandGlossaryRepository WetlandGlossaryRepo { get; private set; }
        public static WetlandTypeRepository WetlandTypeRepo { get; private set; }
        public static WetlandSettingRepository WetlandSettingsRepo { get; private set; }
        public static WetlandSearchRepository WetlandSearchRepo { get; private set; }
        public static WetlandPlantFruitsRepository WetlandPlantFruitsRepo { get; private set; }

        public static WetlandPlantRepositoryLocal WetlandPlantRepoLocal { get; private set; }
        public static WetlandPlantImageRepositoryLocal WetlandPlantImageRepoLocal { get; private set; }
       // public static WetlandPlantSimilarSpeciesRepositoryLocal WetlandPlantSimilarSpeciesRepoLocal { get; private set; }
        public static WetlandPlantReferenceRepositoryLocal WetlandPlantReferenceRepoLocal { get; private set; }
       // public static WetlandMapOverlayCoordinateRepositoryLocal WetlandMapOverlayCoordinateRepoLocal { get; private set; }
        public static WetlandMapOverlayRepositoryLocal WetlandMapOverlayRepoLocal { get; private set; }
        public static WetlandGlossaryRepositoryLocal WetlandGlossaryRepoLocal { get; private set; }
        public static WetlandTypeRepositoryLocal WetlandTypeRepoLocal { get; private set; }
        public static WetlandSettingRepositoryLocal WetlandSettingsRepoLocal { get; private set; }
        public static WetlandSearchRepositoryLocal WetlandSearchRepoLocal { get; private set; }
        public static WetlandPlantFruitsRepositoryLocal WetlandPlantFruitsRepoLocal { get; private set; }

        /*
         New starting workflow

         1.)Check if we have local plants
            a.) We do
            1.) Check last updated column local mobile DB vs on remote DB (Postgres DB) 
                a) If different 
                1.) Update their data? 
            b.) We don't
            1.) Fetch all 

             */
        public App(ISQLitePlatform sqliteplatform, string dbPath)
        {
            InitializeComponent();

            // Initialize SQLite connection and DBConnection class to hold connection
            SQLiteConnection newConn = new SQLiteConnection(sqliteplatform, dbPath, false);            
            DBConnection dbConn = new DBConnection(newConn);

            SQLiteAsyncConnection newConnAsync = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(sqliteplatform, new SQLiteConnectionString(dbPath, false)));
            DBConnection dbConnAsync = new DBConnection(newConnAsync);

            // Initialize repositories
            WetlandPlantRepo = new WetlandPlantRepository();
            WetlandPlantImageRepo = new WetlandPlantImageRepository();
            WetlandPlantSimilarSpeciesRepo = new WetlandPlantSimilarSpeciesRepository();
            WetlandPlantFruitsRepo = new WetlandPlantFruitsRepository();
            WetlandPlantReferenceRepo = new WetlandPlantReferenceRepository();
            WetlandMapOverlayCoordinateRepo = new WetlandMapOverlayCoordinateRepository();
            WetlandMapOverlayRepo = new WetlandMapOverlayRepository();
            WetlandGlossaryRepo = new WetlandGlossaryRepository();
            WetlandTypeRepo = new WetlandTypeRepository();
            WetlandSettingsRepo = new WetlandSettingRepository();
            WetlandSearchRepo = new WetlandSearchRepository();

            WetlandPlantRepoLocal = new WetlandPlantRepositoryLocal(WetlandPlantRepo.GetAllWetlandPlants());


            // Set MainPage
            this.MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts

            App.WetlandSettingsRepo.AddOrUpdateSetting(new WetlandSetting { name = "Sort Field", valuetext = "Sort", valueint = 0 });
        }
        /*
        protected override void OnSleep()
        {
            // Handle when your app sleeps

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        */
    }
}
