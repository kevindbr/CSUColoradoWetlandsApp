using PCLStorage;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PortableApp.Models
{
    [Table("wetland_plants")]
    public class WetlandPlant
    {
        [PrimaryKey]
        public int plantid { get; set; }

        [Unique]
        public int id { get; set; }
        public string scinameauthor { get; set; }
        public string scinamenoauthor { get; set; }
        public string family { get; set; }
        public string commonname { get; set; }
        public string plantscode { get; set; }
        public string mapimg { get; set; }
        public string itiscode { get; set; }
        public string awwetcode { get; set; }
        public string gpwetcode { get; set; }
        public string wmvcwetcode { get; set; }
        public string cvalue { get; set; }
        public string grank { get; set; }
        public string federalstatus { get; set; }
        public string cosrank { get; set; }
        public string mtsrank { get; set; }
        public string ndsrank { get; set; }
        public string sdsrank { get; set; }
        public string utsrank { get; set; }
        public string wysrank { get; set; }
        public string nativity { get; set; }
        public string noxiousweed { get; set; }
        public int elevminfeet { get; set; }
        public int elevminm { get; set; }
        public int elevmaxfeet { get; set; }
        public int elevmaxm { get; set; }
        public string keychar1 { get; set; }
        public string keychar2 { get; set; }
        public string keychar3 { get; set; }
        public string keychar4 { get; set; }
        public string keychar5 { get; set; }
        public string keychar6 { get; set; }
        public string similarsp { get; set; }
        public string habitat { get; set; }
        public string comments { get; set; }
        public string animaluse { get; set; }
        public string ecologicalsystems { get; set; }
        public string synonyms { get; set; }
        public string topimgtopimg { get; set; }
        public string duration { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<WetlandPlantImage> Images { get; set; }
        
        public string ThumbnailPath
        {
            get
            {
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                return rootFolder.Path + "/Images/" + plantscode + "_icon.jpg";
            }
        }

        public string scinameauthorstripped
        {
            get { return scinameauthor.Replace("<em>", "").Replace("</em>", ""); }
        }

        public string scinamenoauthorstripped
        {
            get { return scinamenoauthor.Replace("<em>", "").Replace("</em>", ""); }
        }

    }

}
