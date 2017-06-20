using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;

namespace PortableApp.Models
{
    [Table("wetland_search")]
    public class WetlandSearch
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Query { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string SearchString1 { get; set; }
        public string SearchString2 { get; set; }
        public string SearchString3 { get; set; }
    }
}
