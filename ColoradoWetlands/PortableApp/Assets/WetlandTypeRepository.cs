using System;
using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using SQLite;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{

    public class WetlandTypeRepository
	{
        // establish SQLite connection
        private SQLiteConnection conn;
        private SQLiteAsyncConnection connAsync;
        public string StatusMessage { get; set; }

        public WetlandTypeRepository(string dbPath)
        {
            // Initialize a new SQLiteConnection
            conn = new SQLiteConnection(dbPath);
            connAsync = new SQLiteAsyncConnection(dbPath);

            // Create the Type table
            conn.CreateTable<WetlandType>();
            connAsync.CreateTableAsync<WetlandType>().Wait();
        }

        public async Task AddNewWetlandTypeAsync(string title)
        {
            int result = 0;
            try
            {
                // basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(title))
                    throw new Exception("Valid Title required");

                // insert a new plant type into the Wetland Type table
                result = await connAsync.InsertAsync(new WetlandType { Title = title });

                StatusMessage = string.Format("{0} record(s) added [Title: {1})", result, title);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", title, ex.Message);
            }

        }

        public List<WetlandType> GetAllWetlandTypes()
        {
            // return a list of plants saved to the Type table in the database
            return (from p in conn.Table<WetlandType>() select p).ToList();
        }

        public async Task<List<WetlandType>> GetAllWetlandTypesAsync()
        {
            // return a list of plants saved to the Plant table in the database
            return await connAsync.Table<WetlandType>().ToListAsync();
        }
    }
}