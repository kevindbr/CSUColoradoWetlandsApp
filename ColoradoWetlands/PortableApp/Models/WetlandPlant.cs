using SQLite;

namespace PortableApp.Models
{
    [Table("wetland_plants")]
    public class WetlandPlant
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250)]
        public string CommonName { get; set; }
    }
}
