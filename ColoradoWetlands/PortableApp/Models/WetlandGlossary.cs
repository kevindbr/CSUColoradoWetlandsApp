using PCLStorage;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace PortableApp.Models
{
    [Table("wetland_glossary")]
    public class WetlandGlossary
    {
        [PrimaryKey]
        public int glossaryid { get; set; }

        public int id { get; set; }

        public string name { get; set; }

        public string definition { get; set; }

        public string firstInitial { get { return name[0].ToString(); } }
    }
}
