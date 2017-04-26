using SQLite;
using Xamarin.Forms;

namespace PortableApp.Models
{
    [Table("wetland_plants")]
    public class WetlandPlant
    {
        [PrimaryKey, AutoIncrement]
        public int plantid { get; set; }

        public int id { get; set; }
        public string scinameauthor { get; set; }
        public string scinamenoauthor { get; set; }
        public string family { get; set; }
        public string commonname { get; set; }
        public string plantscode { get; set; }

        public string FileName { get; set; }

        public ImageSource Thumbnail
        {
            get { return ImageSource.FromResource(string.Format("PortableApp.Resources.Images.Plants.{0}", FileName)); }
        }
    }
}
