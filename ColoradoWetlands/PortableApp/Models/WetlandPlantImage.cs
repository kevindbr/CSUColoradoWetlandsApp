using PCLStorage;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace PortableApp.Models
{
    [Table("wetland_plant_images")]
    public class WetlandPlantImage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(WetlandPlant))]
        public int PlantId { get; set; }

        [ManyToOne]
        public WetlandPlant Plant { get; set; }

        [MaxLength(250), Unique]
        public string FileName { get; set; }

        [MaxLength(250)]
        public string Credit { get; set; }

        public IFolder rootFolder { get { return FileSystem.Current.LocalStorage; } }
        public string ImagePathDownloaded { get { return rootFolder.Path + "/Images/" + FileName; } }

        public string ImagePathStreamed { get { return "http://sdt1.cas.colostate.edu/mobileapi/api/wetland/images/" + Id; } }

        public string ImageCredit { get { return string.Format("Image Credit: {0}", Credit); } }

    }
}
