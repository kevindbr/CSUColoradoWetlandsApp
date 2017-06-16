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

        // Seed database with Search Criteria
        public void SeedDB()
        {
            conn.Insert(new WetlandSearch() { Name = "Poaceae", Query = false, Column1 = "family", SearchString1 = "Poaceae" });
            conn.Insert(new WetlandSearch() { Name = "Bentgrass", Query = false, Column1 = "commonname", SearchString1 = "Bentgrass" });
        }
    }
}