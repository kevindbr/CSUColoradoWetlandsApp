using PortableApp.Droid;
using PortableApp;
using Xamarin.Forms;
using System;
using Xamarin.Forms.Platform.Android;
using Esri.ArcGISRuntime.Mapping;
using Plugin.Geolocator;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PortableApp.Droid
{
    public class CustomMapRenderer : ViewRenderer<CustomMap, Android.Views.ViewGroup>
    {
        global::Android.Widget.RelativeLayout mapContainer;
        Esri.ArcGISRuntime.UI.Controls.MapView mapView;
        global::Android.Widget.ImageButton locationButton;
        Plugin.Geolocator.Abstractions.Position position;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomMap> e)
        {
            base.OnElementChanged(e);
            
            if (Control == null)
            {
                // create linear layout container for search and map
                var container = new Android.Widget.LinearLayout(this.Context);
                container.Orientation = Android.Widget.Orientation.Vertical;
                container.Measure(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.MatchParent);

                // prepare search bar
                var searchBar = new Android.Widget.SearchView(this.Context);
                searchBar.Measure(Android.Views.ViewGroup.LayoutParams.MatchParent, 50);
                searchBar.SetBackgroundColor(Android.Graphics.Color.LightGray);
                searchBar.SetQueryHint("Search for address or city...");
                searchBar.QueryTextSubmit += ProcessSearch;
                container.AddView(searchBar);

                // construct container for mapview
                mapContainer = new Android.Widget.RelativeLayout(this.Context);
                mapContainer.Measure(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.MatchParent);

                // prepare mapview
                mapView = new Esri.ArcGISRuntime.UI.Controls.MapView();
                mapView.Measure(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.MatchParent);

                // create two layers
                var wetlandsMapBase = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/Colorado_Wetlands_NWI/MapServer"));
                var satelliteMap = new ArcGISTiledLayer(new Uri("https://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer"));
                var wetlandsMap = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/Colorado_Wetlands_NWI/MapServer"));

                // create a new basemap; add the layers (BaseLayers property)
                var basemap = new Basemap(); 
                basemap.BaseLayers.Add(wetlandsMapBase);
                basemap.BaseLayers.Add(satelliteMap);
                basemap.BaseLayers.Add(wetlandsMap);

                // create a new Map to display the basemap and assign to mapView, add to layout                                         //100000000
                var map = new Esri.ArcGISRuntime.Mapping.Map { Basemap = basemap, InitialViewpoint = new Viewpoint(40.5592, -105.0781, 100000) };
                mapView.Map = map;
                mapContainer.AddView(mapView);

                // create current location button
                locationButton = new Android.Widget.ImageButton(this.Context);
                locationButton.SetImageResource(Resource.Drawable.gpsoutline);
                locationButton.Measure(25, 25);
                locationButton.SetPadding(25, 25, 0, 0);
                locationButton.SetBackgroundColor(Android.Graphics.Color.Transparent);
                locationButton.Click += delegate {
                    ToggleLocation();
                };
                ToggleLocation();


                mapContainer.AddView(locationButton);

                // add map to layout container
                container.AddView(mapContainer);

                // set native control to layout
                SetNativeControl(container);

            }
        }

        // process address search
        private async void ProcessSearch(object sender, EventArgs e)
        {
            var search = (Android.Widget.SearchView)sender;
            if (search.Query != "")
            {
                var uri = new Uri("http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer");
                var locatorTask = new Esri.ArcGISRuntime.Tasks.Geocoding.LocatorTask(uri);
                var info = locatorTask.LocatorInfo;

                var matches = await locatorTask.GeocodeAsync(search.Query);

                if (matches.Count != 0)
                    await mapView.SetViewpointCenterAsync(matches[0].DisplayLocation);
            }
        }

        private async void ToggleLocation()
        {
            if (mapView.LocationDisplay.IsEnabled)
            {
                mapView.LocationDisplay.IsEnabled = false;
                locationButton.SetImageResource(Resource.Drawable.gpsoutline);
            }
            else
            {
                try
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;

                    position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                    if (position == null)
                        return;

                    mapView.LocationDisplay.IsEnabled = true;
                    locationButton.SetImageResource(Resource.Drawable.gpsfilled);                       //10000000
                    await mapView.SetViewpointAsync(new Viewpoint(position.Latitude, position.Longitude, 5000));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to get location, may need to increase timeout: " + ex);
                }
            }
        }
    }
}