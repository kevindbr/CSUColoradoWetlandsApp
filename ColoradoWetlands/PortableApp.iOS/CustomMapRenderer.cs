using PortableApp.iOS;
using PortableApp;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Collections.Generic;
using Esri.ArcGISRuntime.UI.Controls;
using Esri.ArcGISRuntime.Mapping;
using System;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PortableApp.iOS
{
    public class CustomMapRenderer : ViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            // remove previous map
            if (e.OldElement != null)
            {
                //var nativeMap = Control as MKMapView;
                //nativeMap.OverlayRenderer = null;
            }

            // create new map
            if (e.NewElement != null)
            {
                // prepare mapview
                var mapView = Control as MapView;
                mapView = new MapView();

                // create two layers
                var wetlandsMapBase = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/Colorado_Wetlands_NWI/MapServer"));
                var satelliteMap = new ArcGISTiledLayer(new Uri("https://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer"));
                //var counties = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/CO_counties_Appservice/MapServer"));
                //var elev = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/elevation_poly_Appservice/MapServer"));
                var wetlandsMap = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/Colorado_Wetlands_NWI/MapServer"));

                // create a new basemap; add the layers (BaseLayers property)
                var basemap = new Basemap();
                basemap.BaseLayers.Add(wetlandsMapBase);
                basemap.BaseLayers.Add(satelliteMap);
                //basemap.BaseLayers.Add(counties);
                //basemap.BaseLayers.Add(elev);
                basemap.BaseLayers.Add(wetlandsMap);

                // create a new Map to display the basemap and assign to mapView, set native control 
                var map = new Esri.ArcGISRuntime.Mapping.Map { Basemap = basemap, InitialViewpoint = new Viewpoint(40.5592, -105.0781, 100000000) };
                mapView.Map = map;
                SetNativeControl(mapView);
                
            }
        }        
    }
}