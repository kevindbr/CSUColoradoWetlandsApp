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

        public WetlandPlantRepositoryLocal(List<WetlandPlant> allPlantsDB)
		{
            allWetlandPlants = allPlantsDB;

        }

        // return a list of Wetlandplants saved to the WetlandPlant table in the database
        public List<WetlandPlant> GetAllWetlandPlants()
        {
            return allWetlandPlants;
        }

        public List<string> GetPlantJumpList()
        {
            return allWetlandPlants.Select(x => x.scinameauthorstripped[0].ToString()).Distinct().ToList();
        }

        // return a specific WetlandPlant given an id
        public WetlandPlant GetWetlandPlantByAltId(int Id)
        {
            IEnumerable<WetlandPlant> plants = allWetlandPlants.Where(p => p.plantid.Equals(Id));

            if (plants != null)
                return plants.First();

            else return null;
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
           List<WetlandPlant> plants;
           // List<WetlandPlantFruits> fruits = App.WetlandPlantFruitsRepo.GetAllWetlandFruits();
            // execute filtering
            if (selectCritList.Count() > 0)
            {
                try
                {
                    plants = GetSearchedPlants(selectCritList);

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
            List<WetlandPlant> list = allWetlandPlants;
            return new ObservableCollection<WetlandPlant>(list);
        }

        // Construct Predicate for plants query, filtering based on selected criteria
        // Solution taken from http://www.albahari.com/nutshell/predicatebuilder.aspx
        /* private Expression<Func<WetlandPlant, bool>> ConstructPredicate(List<WetlandSearch> selectCritList)
         {
             var overallQuery = PredicateBuilder.True<WetlandPlant>();

             // Add selected Leaf Type characteristics
             var queryColors = selectCritList.Where(x => x.Characteristic.Contains("color"));
             if (queryColors.Count() > 0)
             {
                 List<WetlandPlantFruits> fruits = App.WetlandPlantFruitsRepo.GetAllWetlandFruits().AsQueryable().Where(ConstructFruitPredicate(selectCritList)).ToList();
                 //overallQuery = 

                 var colorQuery = PredicateBuilder.False<WetlandPlant>();
                 foreach (var color in queryColors)
                 {

                    // foreach(var fruit in colorQuery.Or(x => x.FruitWetland))


                         colorQuery = colorQuery.Or(x => x.color == (color.SearchString1));
                 }
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
         }*/


        private List<WetlandPlant> GetSearchedPlants(List<WetlandSearch> selectCritList)
        {
            List<WetlandPlant> searchPlantsColor = GetAllWetlandPlants();
            List<WetlandPlant> searchPlantsDivision = GetAllWetlandPlants();
            List<WetlandPlant> searchPlantsShape = GetAllWetlandPlants();
            List<WetlandPlant> searchPlantsArrangement = GetAllWetlandPlants();
            List<WetlandPlant> searchPlantsSize = GetAllWetlandPlants();
            List<WetlandPlant> searchCombined = new List<WetlandPlant>();
            List<WetlandPlant> allPlants = GetAllWetlandPlants();
            // Add selected Leaf Type characteristics
            var queryColors = selectCritList.Where(x => x.Characteristic.Contains("color"));
            if (queryColors.Count() > 0)
            {
                searchPlantsColor = new List<WetlandPlant>();

                List<WetlandPlantFruits> fruits = App.WetlandPlantFruitsRepo.GetAllWetlandFruits().AsQueryable().Where(ConstructFruitPredicate(selectCritList)).ToList();
                foreach(var fruit in fruits)
                {
                    if(!searchPlantsColor.Contains(GetWetlandPlantByAltId(fruit.plantid)))
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
                if (searchPlantsColor.Contains(plant) && searchPlantsDivision.Contains(plant) && searchPlantsShape.Contains(plant) && searchPlantsArrangement.Contains(plant) && searchPlantsSize.Contains(plant))
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

    }
}