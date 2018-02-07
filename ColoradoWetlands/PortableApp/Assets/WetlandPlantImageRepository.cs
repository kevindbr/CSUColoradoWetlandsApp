using System;
using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using SQLite;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{

    public class WetlandPlantImageRepository : DBConnection
    {

        public WetlandPlantImageRepository()
        {
            // Create the Wetland Plant table
            //conn.DropTable<WetlandPlantImage>();
            conn.CreateTable<WetlandPlantImage>();
        }

        public void ClearWetlandImages()
        {
            conn.DropTable<WetlandPlantImage>();
            conn.CreateTable<WetlandPlantImage>();
        }

        // return a list of Wetland Plant Images saved to the WetlandPlantImage table in the database
        public List<WetlandPlantImage> GetAllWetlandPlantImages()
        {
            return (from p in conn.Table<WetlandPlantImage>() select p).ToList();
        }

        // return a list of Wetland Plant Images for the plant specified
        public List<WetlandPlantImage> PlantImages(int plantId)
        {
            return conn.Table<WetlandPlantImage>().Where(p => p.PlantId.Equals(plantId)).ToList();
        }

    }
}