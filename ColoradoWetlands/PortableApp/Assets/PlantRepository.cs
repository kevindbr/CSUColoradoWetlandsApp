using System;
using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using SQLite;

namespace PortableApp
{

    public class PlantRepository
	{
        // establish SQLite connection
        private SQLiteConnection conn;
        public string StatusMessage { get; set; }

		public PlantRepository(string dbPath)
		{
            // Initialize a new SQLiteConnection
            conn = new SQLiteConnection(dbPath);
            
            // Create the Plant table
            conn.CreateTable<Plant>();
		}

		public void AddNewPlant(string commonName)
		{
			int result = 0;
			try
			{
				// basic validation to ensure a name was entered
				if (string.IsNullOrEmpty(commonName))
					throw new Exception("Valid Common Name required");

                // insert a new plant into the Plant table
                result = conn.Insert(new Plant { CommonName = commonName });

				StatusMessage = string.Format("{0} record(s) added [CommonName: {1})", result, commonName);
			}
			catch (Exception ex)
			{
				StatusMessage = string.Format("Failed to add {0}. Error: {1}", commonName, ex.Message);
			}

		}

		public List<Plant> GetAllPlants()
		{
			// return a list of plants saved to the Plant table in the database
            return conn.Table<Plant>().ToList();
		}
	}
}