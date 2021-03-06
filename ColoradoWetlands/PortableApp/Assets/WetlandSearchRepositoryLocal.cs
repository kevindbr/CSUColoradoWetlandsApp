﻿using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;

namespace PortableApp
{

    public class WetlandSearchRepositoryLocal : DBConnection
	{

        public string StatusMessage { get; set; }

        public WetlandSearchRepositoryLocal()
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

            conn.Insert(new WetlandSearch() { Characteristic = "region-Plains", Name = "Eastern Plains", Query = false, Column1 = "RegionWetland", SearchString1 = "plains", SearchInt1 = 187 });
            conn.Insert(new WetlandSearch() { Characteristic = "region-Mountains", Name = "Rocky Mountains", Query = false, Column1 = "RegionWetland", SearchString1 = "mountains", SearchInt1 = 188 });
            conn.Insert(new WetlandSearch() { Characteristic = "region-Plateau", Name = "Colorado Plateau", Query = false, Column1 = "RegionWetland", SearchString1 = "plateau", SearchInt1 = 189 });

            conn.Insert(new WetlandSearch() { Characteristic = "region-EasternPlains", Name = "Eastern Plains", Query = false, Column1 = "RegionWetland", SearchString1 = "plains", SearchInt1 = 0 });
            conn.Insert(new WetlandSearch() { Characteristic = "region-Mountains", Name = "Rocky Mountains", Query = false, Column1 = "RegionWetland", SearchString1 = "mountains", SearchInt1 = 0 });
            conn.Insert(new WetlandSearch() { Characteristic = "region-Plateau", Name = "Colorado Plateau", Query = false, Column1 = "RegionWetland", SearchString1 = "plateau", SearchInt1 = 0 });

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

            conn.Insert(new WetlandSearch() { Characteristic = "wetlandtype-Marsh", Name = "Marsh", Query = false, Column1 = "ecologicalsystems", SearchString1 = "Marsh" });
            conn.Insert(new WetlandSearch() { Characteristic = "wetlandtype-WetMeadow", Name = "Wet Meadow", Query = false, Column1 = "ecologicalsystems", SearchString1 = "Wet Meadow" });
            conn.Insert(new WetlandSearch() { Characteristic = "wetlandtype-MesicMeadow", Name = "Mesic Meadow", Query = false, Column1 = "ecologicalsystems", SearchString1 = "Mesic Meadow" });
            conn.Insert(new WetlandSearch() { Characteristic = "wetlandtype-Fen", Name = "Fen", Query = false, Column1 = "ecologicalsystems", SearchString1 = "Fen" });
            conn.Insert(new WetlandSearch() { Characteristic = "wetlandtype-Playa", Name = "Playa", Query = false, Column1 = "ecologicalsystems", SearchString1 = "Playa" });
            conn.Insert(new WetlandSearch() { Characteristic = "wetlandtype-SubalpineRiparianWoodland", Name = "Subalpine Riparian Woodland", Query = false, Column1 = "ecologicalsystems", SearchString1 = "Subalpine Riparian Woodland" });
            conn.Insert(new WetlandSearch() { Characteristic = "wetlandtype-SubalpineRiparianShrubland", Name = "Subalpine Riparian Shrubland", Query = false, Column1 = "ecologicalsystems", SearchString1 = "Subalpine Riparian Shrubland" });
            conn.Insert(new WetlandSearch() { Characteristic = "wetlandtype-FoothillsRiparian", Name = "Foothills Riparian", Query = false, Column1 = "ecologicalsystems", SearchString1 = "Foothills Riparian" });
            conn.Insert(new WetlandSearch() { Characteristic = "wetlandtype-PlainsRiparian", Name = "Plains Riparian", Query = false, Column1 = "ecologicalsystems", SearchString1 = "Plains Riparian" });
            conn.Insert(new WetlandSearch() { Characteristic = "wetlandtype-PlainsFloodplain", Name = "Plains Floodplain", Query = false, Column1 = "ecologicalsystems", SearchString1 = "Plains Floodplain" });
            conn.Insert(new WetlandSearch() { Characteristic = "wetlandtype-GreasewoodFlats", Name = "Greasewood Flats", Query = false, Column1 = "ecologicalsystems", SearchString1 = "Greasewood Flats" });
            conn.Insert(new WetlandSearch() { Characteristic = "wetlandtype-HangingGarden", Name = "Hanging Garden", Query = false, Column1 = "ecologicalsystems", SearchString1 = "Hanging Garden" });

