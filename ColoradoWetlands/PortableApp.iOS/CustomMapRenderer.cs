using CoreLocation;
using MapKit;
using PortableApp.iOS;
using PortableApp;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;
using System.Collections.Generic;
using PortableApp.Models;
using Xamarin.Forms.Maps;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PortableApp.iOS
{
    public class CustomMapRenderer : MapRenderer
    {
        CustomMap formsMap;
        List<MKPolygon> polygonOverlays = new List<MKPolygon>();
        //List<MKPolygonRenderer> polygonRenderers = new List<MKPolygonRenderer>();
        WetlandMapOverlay overlay;
        string pinId = "PinnAnnotation";
        List<UIColor> overlayColors = new List<UIColor>();
        int colorIndex = 0;
        UIColor legendColor;

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
                formsMap = (CustomMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                nativeMap.ShowsScale = true;

                for (int i = 0; i < formsMap.Overlays.Count; i++)
                {
                    overlay = formsMap.Overlays[i];
                    UIColor color = DetermineOverlayColor(overlay.legendColor);
                    overlayColors.Add(color);

                    nativeMap.OverlayRenderer = GetOverlayRenderer;

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

                    formsMap.Pins.Add(new Pin { Label = overlay.mapKeyName, Position = new Position(coordinatePairs[0].latitude, coordinatePairs[0].longitude) });
                    nativeMap.GetViewForAnnotation = GetViewForAnnotation;

                }
            }
        }

        // Create instance of MKPolygon overlay renderer and return
        MKOverlayRenderer GetOverlayRenderer(MKMapView mapView, IMKOverlay overlay)
        {
            MKPolygonRenderer polygonRenderer = new MKPolygonRenderer(overlay as MKPolygon);
            polygonRenderer.FillColor = overlayColors[colorIndex];
            colorIndex++;
            polygonRenderer.StrokeColor = UIColor.Gray;
            polygonRenderer.Alpha = 0.8f;
            polygonRenderer.LineWidth = 1;
            return polygonRenderer;
        }

        // Create custom pin
        MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;
            
            annotationView = mapView.DequeueReusableAnnotation(pinId);
            if (annotationView == null)
            {
                annotationView = new MKAnnotationView(annotation, pinId);
                annotationView.Image = UIImage.FromFile("MapLabels/" + annotation.GetTitle() + ".png");
            }

            return annotationView;
        }

        UIColor DetermineOverlayColor(string legendName)
        {
            if (legendName == "Emergent")
                legendColor = UIColor.Green;
            else if (legendName == "Forested")
                legendColor = UIColor.Brown;
            else if (legendName == "Lake")
                legendColor = UIColor.Blue;

            return legendColor;
        }
    }
}