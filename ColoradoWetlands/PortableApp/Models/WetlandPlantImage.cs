using SQLite;
using Xamarin.Forms;

namespace PortableApp.Models
{
    [Table("wetland_plant_images")]
    public class WetlandPlantImage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PlantId { get; set; }

        [MaxLength(250)]
        public string FileName { get; set; }

        [MaxLength(250)]
        public string Credit { get; set; }

        public ImageSource ImageSource
        {
            get { return ImageSource.FromResource(string.Format("PortableApp.Resources.Images.Plants.Plant{0}.{1}", PlantId, FileName)); }
        }

        public string ImageCredit
        {
            get { return string.Format("Image Credit: {0}", Credit); }
        }

    }
}
