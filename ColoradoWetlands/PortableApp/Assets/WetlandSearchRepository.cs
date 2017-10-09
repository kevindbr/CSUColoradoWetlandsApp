using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;

namespace PortableApp
{

    public class WetlandSearchRepository : DBConnection
	{

        public string StatusMessage { get; set; }

        public WetlandSearchRepository()
		{

            //conn.DropTable<WetlandSearch>();

            // Create the Wetland Search table (only if it's not yet created) and seed it if needed
            conn.CreateTable<WetlandSearch>();
            if (GetAllWetlandSearchCriteria().Count == 0) { SeedDB(); };
		}

        // return a list of all Wetland Search Criteria
        public List<WetlandSearch> GetAllWetlandSearchCriteria()
        {
            return conn.Table<WetlandSearch>().ToList();
        }

        // return a list of only selected (for querying) search criteria
        public List<WetlandSearch> GetQueryableSearchCriteria()
        {
            return conn.Table<WetlandSearch>().Where(x => x.Query == true).ToList();
        }

        // get an individual search criteria based on its name
        public async Task<WetlandSearch> GetSearchAsync(string name)
        {
            return await connAsync.Table<WetlandSearch>().Where(s => s.Name.Equals(name)).FirstOrDefaultAsync();
        }

        // update a search criteria
        public async Task UpdateSearchCriteriaAsync(WetlandSearch criteria)
        {
            try
            {
                var result = await connAsync.UpdateAsync(criteria);
                StatusMessage = string.Format("{0} record(s) updated [Name: {1})", result, criteria);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", criteria, ex.Message);
            }            
        }

        public async Task<List<WetlandSearch>> GetQueryableSearchCriteriaAsync()
        {
            return await connAsync.Table<WetlandSearch>().Where(x => x.Query == true).ToListAsync();
        }

        // Seed database with Search Criteria
        public void SeedDB()
        {
            conn.Insert(new WetlandSearch() { Characteristic = "color-Yellow", Name = "Yellow", Query = false, Column1 = "color", SearchString1 = "yellow" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Blue", Name = "Blue", Query = false, Column1 = "color", SearchString1 = "blue" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Red", Name = "Red", Query = false, Column1 = "color", SearchString1 = "red" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Green", Name = "Green", Query = false, Column1 = "color", SearchString1 = "green" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Orange", Name = "Orange", Query = false, Column1 = "color", SearchString1 = "orange" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Pink", Name = "Pink", Query = false, Column1 = "color", SearchString1 = "pink" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Purple", Name = "Purple", Query = false, Column1 = "color", SearchString1 = "purple" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Brown", Name = "Brown", Query = false, Column1 = "color", SearchString1 = "brown" });

        }
    }
}