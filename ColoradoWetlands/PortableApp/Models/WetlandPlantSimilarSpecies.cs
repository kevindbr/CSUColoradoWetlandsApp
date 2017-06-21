using PCLStorage;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace PortableApp.Models
{
    [Table("wetland_plant_similar_species")]
    public class WetlandPlantSimilarSpecies
    {
        [PrimaryKey]
        public int similarspeciesid { get; set; }

        public int id { get; set; }

        [ForeignKey(typeof(WetlandPlant))]
        public int PlantId { get; set; }

        public string similarspicon { get; set; }

        public string similarspscinameauthor { get; set; }

        public string reason { get; set; }

        public IFolder rootFolder { get { return FileSystem.Current.LocalStorage; } }

        public string ThumbnailPath { get { return rootFolder.Path + "/Images/" + similarspicon; } }

        public string similarspscinameauthorstripped
        {
            get { return similarspscinameauthor.Replace("<em>", "").Replace("</em>", ""); }
        }

        public string reasonstripped
        {
            get { return reason.Replace("<em>", "").Replace("</em>", ""); }
        }

    }
}
