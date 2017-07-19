using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PortableApp.Models
{
    [Table("wetland_map_overlay")]
    public class WetlandMapOverlay
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string mapKeyName { get; set; }

        public string legendName { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<WetlandMapOverlayCoordinate> overlayCoordinates { get; set; }

        public Color legendColor
        {
            get
            {
                if (legendName == "Emergent")
                    return Color.Green;
                else if (legendName == "Forested")
                    return Color.Brown;
                else if (legendName == "Lake")
                    return Color.Blue;
                else
                    return Color.Transparent;
            }
        }

        //public IEnumerable<WetlandMapOverlayCoordinate> overlayCoordinates
        //{
        //    get { return JsonConvert.DeserializeObject<IList<WetlandMapOverlayCoordinate>>(coordinatesString); }
        //}

    }

    [Table("wetland_map_overlay_coordinate")]
    public class WetlandMapOverlayCoordinate
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [ForeignKey(typeof(WetlandMapOverlay))]
        public int overlayid { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
