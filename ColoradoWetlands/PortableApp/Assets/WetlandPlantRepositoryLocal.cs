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

    public class WetlandPlantRepositoryLocal
    {

        public string StatusMessage { get; set; }
        private List<WetlandPlant> allWetlandPlants;
        private List<WetlandPlant> searchPlants;

        public WetlandPlantRepositoryLocal(List<WetlandPlant> allPlantsDB)
        {
            allWetlandPlants = allPlantsDB;
        }

        public void ClearWetlandPlantsLocal()
        {
            allWetlandPlants = new List<WetlandPlant>();
            searchPlants = new List<WetlandPlant>();
        }

        // return a list of Wetlandplants saved to the WetlandPlant table in the database
        public List<WetlandPlant> GetAllWetlandPlants()
        {
            return allWetlandPlants;                       
        }

        // return a list of Wetlandplants saved to the WetlandPlant table in the database
        public async Task<ObservableCollection<WetlandPlant>> GetAllSearchPlants()
        {
            return new ObservableCollection<WetlandPlant>(searchPlants);
        }

        public void setSearchPlants(List<WetlandPlant> searchPlants)
        {
            this.searchPlants = searchPlants;
        }

        public List<string> GetPlantJumpList()
        {
            return allWetlandPlants.Select(x => x.scinameauthorstripped[0].ToString()).Distinct().ToList();
        }

        // return a specific WetlandPlant given an id
        public WetlandPlant GetWetlandPlantByAltId(int Id)
        {
            IEnumerable<WetlandPlant> plants = allWetlandPlants.Where(p => p.plantid.Equals(Id));

            // if (plants != null)
            try
            {
                return plants.First();
            }
            catch(InvalidOperationException)
            {
                 return null;
            }     
        }

        // get plants marked as favorites
        public List<WetlandPlant> GetFavoritePlants()
        {
            return allWetlandPlants.Where(p => p.isFavorite == true).ToList();
        }

        // get plants through term supplied in quick search
        public List<WetlandPlant> WetlandPlantsQuickSearch(string searchTerm)
        {
            return allWetlandPlants.Where(p => p.scinameauthorstripped.ToLower().Contains(searchTerm.ToLower()) || p.commonname.ToLower().Contains(searchTerm.ToLower())).ToList();
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


        public async Task<ObservableCollection<WetlandPlant>> FilterPlantsBySearchCriteria()
        {
            // get search criteria and plants
            List<WetlandSearch> selectCritList = await App.WetlandSearchRepo.GetQueryableSearchCriteriaAsync();
            List<WetlandPlant> plants = GetAllWetlandPlants();
            // List<WetlandPlantFruits> fruits = App.WetlandPlantFruitsRepo.GetAllWetlandFruits();
            // execute filtering
            if (selectCritList.Count() > 0)
            {
                try                  
                {
                        plants = GetSearchedPlants(selectCritList);
                                         
                }
                catch (NullReferenceException e)
                {
                    List<WetlandPlant> emptyPlants = new List<WetlandPlant>();
                    plants = emptyPlants;
                }

            }

            return new ObservableCollection<WetlandPlant>(plants);

        }

        public async Task<ObservableCollection<WetlandPlant>> FilterPlantsByElevation(int elevation,string choice)
        {
            List<WetlandPlant> elevationPlants;

            try
            {
                if (choice.Equals("min"))
                {
                    elevationPlants = App.WetlandPlantRepoLocal.GetAllWetlandPlants().AsQueryable().Where(x => x.elevminfeet >= elevation).ToList();
                }
                else if (choice.Equals("max"))
                {
                    elevationPlants = App.WetlandPlantRepoLocal.GetAllWetlandPlants().AsQueryable().Where(x => x.elevmaxfeet <= elevation).ToList();
                }
                else
                {
                    List<WetlandPlant> emptyPlants = new List<WetlandPlant>();
                    elevationPlants = emptyPlants;
                }
            }
            catch (NullReferenceException e)
            {
                List<WetlandPlant> emptyPlants = new List<WetlandPlant>();
                elevationPlants = emptyPlants;
            }

            ObservableCollection<WetlandPlant> elevationCollection = new ObservableCollection<WetlandPlant>(elevationPlants);

            return elevationCollection;

        }

        public async Task<ObservableCollection<WetlandPlant>> FilterPlantsByRank(string rank, string choice)
        {
            List<WetlandPlant> rankPlants;

            try
            {
                if (choice.Equals("g"))
                {
                    rankPlants = App.WetlandPlantRepoLocal.GetAllWetlandPlants().AsQueryable().Where(x => x.grank.Contains(rank)).ToList();
                }
                else if (choice.Equals("s"))
                {
                    rankPlants = App.WetlandPlantRepoLocal.GetAllWetlandPlants().AsQueryable().Where(x => x.cosrank.Contains(rank)).ToList();
                }
                else
                {
                    List<WetlandPlant> emptyPlants = new List<WetlandPlant>();
                    rankPlants = emptyPlants;
                }
            }
            catch (NullReferenceException e)
            {
                List<WetlandPlant> emptyPlants = new List<WetlandPlant>();
                rankPlants = emptyPlants;
            }

            ObservableCollection<WetlandPlant> rankCollection = new ObservableCollection<WetlandPlant>(rankPlants);

            return rankCollection;

        }

        public async Task<ObservableCollection<WetlandPlant>> FilterPlantsByCounty(string county)
        {
            List<WetlandCountyPlant> countyPlants;
            List<WetlandPlant> searchPlantsCounty =  new List<WetlandPlant>();


            countyPlants = App.WetlandCountyPlantRepoLocal.GetAllCounties().AsQueryable().Where(x => county.Equals(x.name)).ToList();
            foreach (var count in countyPlants)
            {
                if (!searchPlantsCounty.Contains(GetWetlandPlantByAltId(count.plantid)))
                {
                    searchPlantsCounty.Add(GetWetlandPlantByAltId(count.plantid));
                }
            }



            return new ObservableCollection<WetlandPlant>(searchPlantsCounty);
        }

        public async Task<ObservableCollection<WetlandPlant>> FilterPlantsByWetlandType(string wetlandType)
        {
            List<WetlandPlant> wetlandTypePlants;

            wetlandTypePlants = App.WetlandPlantRepoLocal.GetAllWetlandPlants().AsQueryable().Where(x => x.ecologicalsystems.Contains(wetlandType)).ToList();

            return new ObservableCollection<WetlandPlant>(wetlandTypePlants);
        }

        // return a list of WetlandPlants saved to the WetlandPlant table in the database
        public async Task<ObservableCollection<WetlandPlant>> GetAllWetlandPlantsAsync()
        {
            List<WetlandPlant> list = allWetlandPlants;
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
            List<WetlandPlant> searchRegion = GetAllWetlandPlants();
            List<WetlandPlant> searchCombined = new List<WetlandPlant>();
            List<WetlandPlant> allPlants = GetAllWetlandPlants();
            // Add selected Leaf Type characteristics


            var queryWetlandOther = selectCritList.Where(x => x.Characteristic.Contains("group") || x.Characteristic.Contains("nativity") || x.Characteristic.Contains("federal") || x.Characteristic.Contains("status") || x.Characteristic.Contains("noxiousweed") || x.Characteristic.Contains("animaluse"));
            if (queryWetlandOther.Count() > 0)
            {
                searchPlantsOther = App.WetlandPlantRepoLocal.GetAllWetlandPlants().AsQueryable().Where(ConstructPredicate(selectCritList)).ToList();

            }

            var queryColors = selectCritList.Where(x => x.Characteristic.Contains("color"));
            if (queryColors.Count() > 0)
            {
                searchPlantsColor = new List<WetlandPlant>();

                List<WetlandPlantFruits> fruits = App.WetlandPlantFruitsRepoLocal.GetAllWetlandFruits().AsQueryable().Where(ConstructFruitPredicate(selectCritList)).ToList();
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

            var queryRegion = selectCritList.Where(x => x.Characteristic.Contains("region"));
            if (queryRegion.Count() > 0)
            {
                searchRegion = new List<WetlandPlant>();

                List<WetlandRegions> regions = App.WetlandRegionRepoLocal.GetAllWetlandRegions().AsQueryable().Where(ConstructRegionPredicate(selectCritList)).ToList();
                foreach (var region in regions)
                {
                    if (!searchRegion.Contains(GetWetlandPlantByAltId(region.plantid)))
                    {
                        searchRegion.Add(GetWetlandPlantByAltId(region.plantid));
                    }
                }
            }


            foreach (var plant in allPlants)
                if (searchPlantsColor.Contains(plant) && searchPlantsDivision.Contains(plant) && searchPlantsShape.Contains(plant) && searchPlantsArrangement.Contains(plant) && searchPlantsSize.Contains(plant) && searchRegion.Contains(plant) && searchPlantsOther.Contains(plant))
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

        private Expression<Func<WetlandRegions, bool>> ConstructRegionPredicate(List<WetlandSearch> selectCritList)
        {
            var overallQuery = PredicateBuilder.True<WetlandRegions>();

            // Add selected Leaf Type characteristics
            var queryRegions = selectCritList.Where(x => x.Characteristic.Contains("region"));
            if (queryRegions.Count() > 0)
            {
                var regionQuery = PredicateBuilder.False<WetlandRegions>();
                foreach (var region in queryRegions)
                {
                    regionQuery = regionQuery.Or(x => x.valueid == (region.SearchInt1));
                }
                overallQuery = overallQuery.And(regionQuery);
            }

            return overallQuery;
        }

        private Expression<Func<WetlandPlant, bool>> ConstructPredicate(List<WetlandSearch> selectCritList)
        {
            var overallQuery = PredicateBuilder.True<WetlandPlant>();
            // Add selected Flower Color characteristics        

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

            var queryNoxiousWeed = selectCritList.Where(x => x.Characteristic.Contains("noxiousweed"));
            if (queryNoxiousWeed.Count() > 0)
            {
                var weedQuery = PredicateBuilder.False<WetlandPlant>();
                foreach (var weed in queryNoxiousWeed) { weedQuery = weedQuery.Or(x => x.noxiousweed.Contains(weed.SearchString1)); }
                overallQuery = overallQuery.And(weedQuery);
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

            var queryAnimalUse = selectCritList.Where(x => x.Characteristic.Contains("animaluse"));
            if (queryAnimalUse.Count() > 0)
            {
                var animalUseQuery = PredicateBuilder.False<WetlandPlant>();
                foreach (var use in queryAnimalUse) { animalUseQuery = animalUseQuery.Or(x => x.animaluse.Contains(use.SearchString1)); }
                overallQuery = overallQuery.And(animalUseQuery);
            }

            return overallQuery;

        }

    }
}