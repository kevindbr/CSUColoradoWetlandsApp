using PortableApp.iOS;
using PortableApp;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Platform.iOS;
using System.Collections.Generic;
using Esri.ArcGISRuntime.UI.Controls;
using Esri.ArcGISRuntime.Mapping;
using System;
using System.Drawing;
using Plugin.Geolocator;
using CoreGraphics;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Geometry;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PortableApp.iOS
{
    public class CustomMapRenderer : ViewRenderer<CustomMap, UIView>
    {
        UISearchBar searchBar;
        MapView mapView;
        Plugin.Geolocator.Abstractions.Position position;
        UIView layout;
        UIButton locationButton;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomMap> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                // prepare layout
                layout = new UIView();

                // prepare search bar
                var searchFrame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 44);
                searchBar = new UISearchBar(searchFrame);
                searchBar.Placeholder = "Search for address or city...";
                searchBar.SearchButtonClicked += ProcessSearch;
                layout.AddSubview(searchBar);

                // prepare mapview
                mapView = new MapView();
                mapView.Frame = new CGRect(0, 44, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

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
                layout.AddSubview(mapView);

                // create current location button
                locationButton = UIButton.FromType(UIButtonType.Custom);
                locationButton.SetImage(UIImage.FromFile("location-white-outline.png"), UIControlState.Normal);
                locationButton.Frame = new CGRect(20, 64, 28, 28);
                locationButton.TouchUpInside += delegate {
                    ToggleLocation();
                };
                layout.AddSubview(locationButton);

                //// prepare compass
                //UIImageView compass = new UIImageView(UIImage.FromFile("compass-icon.png"));
                //compass.Frame = new CGRect(UIScreen.MainScreen.Bounds.Width - 50, 64, 30, 30);
                //mapView.ViewpointChanged += (sender, args) =>
                //{
                //    compass.Transform = CGAffineTransform.MakeRotation((float)-mapView.MapRotation/100);
                //};
                //layout.AddSubview(compass);

                // set native control to layout
                SetNativeControl(layout);
            }                        
        }

        // process address search
        private async void ProcessSearch(object sender, EventArgs e)
        {
            var search = (UISearchBar)sender;
            if (search.Text != "")
            {
                var uri = new Uri("http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer");
                var locatorTask = new Esri.ArcGISRuntime.Tasks.Geocoding.LocatorTask(uri);
                var info = locatorTask.LocatorInfo;

                var matches = await locatorTask.GeocodeAsync(search.Text);

                if (matches.Count != 0)
                    await mapView.SetViewpointCenterAsync(matches[0].DisplayLocation);
            }
        }

        private async void ToggleLocation()
        {
            if (mapView.LocationDisplay.IsEnabled)
            {
                mapView.LocationDisplay.IsEnabled = false;
                locationButton.SetImage(UIImage.FromFile("location-white-outline.png"), UIControlState.Normal);
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
                    locationButton.SetImage(UIImage.FromFile("location-white.png"), UIControlState.Normal);
                    await mapView.SetViewpointAsync(new Viewpoint(position.Latitude, position.Longitude, 10000000));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to get location, may need to increase timeout: " + ex);
                }
            }
        }
    }
}