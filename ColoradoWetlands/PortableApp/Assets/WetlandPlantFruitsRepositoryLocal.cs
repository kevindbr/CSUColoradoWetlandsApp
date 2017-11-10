using System;
using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;

namespace PortableApp
{

    public class WetlandPlantFruitsRepositoryLocal 
    {
        private List<WetlandPlantFruits> allWetlandPlantFruits;

        public WetlandPlantFruitsRepositoryLocal(List<WetlandPlantFruits> fruitsDB)
        {
            allWetlandPlantFruits = fruitsDB;
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandPlantFruits> GetAllWetlandFruits()
        {
            return allWetlandPlantFruits;
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandPlantFruits> PlantsFruits(int plantId)
        {
            return allWetlandPlantFruits.Where(p => p.plantid.Equals(plantId)).ToList();
        }

    }
}
