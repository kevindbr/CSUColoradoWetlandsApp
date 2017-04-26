using SQLite;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class App : Application
    {
        public static WetlandPlantRepository WetlandPlantRepo { get; private set; }
        public static WetlandPlantImageRepository WetlandPlantImageRepo { get; private set; }
        public static WetlandTypeRepository WetlandTypeRepo { get; private set; }

        public App(string dbPath)
        {
            InitializeComponent();

            // Initialize SQLite connection and DBConnection class to hold connection
            SQLiteConnection newConn = new SQLiteConnection(dbPath);
            DBConnection dbConn = new DBConnection(newConn);

            // Initialize repositories
            WetlandPlantRepo = new WetlandPlantRepository(dbPath);
            WetlandPlantImageRepo = new WetlandPlantImageRepository(dbPath);
            WetlandTypeRepo = new WetlandTypeRepository(dbPath);

            // Set MainPage
            this.MainPage = new NavigationPage(new MainPage(dbPath));
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
