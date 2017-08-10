using PortableApp.Droid;
using PortableApp;
using Xamarin.Forms;
using Android.Gms.Maps;
using System;
using PortableApp.Models;
using Xamarin.Forms.Platform.Android;
using Esri.ArcGISRuntime.Mapping;
using Android.App;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PortableApp.Droid
{
    public class CustomMapRenderer : ViewRenderer<CustomMap, Android.Views.ViewGroup>
    {
        global::Android.Views.ViewGroup viewGroup;
        Esri.ArcGISRuntime.UI.Controls.MapView mapView;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomMap> e)
        {
            base.OnElementChanged(e);
            
            if (Control == null)
            {

                //viewGroup = ViewGroup;

                //global::Android.Widget.RelativeLayout layout = new global::Android.Widget.RelativeLayout(viewGroup.Context);

                //// prepare search bar
                //var searchFrame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 44);
                //searchBar = new UISearchBar(searchFrame);
                //searchBar.Placeholder = "Search for address or city...";
                //searchBar.SearchButtonClicked += ProcessSearch;
                //layout.AddSubview(searchBar);

                // prepare mapview
                mapView = new Esri.ArcGISRuntime.UI.Controls.MapView();
                mapView.Layout(0, 0, 0, 0);

                // create two layers
                var wetlandsMapBase = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/Colorado_Wetlands_NWI/MapServer"));
                var satelliteMap = new ArcGISTiledLayer(new Uri("https://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer"));
                var wetlandsMap = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/Colorado_Wetlands_NWI/MapServer"));

                // create a new basemap; add the layers (BaseLayers property)
                var basemap = new Basemap();
                basemap.BaseLayers.Add(wetlandsMapBase);
                basemap.BaseLayers.Add(satelliteMap);
                basemap.BaseLayers.Add(wetlandsMap);

                // create a new Map to display the basemap and assign to mapView, add to layout
                var map = new Esri.ArcGISRuntime.Mapping.Map { Basemap = basemap, InitialViewpoint = new Viewpoint(40.5592, -105.0781, 100000000) };
                mapView.Map = map;
                
                //layout.AddView(mapView);
                //viewGroup.AddView(layout);

                // set native control to layout
                SetNativeControl(mapView);

                //layout.AddSubview(mapView);

                //// create current location button
                //locationButton = UIButton.FromType(UIButtonType.Custom);
                //locationButton.SetImage(UIImage.FromFile("location-white-outline.png"), UIControlState.Normal);
                //locationButton.Frame = new CGRect(20, 64, 28, 28);
                //locationButton.TouchUpInside += delegate {
                //    ToggleLocation();
                //};
                //layout.AddSubview(locationButton);

                //// prepare compass
                //UIImageView compass = new UIImageView(UIImage.FromFile("compass-icon.png"));
                //compass.Frame = new CGRect(UIScreen.MainScreen.Bounds.Width - 50, 64, 30, 30);
                //mapView.ViewpointChanged += (sender, args) =>
                //{
                //    compass.Transform = CGAffineTransform.MakeRotation((float)-mapView.MapRotation/100);
                //};
                //layout.AddSubview(compass);



                //// prepare mapview
                //var mapView = Control as Esri.ArcGISRuntime.UI.Controls.MapView;
                //mapView = new Esri.ArcGISRuntime.UI.Controls.MapView();

                //// create two layers
                //var wetlandsMapBase = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/Colorado_Wetlands_NWI/MapServer"));
                //var satelliteMap = new ArcGISTiledLayer(new Uri("https://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer"));                
                ////var counties = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/CO_counties_Appservice/MapServer"));
                ////var elev = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/elevation_poly_Appservice/MapServer"));
                //var wetlandsMap = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/Colorado_Wetlands_NWI/MapServer"));

                //// create a new basemap; add the layers (BaseLayers property)
                //var basemap = new Basemap();
                //basemap.BaseLayers.Add(wetlandsMapBase);
                //basemap.BaseLayers.Add(satelliteMap);
                ////basemap.BaseLayers.Add(counties);
                ////basemap.BaseLayers.Add(elev);
                //basemap.BaseLayers.Add(wetlandsMap);



                //// create a new Map to display the basemap and assign to mapView, set native control 
                //var map = new Esri.ArcGISRuntime.Mapping.Map { Basemap = basemap, InitialViewpoint = new Viewpoint(40.5592, -105.0781, 100000000) };
                //mapView.Map = map;

                ////mapView.LocationDisplay.IsEnabled = true;
                ////if (mapView.LocationDisplay.IsEnabled)
                ////{
                ////    // Get the current AutoPanMode setting as it is automatically disabled when calling MyMapView.SetView().
                ////    var PanMode = mapView.LocationDisplay.AutoPanMode;

                ////    mapView.SetViewpointRotationAsync(0);
                ////    mapView.SetViewpoint(mapView.GetCurrentViewpoint(ViewpointType.BoundingGeometry));

                ////    // Reset the AutoPanMode 
                ////    mapView.LocationDisplay.AutoPanMode = PanMode;
                ////}
                //mapView.SetViewpoint(new Viewpoint(40.5592, -105.0781, 1000000000));

                //SetNativeControl(mapView);

            }
        }
    }
}