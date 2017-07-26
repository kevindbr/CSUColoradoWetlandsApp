using PortableApp.Droid;
using PortableApp;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System;
using PortableApp.Models;
using Xamarin.Forms.Platform.Android;
using Esri.ArcGISRuntime.UI.Controls;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PortableApp.Droid
{
    public class CustomMapRenderer : ViewRenderer
    {
        GoogleMap map;
        CustomMap formsMap;
        WetlandMapOverlay overlay;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {                
                // prepare mapview
                var mapView = Control as Esri.ArcGISRuntime.UI.Controls.MapView;
                mapView = new Esri.ArcGISRuntime.UI.Controls.MapView();

                // create two layers
                var baselayer = new ArcGISTiledLayer(new Uri("https://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer"));
                var overlay = new ArcGISMapImageLayer(new Uri("http://sampleserver6.arcgisonline.com/arcgis/rest/services/Census/MapServer"));
                var overlay2 = new ArcGISMapImageLayer(new Uri("http://cnhp.colostate.edu/arcgis/rest/services/wetlands/Colorado_Wetlands_NWI/MapServer"));

                // create a new basemap; add the layers (BaseLayers property)
                var basemap = new Basemap();
                basemap.BaseLayers.Add(baselayer);
                basemap.BaseLayers.Add(overlay);
                basemap.BaseLayers.Add(overlay2);

                // create a new Map to display the basemap and assign to mapView, set native control 
                var map = new Esri.ArcGISRuntime.Mapping.Map { Basemap = basemap, InitialViewpoint = new Viewpoint(40.5592, -105.0781, 500000000) };

                mapView.Map = map;
                SetNativeControl(mapView);

                //formsMap = (CustomMap)e.NewElement;

                //((MapView)Control).GetMapAsync(this);
            }
        }
        
        //public void OnMapReady(GoogleMap googleMap)
        //{
        //    InvokeOnMapReadyBaseClassHack(googleMap);
        //    map = googleMap;

        //    for (int i = 0; i < formsMap.Overlays.Count; i++)
        //    {
        //        overlay = formsMap.Overlays[i];

        //        var polygonOptions = new PolygonOptions();
        //        polygonOptions.InvokeFillColor(overlay.legendColor.ToAndroid());
        //        polygonOptions.InvokeStrokeColor(Color.Gray.ToAndroid());
        //        polygonOptions.InvokeStrokeWidth(0.8f);

        //        foreach (var position in overlay.overlayCoordinates)
        //        {
        //            polygonOptions.Add(new LatLng(position.latitude, position.longitude));
        //        }

        //        map.AddPolygon(polygonOptions);
        //    }
        //    map.UiSettings.SetAllGesturesEnabled(true);
        //}

        //private void InvokeOnMapReadyBaseClassHack(GoogleMap googleMap)
        //{
        //    System.Reflection.MethodInfo onMapReadyMethodInfo = null;

        //    Type baseType = typeof(MapRenderer);
        //    foreach (var currentMethod in baseType.GetMethods(System.Reflection.BindingFlags.NonPublic |
        //                                                     System.Reflection.BindingFlags.Instance |
        //                                                      System.Reflection.BindingFlags.DeclaredOnly))
        //    {

        //        if (currentMethod.IsFinal && currentMethod.IsPrivate)
        //        {
        //            if (string.Equals(currentMethod.Name, "OnMapReady", StringComparison.Ordinal))
        //            {
        //                onMapReadyMethodInfo = currentMethod;

        //                break;
        //            }

        //            if (currentMethod.Name.EndsWith(".OnMapReady", StringComparison.Ordinal))
        //            {
        //                onMapReadyMethodInfo = currentMethod;

        //                break;
        //            }
        //        }
        //    }

        //    if (onMapReadyMethodInfo != null)
        //    {
        //        onMapReadyMethodInfo.Invoke(this, new[] { googleMap });
        //    }
        //}
    }
}