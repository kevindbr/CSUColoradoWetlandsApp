using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Threading.Tasks;
using System;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;

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

        // return a list of Wetlandplants saved to the WetlandPlant table in the database
        public List<WetlandPlant> GetAllWetlandPlants()
        {
            return conn.GetAllWithChildren<WetlandPlant>();
        }

        public List<string> GetPlantJumpList()
        {
            return GetAllWetlandPlants().Select(x => x.scinameauthorstripped[0].ToString()).Distinct().ToList();
        }

        // return a specific WetlandPlant given an id
        public WetlandPlant GetWetlandPlantByAltId(int Id)
        {
            WetlandPlant plant = conn.Table<WetlandPlant>().Where(p => p.id.Equals(Id)).FirstOrDefault();
            return conn.GetWithChildren<WetlandPlant>(plant.plantid);
        }

        // get plants marked as favorites
        public List<WetlandPlant> GetFavoritePlants()
        {
            return GetAllWetlandPlants().Where(p => p.isFavorite == true).ToList();
        }

        // get plants through term supplied in quick search
        public List<WetlandPlant> WetlandPlantsQuickSearch(string searchTerm)
        {
            return GetAllWetlandPlants().Where(p => p.scinameauthorstripped.ToLower().Contains(searchTerm.ToLower()) || p.commonname.ToLower().Contains(searchTerm.ToLower())).ToList();
        }

        // get current search criteria (saved in db) and return appropriate list of Wetland Plants
        public IEnumerable<WetlandPlant> GetPlantsBySearchCriteria()
        {
            IEnumerable<WetlandPlant> plants = GetAllWetlandPlants();
            List<WetlandSearch> searchCriteria = new List<WetlandSearch>(App.WetlandSearchRepo.GetQueryableSearchCriteria());
            if (searchCriteria.Count > 0)
            {
                foreach (WetlandSearch criterion in searchCriteria)
                {
                    try
                    {
                        if (criterion.Name == "Bentgrass") { plants = plants.Where(x => x.commonname.Contains(criterion.SearchString1)); };
                        if (criterion.Name == "Poaceae") { plants = plants.Where(x => x.family.Contains(criterion.SearchString1)); };
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return plants;
        }

        public async Task AddPlantAsync(WetlandPlant plant)
        {
            try
            {
                if (string.IsNullOrEmpty(plant.commonname))
                    throw new Exception("Valid plant required");
                await connAsync.InsertWithChildrenAsync(plant);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", plant, ex.Message);
            }
            
        }

        public async Task UpdatePlantAsync(WetlandPlant plant)
        {
            try
            {
                await connAsync.UpdateAsync(plant);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to update {0}. Error: {1}", plant, ex.Message);
            }
        }

    }
}