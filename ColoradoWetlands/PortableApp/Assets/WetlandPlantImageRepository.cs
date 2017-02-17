using System;
using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using SQLite;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{

    public class WetlandPlantImageRepository
    {
        // establish SQLite connection
        private SQLiteConnection conn;
        private SQLiteAsyncConnection connAsync;
        public string StatusMessage { get; set; }

        public WetlandPlantImageRepository(string dbPath)
        {
            // Initialize a new SQLiteConnection
            conn = new SQLiteConnection(dbPath);
            connAsync = new SQLiteAsyncConnection(dbPath);

            // Create the Wetland Plant table
            conn.CreateTable<WetlandPlantImage>();
            connAsync.CreateTableAsync<WetlandPlantImage>().Wait();
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