using CoreLocation;
using MapKit;
using PortableApp.iOS;
using PortableApp;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;
using System.Collections.Generic;
using System;
using PortableApp.Models;
using Xamarin.Forms.Maps;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PortableApp.iOS
{
    public class CustomMapRenderer : MapRenderer
    {
        List<MKPolygon> polygonOverlays = new List<MKPolygon>();
        MKPolygonRenderer polygonRenderer;
        List<MKPolygonRenderer> polygonRenderers = new List<MKPolygonRenderer>();
        WetlandMapOverlay overlay;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            // remove previous map
            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                nativeMap.OverlayRenderer = null;
            }

            // create new map
            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                var nativeMap = Control as MKMapView;

                for (int i = 0; i < formsMap.Overlays.Count; i++)
                {
                    nativeMap.OverlayRenderer = GetOverlayRenderer;

                    overlay = formsMap.Overlays[i];
                    List<WetlandMapOverlayCoordinate> coordinatePairs = formsMap.Overlays[i].overlayCoordinates;
                    CLLocationCoordinate2D[] coords = new CLLocationCoordinate2D[coordinatePairs.Count];

                    int index = 0;
                    foreach (var position in coordinatePairs)
                    {
                        coords[index] = new CLLocationCoordinate2D(position.latitude, position.longitude);
                        index++;
                    }

                    var blockOverlay = MKPolygon.FromCoordinates(coords);
                    nativeMap.AddOverlay(blockOverlay);

                    formsMap.Pins.Add(new Pin { Label = "", Position = new Position(coordinatePairs[0].latitude, coordinatePairs[0].longitude) });

                    nativeMap.GetViewForAnnotation = GetViewForAnnotation;

                }
            }
        }



        // Create instance of MKPolygon overlay renderer and return
        MKOverlayRenderer GetOverlayRenderer (MKMapView mapView, IMKOverlay overlay)
        {
            polygonRenderer = new MKPolygonRenderer(overlay as MKPolygon);
            polygonRenderer.FillColor = UIColor.Red;
            polygonRenderer.StrokeColor = UIColor.Blue;
            polygonRenderer.Alpha = 0.4f;
            polygonRenderer.LineWidth = 9;
            return polygonRenderer;
        }

        // Create custom pin
        MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;
            
            annotationView = mapView.DequeueReusableAnnotation("0");
            if (annotationView == null)
            {
                annotationView = new MKAnnotationView(annotation, "0");
                annotationView.Image = UIImage.FromFile("MapLabels/" + overlay.mapKeyName + ".png");
            }

            return annotationView;
        }
    }
}