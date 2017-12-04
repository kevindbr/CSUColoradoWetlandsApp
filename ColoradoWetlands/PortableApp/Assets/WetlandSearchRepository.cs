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
            conn.Insert(new WetlandSearch() { Characteristic = "color-Yellow", Name = "Yellow", Query = false, Column1 = "valueid", SearchInt1 = 7 });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Blue", Name = "Blue", Query = false, Column1 = "valueid", SearchInt1 = 6 });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Red", Name = "Red", Query = false, Column1 = "valueid", SearchInt1 = 4 });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Green", Name = "Green", Query = false, Column1 = "valueid", SearchInt1 = 9 });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Orange", Name = "Orange", Query = false, Column1 = "valueid", SearchInt1 = 2 });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Pink", Name = "Pink", Query = false, Column1 = "valueid", SearchInt1 = 3 });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Purple", Name = "Purple", Query = false, Column1 = "valueid", SearchInt1 = 5 });
            conn.Insert(new WetlandSearch() { Characteristic = "color-Brown", Name = "Brown", Query = false, Column1 = "valueid", SearchInt1 = 8 });

            conn.Insert(new WetlandSearch() { Characteristic = "leafdivision-Simple", Name = "Simple", Query = false, Column1 = "leafdivision", SearchString1 = "simple", SearchInt1 = 11 });
            conn.Insert(new WetlandSearch() { Characteristic = "leafdivision-Compound", Name = "Compound", Query = false, Column1 = "leafdivision", SearchString1 = "compound" , SearchInt1 = 12 });

            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-Linear", Name = "Linear", Query = false, Column1 = "leafshape", SearchString1 = "linear", IconFileName = "linear.png", SearchInt1 = 13 });
            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-Round", Name = "Round", Query = false, Column1 = "leafshape", SearchString1 = "round", IconFileName = "round.png", SearchInt1 = 14 });
            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-WideBase", Name = "Wide Base", Query = false, Column1 = "leafshape", SearchString1 = "widebase", IconFileName = "wide_base1.png", SearchInt1 = 15 });
            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-WideTip", Name = "Wide Tip", Query = false, Column1 = "leafshape", SearchString1 = "widetip", IconFileName = "wide_tip1.png", SearchInt1 = 16 });
            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-Lobed", Name = "Lobed", Query = false, Column1 = "leafshape", SearchString1 = "lobed", IconFileName = "lobed.png", SearchInt1 = 17 });
            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-Palmate", Name = "Palmate", Query = false, Column1 = "leafshape", SearchString1 = "palmate", IconFileName = "palmate.png", SearchInt1 = 18 });
            conn.Insert(new WetlandSearch() { Characteristic = "leafshape-Pinnate", Name = "Pinnate", Query = false, Column1 = "leafshape", SearchString1 = "pinnate", IconFileName = "pinnate.png", SearchInt1 = 19 });

            conn.Insert(new WetlandSearch() { Characteristic = "leafarrangement-Alternate", Name = "Alternate", Query = false, Column1 = "leafarrangement", SearchString1 = "alternate", IconFileName = "alternate.png", SearchInt1 = 21 });
            conn.Insert(new WetlandSearch() { Characteristic = "leafarrangement-Opposite", Name = "Opposite", Query = false, Column1 = "leafarrangement", SearchString1 = "opposite", IconFileName = "opposite.png", SearchInt1 = 22 });
            conn.Insert(new WetlandSearch() { Characteristic = "leafarrangement-Whorled", Name = "Whorled", Query = false, Column1 = "leafarrangement", SearchString1 = "whorled", IconFileName = "whorled.png", SearchInt1 = 23 });
            conn.Insert(new WetlandSearch() { Characteristic = "leafarrangement-Basal", Name = "Basal", Query = false, Column1 = "leafarrangement", SearchString1 = "basal", IconFileName = "basal.png", SearchInt1 = 24 });

            conn.Insert(new WetlandSearch() { Characteristic = "plantsize-VerySmall", Name = "<5cm", Query = false, Column1 = "plantsize", SearchString1 = "verysmall", SearchInt1 = 25 });
            conn.Insert(new WetlandSearch() { Characteristic = "plantsize-Small", Name = "5cm-20cm", Query = false, Column1 = "plantsize", SearchString1 = "small", SearchInt1 = 26 });
            conn.Insert(new WetlandSearch() { Characteristic = "plantsize-Medium", Name = "20cm-50cm", Query = false, Column1 = "plantsize", SearchString1 = "medium", SearchInt1 = 27 });
            conn.Insert(new WetlandSearch() { Characteristic = "plantsize-Large", Name = ">50cm", Query = false, Column1 = "plantsize", SearchString1 = "large", SearchInt1 = 28 });
            
        }
    }
}