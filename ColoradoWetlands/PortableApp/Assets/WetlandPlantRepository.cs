using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Threading.Tasks;
using System;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

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

        public async Task AddOrUpdatePlantAsync(WetlandPlant plant)
        {
            try
            {
                if (string.IsNullOrEmpty(plant.commonname))
                    throw new Exception("Valid plant required");

                await connAsync.RunInTransactionAsync((SQLite.Net.SQLiteConnection tran) =>
                {
                     connAsync.InsertOrReplaceWithChildrenAsync(plant);
                });
                //  await connAsync.InsertOrReplaceAllWithChildrenAsync(plant);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", plant, ex.Message);
            }
            
        }

        public async Task AddOrUpdateAllPlantsAsync(IList<WetlandPlant> plants)
        {
            try
            {
                // await connAsync.InsertOrReplaceWithChildrenAsync(plant);
                await connAsync.RunInTransactionAsync((SQLite.Net.SQLiteConnection tran) =>
                {
                    connAsync.InsertOrReplaceAllWithChildrenAsync(plants);
                });
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", "plants", ex.Message);
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

        public async Task<ObservableCollection<WetlandPlant>> FilterPlantsBySearchCriteria()
        {
            // get search criteria and plants
            List<WetlandSearch> selectCritList = await App.WetlandSearchRepo.GetQueryableSearchCriteriaAsync();
            List<WetlandPlant> plants;

            // execute filtering
            if (selectCritList.Count() > 0)
            {
                var predicate = ConstructPredicate(selectCritList);
                try
                {
                    plants = conn.Table<WetlandPlant>().AsQueryable().Where(predicate).ToList();
                }
                catch(NullReferenceException e)
                {
                    List<WetlandPlant> emptyPlants = new List<WetlandPlant>();
                    plants = emptyPlants;
                }

            }
            else
            {
                plants = GetAllWetlandPlants();
            }

            return new ObservableCollection<WetlandPlant>(plants);

        }

        // return a list of WetlandPlants saved to the WetlandPlant table in the database
        public async Task<ObservableCollection<WetlandPlant>> GetAllWetlandPlantsAsync()
        {
            List<WetlandPlant> list = await connAsync.GetAllWithChildrenAsync <WetlandPlant>();
            return new ObservableCollection<WetlandPlant>(list);
        }

        // Construct Predicate for plants query, filtering based on selected criteria
        // Solution taken from http://www.albahari.com/nutshell/predicatebuilder.aspx
        private Expression<Func<WetlandPlant, bool>> ConstructPredicate(List<WetlandSearch> selectCritList)
        {
            var overallQuery = PredicateBuilder.True<WetlandPlant>();

            // Add selected Leaf Type characteristics
            var queryColors = selectCritList.Where(x => x.Characteristic.Contains("color"));
            if (queryColors.Count() > 0)
            {
                var colorQuery = PredicateBuilder.False<WetlandPlant>();
                foreach (var color in queryColors) { colorQuery = colorQuery.Or(x => x.color == (color.SearchString1)); }
                overallQuery = overallQuery.And(colorQuery);
            }
            
            //Add selected Flower Color characteristics
            var queryLeafDivision = selectCritList.Where(x => x.Characteristic.Contains("leafdivision"));
            if (queryLeafDivision.Count() > 0)
            {
                var leafdivisionQuery = PredicateBuilder.False<WetlandPlant>();
                foreach (var leafdivision in queryLeafDivision) { leafdivisionQuery = leafdivisionQuery.Or(x => x.leafdivision == (leafdivision.SearchString1)); }
                overallQuery = overallQuery.And(leafdivisionQuery);
            }

            // Add selected Flower Color characteristics
            var queryLeafShape = selectCritList.Where(x => x.Characteristic.Contains("leafshape"));
            if (queryLeafShape.Count() > 0)
            {
                var leafShapeQuery = PredicateBuilder.False<WetlandPlant>();
                foreach (var leafshape in queryLeafShape) { leafShapeQuery = leafShapeQuery.Or(x => x.leafshape ==  (leafshape.SearchString1)); }
                overallQuery = overallQuery.And(leafShapeQuery);
            }

            // Add selected Flower Color characteristics
            var queryLeafArrangement = selectCritList.Where(x => x.Characteristic.Contains("leafarrangement"));
            if (queryLeafArrangement.Count() > 0)
            {
                var leafArrangementeQuery = PredicateBuilder.False<WetlandPlant>();
                foreach (var leafarrangement in queryLeafArrangement) { leafArrangementeQuery = leafArrangementeQuery.Or(x => x.leafarrangement == (leafarrangement.SearchString1)); }
                overallQuery = overallQuery.And(leafArrangementeQuery);
            }

            // Add selected Flower Color characteristics
            var queryPlantSize = selectCritList.Where(x => x.Characteristic.Contains("plantsize"));
            if (queryPlantSize.Count() > 0)
            {
                var plantSizeQuery = PredicateBuilder.False<WetlandPlant>();
                foreach (var plantsize in queryPlantSize) { plantSizeQuery = plantSizeQuery.Or(x => x.plantsize == (plantsize.SearchString1)); }
                overallQuery = overallQuery.And(plantSizeQuery);
            }
            
            //if(overallQuery)

            return overallQuery;
        }

    }
}