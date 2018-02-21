using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using CarouselView.FormsPlugin.Android;

namespace PortableApp.Droid
{
    [Activity(Label = "Colorado Wetlands", Icon = "@drawable/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            CarouselViewRenderer.Init();
            FFImageLoading.Forms.Droid.CachedImageRenderer.Init();

            string dbPath = FileAccessHelper.GetLocalFilePath("db.db3");
            var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            LoadApplication(new PortableApp.App(platform, dbPath));
        }
    }
}

