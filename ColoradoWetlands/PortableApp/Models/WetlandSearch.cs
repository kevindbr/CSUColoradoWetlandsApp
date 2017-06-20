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
    }
}
