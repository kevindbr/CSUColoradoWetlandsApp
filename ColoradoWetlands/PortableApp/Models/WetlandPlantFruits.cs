using PCLStorage;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace PortableApp.Models
{
    [Table("wetland_fruits")]
    public class WetlandPlantFruits
    {
        [PrimaryKey]
        public int fruitid { get; set; }

        [ForeignKey(typeof(WetlandPlant))]
        public int plantid { get; set; }

        public int valueid { get; set; }

    }

    [Table("wetland_divisions")]
    public class WetlandPlantDivision
    {
        [PrimaryKey]
        public int divisionid { get; set; }

        [ForeignKey(typeof(WetlandPlant))]
        public int plantid { get; set; }

        public int valueid { get; set; }

    }


    [Table("wetland_leaf_shapes")]
    public class WetlandPlantShape
    {
        [PrimaryKey]
        public int shapeid { get; set; }

        [ForeignKey(typeof(WetlandPlant))]
        public int plantid { get; set; }

        public int valueid { get; set; }
    }

    [Table("leaf_arrangements")]
    public class WetlandPlantArrangement
    {
        [PrimaryKey]
        public int arrangementid { get; set; }

        [ForeignKey(typeof(WetlandPlant))]
        public int plantid { get; set; }

        public int valueid { get; set; }

    }

    [Table("plant_sizes")]
    public class WetlandPlantSize
    {
        [PrimaryKey]
        public int sizeid { get; set; }

        [ForeignKey(typeof(WetlandPlant))]
        public int plantid { get; set; }

        public int valueid { get; set; }

    }

    [Table("wetland_county_plant")]
    public class WetlandCountyPlant
    {
        [PrimaryKey]
        public int countyplantid { get; set; }

        public int county_id { get; set; }

        [ForeignKey(typeof(WetlandPlant))]
        public int plantid { get; set; }

        public string name { get; set; }

    }
}
