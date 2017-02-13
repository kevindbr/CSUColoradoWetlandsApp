using SQLite;

namespace PortableApp.Models
{
    [Table("plants")]
    public class Plant
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250)]
        public string CommonName { get; set; }
    }
}
