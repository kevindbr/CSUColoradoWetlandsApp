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
        public static WetlandSettingRepository WetlandSettingsRepo { get; private set; }
        public static WetlandSearchRepository WetlandSearchRepo { get; private set; }
        public static WetlandPlantFruitsRepository WetlandPlantFruitsRepo { get; private set; }
        public static WetlandPlantDivisionRepository WetlandPlantDivisionRepo { get; private set; }
        public static WetlandPlantShapeRepository WetlandPlantShapeRepo { get; private set; }
        public static WetlandPlantLeafArrangementRepository WetlandPlantLeafArrangementRepo { get; private set; }
        public static WetlandPlantSizeRepository WetlandPlantSizeRepo { get; private set; }
        public static WetlandCountyPlantRepository WetlandCountyPlantRepo { get; private set; }
        public static WetlandPlantRegionRepository WetlandRegionRepo { get; private set; }

        public static WetlandPlantRepositoryLocal WetlandPlantRepoLocal { get;  set; }
        public static WetlandPlantImageRepositoryLocal WetlandPlantImageRepoLocal { get;  set; }
        //public static WetlandPlantSimilarSpeciesRepositoryLocal WetlandPlantSimilarSpeciesRepoLocal { get; private set; }
        public static WetlandPlantReferenceRepositoryLocal WetlandPlantReferenceRepoLocal { get;  set; }
        //public static WetlandMapOverlayCoordinateRepositoryLocal WetlandMapOverlayCoordinateRepoLocal { get; private set; }
        public static WetlandMapOverlayRepositoryLocal WetlandMapOverlayRepoLocal { get;  set; }
        public static WetlandGlossaryRepositoryLocal WetlandGlossaryRepoLocal { get;  set; }
        public static WetlandSettingRepositoryLocal WetlandSettingsRepoLocal { get;  set; }
        public static WetlandSearchRepositoryLocal WetlandSearchRepoLocal { get;  set; }
        public static WetlandPlantFruitsRepositoryLocal WetlandPlantFruitsRepoLocal { get; set; }
        public static WetlandPlantDivisionRepositoryLocal WetlandPlantDivisionRepoLocal { get; set; }
        public static WetlandPlantShapeRepositoryLocal WetlandPlantShapeRepoLocal { get; set; }
        public static WetlandPlantLeafArrangementRepositoryLocal WetlandPlantLeafArrangementRepoLocal { get; set; }
        public static WetlandPlantSizeRepositoryLocal WetlandPlantSizeRepoLocal { get; set; }
        public static WetlandCountyPlantRepositoryLocal WetlandCountyPlantRepoLocal { get; set; }
        public static WetlandPlantRegionRepositoryLocal WetlandRegionRepoLocal { get; set; }
        public static object WetlandSettingRepository { get; internal set; }

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
            WetlandSettingsRepo = new WetlandSettingRepository();
            WetlandSearchRepo = new WetlandSearchRepository();
            WetlandPlantDivisionRepo = new WetlandPlantDivisionRepository();
            WetlandPlantShapeRepo = new WetlandPlantShapeRepository();
            WetlandPlantLeafArrangementRepo = new WetlandPlantLeafArrangementRepository();
            WetlandPlantSizeRepo = new WetlandPlantSizeRepository();
            WetlandCountyPlantRepo = new WetlandCountyPlantRepository();
            WetlandRegionRepo = new WetlandPlantRegionRepository();

            WetlandPlantRepoLocal = new WetlandPlantRepositoryLocal(WetlandPlantRepo.GetAllWetlandPlants());
            WetlandPlantImageRepoLocal = new WetlandPlantImageRepositoryLocal(WetlandPlantImageRepo.GetAllWetlandPlantImages());
            WetlandPlantFruitsRepoLocal = new WetlandPlantFruitsRepositoryLocal(WetlandPlantFruitsRepo.GetAllWetlandFruits());
            WetlandPlantDivisionRepoLocal = new WetlandPlantDivisionRepositoryLocal(WetlandPlantDivisionRepo.GetAllDivisions());
            WetlandPlantShapeRepoLocal = new WetlandPlantShapeRepositoryLocal(WetlandPlantShapeRepo.GetAllShapes());
            WetlandPlantLeafArrangementRepoLocal = new WetlandPlantLeafArrangementRepositoryLocal(WetlandPlantLeafArrangementRepo.GetAllArrangements());
            WetlandPlantSizeRepoLocal = new WetlandPlantSizeRepositoryLocal(WetlandPlantSizeRepo.GetAllPlantSizes());
            WetlandCountyPlantRepoLocal = new WetlandCountyPlantRepositoryLocal(WetlandCountyPlantRepo.GetAllCounties());
            WetlandRegionRepoLocal = new WetlandPlantRegionRepositoryLocal(WetlandRegionRepo.GetAllWetlandRegions());
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
