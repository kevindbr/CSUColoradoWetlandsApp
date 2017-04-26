using System;
using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using SQLite;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{

    public class WetlandPlantRepository
	{
        // establish SQLite connection
        private SQLiteConnection conn;
        private SQLiteAsyncConnection connAsync;
        public string StatusMessage { get; set; }

		public WetlandPlantRepository(string dbPath)
		{
            // Initialize a new SQLiteConnection
            conn = new SQLiteConnection(dbPath);
            connAsync = new SQLiteAsyncConnection(dbPath);

            // Create the Wetland Plant table
            conn.CreateTable<WetlandPlant>();
            connAsync.CreateTableAsync<WetlandPlant>().Wait();
		}

		public async Task AddNewWetlandPlantAsync(string commonName)
		{
			int result = 0;
			try
			{
				// basic validation to ensure a name was entered
				if (string.IsNullOrEmpty(commonName))
					throw new Exception("Valid Common Name required");

                // insert a new plant into the Plant table
                result = await connAsync.InsertAsync(new WetlandPlant { commonname = commonName });

				StatusMessage = string.Format("{0} record(s) added [CommonName: {1})", result, commonName);
			}
			catch (Exception ex)
			{
				StatusMessage = string.Format("Failed to add {0}. Error: {1}", commonName, ex.Message);
			}

		}

        public List<WetlandPlant> GetAllWetlandPlants()
        {
            // return a list of Wetlandplants saved to the WetlandPlant table in the database
            return (from p in conn.Table<WetlandPlant>() select p).ToList();
        }

        public async Task<List<WetlandPlant>> GetAllWetlandPlantsAsync()
        {
            // return a list of plants saved to the Wetland Plant table in the database
            return await connAsync.Table<WetlandPlant>().ToListAsync();
        }
    }
}