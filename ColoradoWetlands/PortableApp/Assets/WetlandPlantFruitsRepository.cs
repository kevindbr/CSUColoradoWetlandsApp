using System;
using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;

namespace PortableApp
{

    public class WetlandPlantFruitsRepository : DBConnection
    {

        public WetlandPlantFruitsRepository()
        {
            // Create the Wetland Plant SimilarSpecies table
            conn.CreateTable<WetlandPlantFruits>();
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandPlantFruits> GetAllWetlandFruits()
        {
            return (from p in conn.Table<WetlandPlantFruits>() select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandPlantFruits> PlantsFruits(int plantId)
        {
            return conn.Table<WetlandPlantFruits>().Where(p => p.plantid.Equals(plantId)).ToList();
        }

        public void ClearWetlandFruits()
        {
            conn.DropTable<WetlandPlantFruits>();
            conn.CreateTable<WetlandPlantFruits>();
        }

    }

    public class WetlandPlantRegionRepository : DBConnection
    {

        public WetlandPlantRegionRepository()
        {
            // Create the Wetland Plant SimilarSpecies table
            conn.CreateTable<WetlandRegions>();
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandRegions> GetAllWetlandRegions()
        {
            return (from p in conn.Table<WetlandRegions>() select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandRegions> PlantsRegions(int plantId)
        {
            return conn.Table<WetlandRegions>().Where(p => p.plantid.Equals(plantId)).ToList();
        }

        public void ClearWetlandRegions()
        {
            conn.DropTable<WetlandRegions>();
            conn.CreateTable<WetlandRegions>();
        }

    }

    public class WetlandPlantDivisionRepository : DBConnection
    {
        public WetlandPlantDivisionRepository()
        {
            // Create the Wetland Plant SimilarSpecies table
            conn.CreateTable<WetlandPlantDivision>();
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandPlantDivision> GetAllDivisions()
        {
            return (from p in conn.Table<WetlandPlantDivision>() select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandPlantDivision> PlantsDivision(int plantId)
        {
            return conn.Table<WetlandPlantDivision>().Where(p => p.plantid.Equals(plantId)).ToList();
        }

        public void ClearWetlandDivisions()
        {
            conn.DropTable<WetlandPlantDivision>();
            conn.CreateTable<WetlandPlantDivision>();
        }
    }

    public class WetlandPlantShapeRepository : DBConnection
    {
        public WetlandPlantShapeRepository()
        {
            // Create the Wetland Plant SimilarSpecies table
            conn.CreateTable<WetlandPlantShape>();
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandPlantShape> GetAllShapes()
        {
            return (from p in conn.Table<WetlandPlantShape>() select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandPlantShape> PlantsShape(int plantId)
        {
            return conn.Table<WetlandPlantShape>().Where(p => p.plantid.Equals(plantId)).ToList();
        }

        public void ClearWetlandShapes()
        {
            conn.DropTable<WetlandPlantShape>();
            conn.CreateTable<WetlandPlantShape>();
        }
    }

    public class WetlandPlantLeafArrangementRepository : DBConnection
    {
        public WetlandPlantLeafArrangementRepository()
        {
            // Create the Wetland Plant SimilarSpecies table
            conn.CreateTable<WetlandPlantArrangement>();
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandPlantArrangement> GetAllArrangements()
        {
            return (from p in conn.Table<WetlandPlantArrangement>() select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandPlantArrangement> PlantsShape(int plantId)
        {
            return conn.Table<WetlandPlantArrangement>().Where(p => p.plantid.Equals(plantId)).ToList();
        }

        public void ClearWetlandArrangements()
        {
            conn.DropTable<WetlandPlantArrangement>();
            conn.CreateTable<WetlandPlantArrangement>();
        }
    }

    public class WetlandPlantSizeRepository : DBConnection
    {
        public WetlandPlantSizeRepository()
        {
            // Create the Wetland Plant SimilarSpecies table
            conn.CreateTable<WetlandPlantSize>();
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandPlantSize> GetAllPlantSizes()
        {
            return (from p in conn.Table<WetlandPlantSize>() select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandPlantSize> PlantsShape(int plantId)
        {
            return conn.Table<WetlandPlantSize>().Where(p => p.plantid.Equals(plantId)).ToList();
        }
        public void ClearWetlandSizes()
        {
            conn.DropTable<WetlandPlantSize>();
            conn.CreateTable<WetlandPlantSize>();
        }
    }


    public class WetlandCountyPlantRepository : DBConnection
    {
        public WetlandCountyPlantRepository()
        {
            // Create the Wetland Plant SimilarSpecies table
            conn.CreateTable<WetlandCountyPlant>();
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandCountyPlant> GetAllCounties()
        {
            return (from p in conn.Table<WetlandCountyPlant>() select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandCountyPlant> PlantCounty(int plantId)
        {
            return conn.Table<WetlandCountyPlant>().Where(p => p.plantid.Equals(plantId)).ToList();
        }
        public void ClearWetlandCounties()
        {
            conn.DropTable<WetlandCountyPlant>();
            conn.CreateTable<WetlandCountyPlant>();
        }
    }


    public class WetlandPlantFruitsRepositoryLocal 
    {
        private List<WetlandPlantFruits> allItems;

        public WetlandPlantFruitsRepositoryLocal(List<WetlandPlantFruits> fromDB)
        {
            // Create the Wetland Plant SimilarSpecies table
            allItems = fromDB;
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandPlantFruits> GetAllWetlandFruits()
        {
            return (from p in allItems select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandPlantFruits> PlantsFruits(int plantId)
        {
            return allItems.Where(p => p.plantid.Equals(plantId)).ToList();
        }

        public void ClearWetlandFruitsLocal()
        {
            allItems = new List<WetlandPlantFruits>();
        }

    }

    public class WetlandPlantRegionRepositoryLocal
    {
        private List<WetlandRegions> allItems;

        public WetlandPlantRegionRepositoryLocal(List<WetlandRegions> fromDB)
        {
            // Create the Wetland Plant SimilarSpecies table
            allItems = fromDB;
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandRegions> GetAllWetlandRegions()
        {
            return (from p in allItems select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandRegions> PlantsRegions(int plantId)
        {
            return allItems.Where(p => p.plantid.Equals(plantId)).ToList();
        }

        public void ClearWetlandRegionsLocal()
        {
            allItems = new List<WetlandRegions>();
        }

    }

    public class WetlandPlantDivisionRepositoryLocal 
    {
        private List<WetlandPlantDivision> allItems;

        public WetlandPlantDivisionRepositoryLocal(List<WetlandPlantDivision> fromDB)
        {
            // Create the Wetland Plant SimilarSpecies table
            allItems = fromDB;
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandPlantDivision> GetAllDivisions()
        {
            return (from p in allItems select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandPlantDivision> PlantsDivision(int plantId)
        {
            return allItems.Where(p => p.plantid.Equals(plantId)).ToList();
        }

        public void ClearWetlandDivisionsLocal()
        {
            allItems = new List<WetlandPlantDivision>();
        }
    }

    public class WetlandPlantShapeRepositoryLocal
    {
        private List<WetlandPlantShape> allItems;

        public WetlandPlantShapeRepositoryLocal(List<WetlandPlantShape> fromDB)
        {
            // Create the Wetland Plant SimilarSpecies table
            allItems = fromDB;
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandPlantShape> GetAllShapes()
        {
            return (from p in allItems select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandPlantShape> PlantsShape(int plantId)
        {
            return allItems.Where(p => p.plantid.Equals(plantId)).ToList();
        }

        public void ClearWetlandShapesLocal()
        {
            allItems = new List<WetlandPlantShape>();
        }
    }

    public class WetlandPlantLeafArrangementRepositoryLocal
    {
        private List<WetlandPlantArrangement> allItems;

        public WetlandPlantLeafArrangementRepositoryLocal(List<WetlandPlantArrangement> fromDB)
        {
            // Create the Wetland Plant SimilarSpecies table
            allItems = fromDB;
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandPlantArrangement> GetAllArrangements()
        {
            return (from p in allItems select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandPlantArrangement> PlantsShape(int plantId)
        {
            return allItems.Where(p => p.plantid.Equals(plantId)).ToList();
        }

        public void ClearWetlandArrangementsLocal()
        {
            allItems = new List<WetlandPlantArrangement>();
        }
    }

    public class WetlandPlantSizeRepositoryLocal
    {
        private List<WetlandPlantSize> allItems;

        public WetlandPlantSizeRepositoryLocal(List<WetlandPlantSize> fromDB)
        {
            // Create the Wetland Plant SimilarSpecies table
            allItems = fromDB;
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandPlantSize> GetAllSizes()
        {
            return (from p in allItems select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandPlantSize> PlantsShape(int plantId)
        {
            return allItems.Where(p => p.plantid.Equals(plantId)).ToList();
        }

        public void ClearWetlandSizesLocal()
        {
            allItems = new List<WetlandPlantSize>();
        }
    }

    public class WetlandCountyPlantRepositoryLocal
    {
        private List<WetlandCountyPlant> allItems;

        public WetlandCountyPlantRepositoryLocal(List<WetlandCountyPlant> fromDB)
        {
            // Create the Wetland Plant SimilarSpecies table
            allItems = fromDB;
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandCountyPlant> GetAllCounties()
        {
            return (from p in allItems select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandCountyPlant> PlantCounty(int plantId)
        {
            return allItems.Where(p => p.plantid.Equals(plantId)).ToList();
        }

        public void ClearWetlandCountiesLocal()
        {
            allItems = new List<WetlandCountyPlant>();
        }
    }


}
