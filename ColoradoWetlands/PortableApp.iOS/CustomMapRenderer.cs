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

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PortableApp.iOS
{
    public class CustomMapRenderer : ViewRenderer<CustomMap, UIView>
    {
        MapView mapView;
        public Plugin.Geolocator.Abstractions.Position position;
        UIView layout;
        UIButton locationButton;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomMap> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                // prepare layout
                layout = new UIView();

                // prepare mapview
                mapView = new MapView();
                mapView.Frame = UIScreen.MainScreen.Bounds;

                // create two layers
                var wetlandsMapBase = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/Colorado_Wetlands_NWI/MapServer"));
                var satelliteMap = new ArcGISTiledLayer(new Uri("https://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer"));
                var wetlandsMap = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/Colorado_Wetlands_NWI/MapServer"));

                // create a new basemap; add the layers (BaseLayers property)
                var basemap = new Basemap();
                basemap.BaseLayers.Add(wetlandsMapBase);
                basemap.BaseLayers.Add(satelliteMap);
                basemap.BaseLayers.Add(wetlandsMap);

                // create a new Map to display the basemap and assign to mapView, set native control 
                var map = new Esri.ArcGISRuntime.Mapping.Map { Basemap = basemap, InitialViewpoint = new Viewpoint(40.5592, -105.0781, 100000000) };
                mapView.Map = map;

                // create current location button
                locationButton = UIButton.FromType(UIButtonType.Custom);
                locationButton.SetImage(UIImage.FromFile("location-white-outline.png"), UIControlState.Normal);
                locationButton.Frame = new CGRect(20, 20, 28, 28);
                locationButton.TouchUpInside += delegate {
                    ToggleLocation();
                };

                // add mapview and location button to layout, set native control to layout
                layout.AddSubviews(mapView, locationButton);
                SetNativeControl(layout);
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