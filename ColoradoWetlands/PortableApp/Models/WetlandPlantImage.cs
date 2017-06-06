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

        public string ImagePath
        {
            get
            {
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                return rootFolder.Path + "/Images/" + FileName;
            }
        }

        public string ImageCredit
        {
            get { return string.Format("Image Credit: {0}", Credit); }
        }

    }
}
