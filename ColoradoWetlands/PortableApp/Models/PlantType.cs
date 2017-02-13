using SQLite;

namespace PortableApp.Models
{
    [Table("plant_types")]
    public class PlantType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }
    }
}
