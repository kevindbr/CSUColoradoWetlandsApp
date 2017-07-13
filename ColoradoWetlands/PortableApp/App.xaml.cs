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
            WetlandPlantReferenceRepo = new WetlandPlantReferenceRepository();
            WetlandMapOverlayCoordinateRepo = new WetlandMapOverlayCoordinateRepository();
            WetlandMapOverlayRepo = new WetlandMapOverlayRepository();
            WetlandGlossaryRepo = new WetlandGlossaryRepository();
            WetlandTypeRepo = new WetlandTypeRepository();
            WetlandSettingsRepo = new WetlandSettingRepository();
            WetlandSearchRepo = new WetlandSearchRepository();

            // Set MainPage
            this.MainPage = new NavigationPage(new MainPage());
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
