using SQLite.Net.Attributes;

namespace PortableApp.Models
{
    [Table("wetland_types")]
    public class WetlandType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

    }
}
