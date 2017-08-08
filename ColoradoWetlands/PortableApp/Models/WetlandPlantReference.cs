using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace PortableApp.Models
{
    [Table("wetland_plant_references")]
    public class WetlandPlantReference
    {
        [PrimaryKey, Unique]
        public int ReferenceId { get; set; }

        [ForeignKey(typeof(WetlandPlant))]
        public int PlantId { get; set; }

        [ManyToOne]
        public WetlandPlant Plant { get; set; }

        public string reference { get; set; }

        public string fullcitation { get; set; }

    }
}
