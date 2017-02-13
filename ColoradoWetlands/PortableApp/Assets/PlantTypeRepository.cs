using System;
using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using SQLite;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{

    public class PlantTypeRepository
	{
        // establish SQLite connection
        private SQLiteConnection conn;
        private SQLiteAsyncConnection connAsync;
        public string StatusMessage { get; set; }

        public PlantTypeRepository(string dbPath)
        {
            // Initialize a new SQLiteConnection
            conn = new SQLiteConnection(dbPath);
            connAsync = new SQLiteAsyncConnection(dbPath);

            // Create the Plant table
            conn.CreateTable<PlantType>();
            connAsync.CreateTableAsync<PlantType>().Wait();
        }

        public async Task AddNewPlantTypeAsync(string title)
        {
            int result = 0;
            try
            {
                // basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(title))
                    throw new Exception("Valid Title required");

                // insert a new plant type into the Plant Type table
                result = await connAsync.InsertAsync(new PlantType { Title = title });

                StatusMessage = string.Format("{0} record(s) added [Title: {1})", result, title);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", title, ex.Message);
            }

        }

        public IEnumerable<PlantType> GetAllPlantTypes()
        {
            // return a list of plants saved to the Plant table in the database
            return (from p in conn.Table<PlantType>() select p).ToList();
        }

        public async Task<List<PlantType>> GetAllPlantTypesAsync()
        {
            // return a list of plants saved to the Plant table in the database
            return await connAsync.Table<PlantType>().ToListAsync();
        }
    }
}