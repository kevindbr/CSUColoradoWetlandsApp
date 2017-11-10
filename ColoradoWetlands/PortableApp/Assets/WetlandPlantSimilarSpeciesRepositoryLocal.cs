using System;
using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;

namespace PortableApp
{

    public class WetlandPlantSimilarSpeciesRepositoryLocal : DBConnection
    {

        public WetlandPlantSimilarSpeciesRepositoryLocal()
        {
            // Create the Wetland Plant SimilarSpecies table
            //conn.DropTable<WetlandPlantSimilarSpecies>();
            conn.CreateTable<WetlandPlantSimilarSpecies>();
        }

        // return a list of Wetland Plant SimilarSpecies saved to the WetlandPlantSimilarSpecies table in the database
        public List<WetlandPlantSimilarSpecies> GetAllWetlandPlantSimilarSpecies()
        {
            return (from p in conn.Table<WetlandPlantSimilarSpecies>() select p).ToList();
        }

        // return a list of Wetland Plant SimilarSpecies for the plant specified
        public List<WetlandPlantSimilarSpecies> PlantSimilarSpecies(int plantId)
        {
            return conn.Table<WetlandPlantSimilarSpecies>().Where(p => p.PlantId.Equals(plantId)).ToList();
        }

    }
}
