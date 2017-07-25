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

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PortableApp.Droid
{
    public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
    {
        GoogleMap map;
        CustomMap formsMap;
        WetlandMapOverlay overlay;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {
                formsMap = (CustomMap)e.NewElement;

                ((MapView)Control).GetMapAsync(this);
            }
        }
        
        public void OnMapReady(GoogleMap googleMap)
        {
            InvokeOnMapReadyBaseClassHack(googleMap);
            map = googleMap;

            for (int i = 0; i < formsMap.Overlays.Count; i++)
            {
                overlay = formsMap.Overlays[i];

                var polygonOptions = new PolygonOptions();
                polygonOptions.InvokeFillColor(overlay.legendColor.ToAndroid());
                polygonOptions.InvokeStrokeColor(Color.Gray.ToAndroid());
                polygonOptions.InvokeStrokeWidth(0.8f);

                foreach (var position in overlay.overlayCoordinates)
                {
                    polygonOptions.Add(new LatLng(position.latitude, position.longitude));
                }

                map.AddPolygon(polygonOptions);
            }
            map.UiSettings.SetAllGesturesEnabled(true);
        }

        private void InvokeOnMapReadyBaseClassHack(GoogleMap googleMap)
        {
            System.Reflection.MethodInfo onMapReadyMethodInfo = null;

            Type baseType = typeof(MapRenderer);
            foreach (var currentMethod in baseType.GetMethods(System.Reflection.BindingFlags.NonPublic |
                                                             System.Reflection.BindingFlags.Instance |
                                                              System.Reflection.BindingFlags.DeclaredOnly))
            {

                if (currentMethod.IsFinal && currentMethod.IsPrivate)
                {
                    if (string.Equals(currentMethod.Name, "OnMapReady", StringComparison.Ordinal))
                    {
                        onMapReadyMethodInfo = currentMethod;

                        break;
                    }

                    if (currentMethod.Name.EndsWith(".OnMapReady", StringComparison.Ordinal))
                    {
                        onMapReadyMethodInfo = currentMethod;

                        break;
                    }
                }
            }

            if (onMapReadyMethodInfo != null)
            {
                onMapReadyMethodInfo.Invoke(this, new[] { googleMap });
            }
        }
    }
}