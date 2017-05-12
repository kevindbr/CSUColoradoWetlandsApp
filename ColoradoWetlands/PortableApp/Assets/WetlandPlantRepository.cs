using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;

namespace PortableApp
{

    public class WetlandPlantRepository : DBConnection
	{

        public string StatusMessage { get; set; }

        public WetlandPlantRepository()
		{
            // Create the Wetland Plant table (only if it's not yet created)
            //conn.DropTable<WetlandPlant>();
            conn.CreateTable<WetlandPlant>();
		}
        
        public List<WetlandPlant> GetAllWetlandPlants()
        {
            // return a list of Wetlandplants saved to the WetlandPlant table in the database
            return (from p in conn.Table<WetlandPlant>() select p).ToList();
        }

        public async Task AddPlantAsync(WetlandPlant plant)
        {
            try
            {
                if (string.IsNullOrEmpty(plant.commonname))
                    throw new Exception("Valid plant required");

                var result = await connAsync.InsertAsync(plant);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, plant);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", plant, ex.Message);
            }
            
        }

    }
}