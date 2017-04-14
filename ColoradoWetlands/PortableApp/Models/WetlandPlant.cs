using SQLite;
using Xamarin.Forms;

namespace PortableApp.Models
{
    [Table("wetland_plants")]
    public class WetlandPlant
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250)]
        public string CommonName { get; set; }

        public string Description { get; set; }

        public string Description2 { get; set; }

        public string Description3 { get; set; }

        public string FileName { get; set; }

        public ImageSource Thumbnail
        {
            get { return ImageSource.FromResource(string.Format("PortableApp.Resources.Images.Plants.{0}", FileName)); }
        }
    }
}
