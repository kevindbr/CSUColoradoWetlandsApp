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

            conn.DropTable<WetlandSearch>();

            // Create the Wetland Search table (only if it's not yet created) and seed it if needed
            conn.CreateTable<WetlandSearch>();
            SeedDB();
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

        public async Task<List<WetlandSearch>> GetQueryableSearchCriteriaAsync()
        {
            return await connAsync.Table<WetlandSearch>().Where(x => x.Query == true).ToListAsync();
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
            conn.Insert(new WetlandSearch() { Characteristic = "color-Yellow", Name = "Yellow", Query = false, Column1 = "color", SearchString1 = "yellow" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Blue", Name = "Blue", Query = false, Column1 = "color", SearchString1 = "blue" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Red", Name = "Red", Query = false, Column1 = "color", SearchString1 = "red" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Green", Name = "Green", Query = false, Column1 = "color", SearchString1 = "green" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Orange", Name = "Orange", Query = false, Column1 = "color", SearchString1 = "orange" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Pink", Name = "Pink", Query = false, Column1 = "color", SearchString1 = "pink" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Purple", Name = "Purple", Query = false, Column1 = "color", SearchString1 = "purple" });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Brown", Name = "Brown", Query = false, Column1 = "color", SearchString1 = "brown" });

            conn.Insert(new WetlandSearch() { Characteristic = "leafdivision-Simple", Name = "Simple", Query = false, Column1 = "leafdivision", SearchString1 = "simple" });
            conn.Insert(new WetlandSearch() { Characteristic = "leafdivision-Compound", Name = "Compound", Query = false, Column1 = "leafdivision", SearchString1 = "compound" });

            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-Linear", Name = "Linear", Query = false, Column1 = "leafshape", SearchString1 = "linear" });
            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-Round", Name = "Round", Query = false, Column1 = "leafshape", SearchString1 = "round" });
            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-WideBase", Name = "Wide Base", Query = false, Column1 = "leafshape", SearchString1 = "widebase" });
            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-WideTip", Name = "Wide Tip", Query = false, Column1 = "leafshape", SearchString1 = "widetip" });
            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-Lobed", Name = "Lobed", Query = false, Column1 = "leafshape", SearchString1 = "lobed" });
            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-Palmate", Name = "Palmate", Query = false, Column1 = "leafshape", SearchString1 = "palmate" });
            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-Pinnate", Name = "Pinnate", Query = false, Column1 = "leafshape", SearchString1 = "pinnate" });

            conn.Insert(new WetlandSearch() { Characteristic = "leafarrangement-Alternate", Name = "Alternate", Query = false, Column1 = "leafarrangement", SearchString1 = "alternate" });
            conn.Insert(new WetlandSearch() { Characteristic = "leafarrangement-Opposite", Name = "Opposite", Query = false, Column1 = "leafarrangement", SearchString1 = "opposite" });
            conn.Insert(new WetlandSearch() { Characteristic = "leafarrangement-Whorled", Name = "Whorled", Query = false, Column1 = "leafarrangement", SearchString1 = "whorled" });
            conn.Insert(new WetlandSearch() { Characteristic = "leafarrangement-Basal", Name = "Basal", Query = false, Column1 = "leafarrangement", SearchString1 = "basal" });

            conn.Insert(new WetlandSearch() { Characteristic = "plantsize-VerySmall", Name = "Very Small", Query = false, Column1 = "plantsize", SearchString1 = "verysmall" });
            conn.Insert(new WetlandSearch() { Characteristic = "plantsize-Small", Name = "Small", Query = false, Column1 = "plantsize", SearchString1 = "small" });
            conn.Insert(new WetlandSearch() { Characteristic = "plantsize-Medium", Name = "Medium", Query = false, Column1 = "plantsize", SearchString1 = "medium" });
            conn.Insert(new WetlandSearch() { Characteristic = "plantsize-Large", Name = "Large", Query = false, Column1 = "plantsize", SearchString1 = "large" });

        }
    }
}