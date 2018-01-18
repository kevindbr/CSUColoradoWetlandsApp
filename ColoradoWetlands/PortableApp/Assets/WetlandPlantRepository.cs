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
                     tran.InsertOrReplaceWithChildren(plant);
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
                    tran.InsertOrReplaceAllWithChildren(plants);
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

        private List<WetlandPlant> GetSearchedPlants(List<WetlandSearch> selectCritList)
        {
            List<WetlandPlant> searchPlantsOther = GetAllWetlandPlants();
            List<WetlandPlant> searchPlantsColor = GetAllWetlandPlants();
            List<WetlandPlant> searchPlantsDivision = GetAllWetlandPlants();
            List<WetlandPlant> searchPlantsShape = GetAllWetlandPlants();
            List<WetlandPlant> searchPlantsArrangement = GetAllWetlandPlants();
            List<WetlandPlant> searchPlantsSize = GetAllWetlandPlants();
            List<WetlandPlant> searchCombined = new List<WetlandPlant>();
            List<WetlandPlant> allPlants = GetAllWetlandPlants();
            // Add selected Leaf Type characteristics

            var queryWetlandOther = selectCritList.Where(x => x.Characteristic.Contains("wetlandtype") || x.Characteristic.Contains("group") || x.Characteristic.Contains("nativity") || x.Characteristic.Contains("federal") || x.Characteristic.Contains("status"));
            if (queryWetlandOther.Count() > 0)
            {
                searchPlantsOther = App.WetlandPlantRepoLocal.GetAllWetlandPlants().AsQueryable().Where(ConstructPredicate(selectCritList)).ToList();

            }

            var queryColors = selectCritList.Where(x => x.Characteristic.Contains("color"));
            if (queryColors.Count() > 0)
            {
                searchPlantsColor = new List<WetlandPlant>();

                List<WetlandPlantFruits> fruits = App.WetlandPlantFruitsRepo.GetAllWetlandFruits().AsQueryable().Where(ConstructFruitPredicate(selectCritList)).ToList();
                foreach (var fruit in fruits)
                {
                    if (!searchPlantsColor.Contains(GetWetlandPlantByAltId(fruit.plantid)))
                    {
                        searchPlantsColor.Add(GetWetlandPlantByAltId(fruit.plantid));
                    }
                }
            }

            var queryLeafDivision = selectCritList.Where(x => x.Characteristic.Contains("leafdivision"));
            if (queryLeafDivision.Count() > 0)
            {
                searchPlantsDivision = new List<WetlandPlant>();

                List<WetlandPlantDivision> divisions = App.WetlandPlantDivisionRepoLocal.GetAllDivisions().AsQueryable().Where(ConstructDivisionPredicate(selectCritList)).ToList();
                foreach (var division in divisions)
                {
                    if (!searchPlantsDivision.Contains(GetWetlandPlantByAltId(division.plantid)))
                    {
                        searchPlantsDivision.Add(GetWetlandPlantByAltId(division.plantid));
                    }
                }

            }

            var queryShape = selectCritList.Where(x => x.Characteristic.Contains("leafshape"));
            if (queryShape.Count() > 0)
            {
                searchPlantsShape = new List<WetlandPlant>();

                List<WetlandPlantShape> leafs = App.WetlandPlantShapeRepoLocal.GetAllShapes().AsQueryable().Where(ConstructShapePredicate(selectCritList)).ToList();
                foreach (var leaf in leafs)
                {
                    if (!searchPlantsShape.Contains(GetWetlandPlantByAltId(leaf.plantid)))
                    {
                        searchPlantsShape.Add(GetWetlandPlantByAltId(leaf.plantid));
                    }
                }
            }


            var queryArrangement = selectCritList.Where(x => x.Characteristic.Contains("leafarrangement"));
            if (queryArrangement.Count() > 0)
            {
                searchPlantsArrangement = new List<WetlandPlant>();

                List<WetlandPlantArrangement> arrangements = App.WetlandPlantLeafArrangementRepoLocal.GetAllArrangements().AsQueryable().Where(ConstructArrangementPredicate(selectCritList)).ToList();
                foreach (var arrangement in arrangements)
                {
                    if (!searchPlantsArrangement.Contains(GetWetlandPlantByAltId(arrangement.plantid)))
                    {
                        searchPlantsArrangement.Add(GetWetlandPlantByAltId(arrangement.plantid));
                    }
                }
            }

            var querySize = selectCritList.Where(x => x.Characteristic.Contains("plantsize"));
            if (querySize.Count() > 0)
            {
                searchPlantsSize = new List<WetlandPlant>();

                List<WetlandPlantSize> sizes = App.WetlandPlantSizeRepoLocal.GetAllSizes().AsQueryable().Where(ConstructSizePredicate(selectCritList)).ToList();
                foreach (var size in sizes)
                {
                    if (!searchPlantsSize.Contains(GetWetlandPlantByAltId(size.plantid)))
                    {
                        searchPlantsSize.Add(GetWetlandPlantByAltId(size.plantid));
                    }
                }
            }


            foreach (var plant in allPlants)
                if (searchPlantsColor.Contains(plant) && searchPlantsDivision.Contains(plant) && searchPlantsShape.Contains(plant) && searchPlantsArrangement.Contains(plant) && searchPlantsSize.Contains(plant) && searchPlantsOther.Contains(plant))
                    searchCombined.Add(plant);

            return searchCombined;
        }

        private Expression<Func<WetlandPlantFruits, bool>> ConstructFruitPredicate(List<WetlandSearch> selectCritList)
        {
            var overallQuery = PredicateBuilder.True<WetlandPlantFruits>();

            // Add selected Leaf Type characteristics
            var queryColors = selectCritList.Where(x => x.Characteristic.Contains("color"));
            if (queryColors.Count() > 0)
            {
                var colorQuery = PredicateBuilder.False<WetlandPlantFruits>();
                foreach (var color in queryColors)
                {
                    colorQuery = colorQuery.Or(x => x.valueid == (color.SearchInt1));
                }
                overallQuery = overallQuery.And(colorQuery);
            }

            return overallQuery;
        }

        private Expression<Func<WetlandPlantDivision, bool>> ConstructDivisionPredicate(List<WetlandSearch> selectCritList)
        {
            var overallQuery = PredicateBuilder.True<WetlandPlantDivision>();

            // Add selected Leaf Type characteristics
            var queryColors = selectCritList.Where(x => x.Characteristic.Contains("leafdivision"));
            if (queryColors.Count() > 0)
            {
                var colorQuery = PredicateBuilder.False<WetlandPlantDivision>();
                foreach (var color in queryColors)
                {
                    colorQuery = colorQuery.Or(x => x.valueid == (color.SearchInt1));
                }
                overallQuery = overallQuery.And(colorQuery);
            }

            return overallQuery;
        }

        private Expression<Func<WetlandPlantShape, bool>> ConstructShapePredicate(List<WetlandSearch> selectCritList)
        {
            var overallQuery = PredicateBuilder.True<WetlandPlantShape>();

            // Add selected Leaf Type characteristics
            var queryColors = selectCritList.Where(x => x.Characteristic.Contains("leafshape"));
            if (queryColors.Count() > 0)
            {
                var colorQuery = PredicateBuilder.False<WetlandPlantShape>();
                foreach (var color in queryColors)
                {
                    colorQuery = colorQuery.Or(x => x.valueid == (color.SearchInt1));
                }
                overallQuery = overallQuery.And(colorQuery);
            }

            return overallQuery;
        }

        private Expression<Func<WetlandPlantArrangement, bool>> ConstructArrangementPredicate(List<WetlandSearch> selectCritList)
        {
            var overallQuery = PredicateBuilder.True<WetlandPlantArrangement>();

            // Add selected Leaf Type characteristics
            var queryColors = selectCritList.Where(x => x.Characteristic.Contains("leafarrangement"));
            if (queryColors.Count() > 0)
            {
                var colorQuery = PredicateBuilder.False<WetlandPlantArrangement>();
                foreach (var color in queryColors)
                {
                    colorQuery = colorQuery.Or(x => x.valueid == (color.SearchInt1));
                }
                overallQuery = overallQuery.And(colorQuery);
            }

            return overallQuery;
        }

        private Expression<Func<WetlandPlantSize, bool>> ConstructSizePredicate(List<WetlandSearch> selectCritList)
        {
            var overallQuery = PredicateBuilder.True<WetlandPlantSize>();

            // Add selected Leaf Type characteristics
            var queryColors = selectCritList.Where(x => x.Characteristic.Contains("plantsize"));
            if (queryColors.Count() > 0)
            {
                var colorQuery = PredicateBuilder.False<WetlandPlantSize>();
                foreach (var color in queryColors)
                {
                    colorQuery = colorQuery.Or(x => x.valueid == (color.SearchInt1));
                }
                overallQuery = overallQuery.And(colorQuery);
            }

            return overallQuery;
        }

        private Expression<Func<WetlandPlant, bool>> ConstructPredicate(List<WetlandSearch> selectCritList)
        {
            var overallQuery = PredicateBuilder.True<WetlandPlant>();
            // Add selected Flower Color characteristics
            var queryWetlandType = selectCritList.Where(x => x.Characteristic.Contains("wetlandtype"));
            if (queryWetlandType.Count() > 0)
            {
                var wetlandTypeQuery = PredicateBuilder.False<WetlandPlant>();
                foreach (var wetlandtype in queryWetlandType) { wetlandTypeQuery = wetlandTypeQuery.Or(x => x.ecologicalsystems.Contains(wetlandtype.SearchString1)); }
                overallQuery = overallQuery.And(wetlandTypeQuery);
            }

            var queryGroup = selectCritList.Where(x => x.Characteristic.Contains("group"));
            if (queryGroup.Count() > 0)
            {
                var groupQuery = PredicateBuilder.False<WetlandPlant>();
                foreach (var group in queryGroup) { groupQuery = groupQuery.Or(x => x.sections.Contains(group.SearchString1)); }
                overallQuery = overallQuery.And(groupQuery);
            }

            var queryNativity = selectCritList.Where(x => x.Characteristic.Contains("nativity"));
            if (queryNativity.Count() > 0)
            {
                var nativityQuery = PredicateBuilder.False<WetlandPlant>();
                foreach (var native in queryNativity) { nativityQuery = nativityQuery.Or(x => x.nativity.Contains(native.SearchString1)); }
                overallQuery = overallQuery.And(nativityQuery);
            }

            var queryFederal = selectCritList.Where(x => x.Characteristic.Contains("federal"));
            if (queryFederal.Count() > 0)
            {
                var federalQuery = PredicateBuilder.False<WetlandPlant>();
                foreach (var federal in queryFederal) { federalQuery = federalQuery.Or(x => x.federalstatus.Contains(federal.SearchString1)); }
                overallQuery = overallQuery.And(federalQuery);
            }

            var queryStatus = selectCritList.Where(x => x.Characteristic.Contains("status"));
            if (queryStatus.Count() > 0)
            {
                var statusQuery = PredicateBuilder.False<WetlandPlant>();
                foreach (var status in queryStatus) { statusQuery = statusQuery.Or(x => x.awwetcode.Contains(status.SearchString1) || x.gpwetcode.Contains(status.SearchString1) || x.wmvcwetcode.Contains(status.SearchString1)); }
                overallQuery = overallQuery.And(statusQuery);
            }

            return overallQuery;

        }
    }
}