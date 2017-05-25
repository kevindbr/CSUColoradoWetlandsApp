using PCLStorage;
using SQLite;
using System.IO;
using System.Threading.Tasks;
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

        public string ImagePath
        {
            get
            {
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                return rootFolder + "/Images/" + FileName;
            }
        }

        public string ImageCredit
        {
            get { return string.Format("Image Credit: {0}", Credit); }
        }

    }
}