            conn.Insert(new WetlandSearch() { Characteristic = "group-Woody", Name = "Woody", Query = false, Column1 = "sections", SearchString1 = "Woody", IconFileName = "shrubtree.png" });
            conn.Insert(new WetlandSearch() { Characteristic = "group-Dicot", Name = "Dicot Herbs", Query = false, Column1 = "sections", SearchString1 = "Dicot", IconFileName = "dicot.png" });
            conn.Insert(new WetlandSearch() { Characteristic = "group-Monocot", Name = "Monocot Herbs", Query = false, Column1 = "sections", SearchString1 = "Monocot", IconFileName = "monocot.png" });
            conn.Insert(new WetlandSearch() { Characteristic = "group-Aquatic", Name = "Aquatic Herbs", Query = false, Column1 = "sections", SearchString1 = "Aquatic", IconFileName = "aquatic.png" });
            conn.Insert(new WetlandSearch() { Characteristic = "group-Rushes", Name = "Rushes", Query = false, Column1 = "sections", SearchString1 = "Rushes", IconFileName = "rushes.png" });
            conn.Insert(new WetlandSearch() { Characteristic = "group-Sedges", Name = "Sedges", Query = false, Column1 = "sections", SearchString1 = "Sedges", IconFileName = "sedge.png" });
            conn.Insert(new WetlandSearch() { Characteristic = "group-Grasses", Name = "Grasses", Query = false, Column1 = "sections", SearchString1 = "Grasses", IconFileName = "grass.png" });
            conn.Insert(new WetlandSearch() { Characteristic = "group-Ferns", Name = "Ferns", Query = false, Column1 = "sections", SearchString1 = "Ferns", IconFileName = "fern.png" });

            conn.Insert(new WetlandSearch() { Characteristic = "nativity-Native", Name = "Native", Query = false, Column1 = "nativity", SearchString1 = "Native" });
            conn.Insert(new WetlandSearch() { Characteristic = "nativity-Non", Name = "Non-native", Query = false, Column1 = "nativity", SearchString1 = "Non-native" });
            conn.Insert(new WetlandSearch() { Characteristic = "noxiousweed-WatchList", Name = "CO Noxious Weed Watch List", Query = false, Column1 = "noxiousweed", SearchString1 = "CO Noxious Weed Watch List" });
            conn.Insert(new WetlandSearch() { Characteristic = "noxiousweed-ListA", Name = "CO Noxious Weed List A", Query = false, Column1 = "noxiousweed", SearchString1 = "CO Noxious Weed List A" });
            conn.Insert(new WetlandSearch() { Characteristic = "noxiousweed-ListB", Name = "CO Noxious Weed List B", Query = false, Column1 = "noxiousweed", SearchString1 = "CO Noxious Weed List B" });
            conn.Insert(new WetlandSearch() { Characteristic = "noxiousweed-ListC", Name = "CO Noxious Weed List C", Query = false, Column1 = "noxiousweed", SearchString1 = "CO Noxious Weed List C" });

            conn.Insert(new WetlandSearch() { Characteristic = "federal-USFS", Name = "USFS Sensitive", Query = false, Column1 = "federalstatus", SearchString1 = "USFS" });
            conn.Insert(new WetlandSearch() { Characteristic = "federal-Threatened", Name = "Listed Threatened", Query = false, Column1 = "federalstatus", SearchString1 = "Threatened" });
            conn.Insert(new WetlandSearch() { Characteristic = "federal-BLM", Name = "BLM Sensitive", Query = false, Column1 = "federalstatus", SearchString1 = "BLM" });

            conn.Insert(new WetlandSearch() { Characteristic = "status-FAC", Name = "FAC", Query = false, Column1 = "awwetcode", Column2 = "gpwetcode", Column3 = "wmvcwetcode", SearchString1 = "FAC" });
            conn.Insert(new WetlandSearch() { Characteristic = "status-FACW", Name = "FACW", Query = false, Column1 = "awwetcode", Column2 = "gpwetcode", Column3 = "wmvcwetcode", SearchString1 = "FACW" });
            conn.Insert(new WetlandSearch() { Characteristic = "status-FACU", Name = "FACU", Query = false, Column1 = "awwetcode", Column2 = "gpwetcode", Column3 = "wmvcwetcode", SearchString1 = "FACU" });
            conn.Insert(new WetlandSearch() { Characteristic = "status-OBL", Name = "OBL", Query = false, Column1 = "awwetcode", Column2 = "gpwetcode", Column3 = "wmvcwetcode", SearchString1 = "OBL" });
            conn.Insert(new WetlandSearch() { Characteristic = "status-NI", Name = "NI", Query = false, Column1 = "awwetcode", Column2 = "gpwetcode", Column3 = "wmvcwetcode", SearchString1 = "NI" });
            conn.Insert(new WetlandSearch() { Characteristic = "status-UPL", Name = "UPL", Query = false, Column1 = "awwetcode", Column2 = "gpwetcode", Column3 = "wmvcwetcode", SearchString1 = "UPL" });
        }
    }
}