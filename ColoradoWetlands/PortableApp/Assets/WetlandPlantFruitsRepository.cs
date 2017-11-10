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
            //conn.DropTable<WetlandPlantSimilarSpecies>();
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

    }
}
