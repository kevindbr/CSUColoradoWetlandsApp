using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;

namespace PortableApp
{

    public class WetlandPlantReferenceRepositoryLocal
    {
        private List<WetlandPlantReference> allWetlandPlantReferences;

        public WetlandPlantReferenceRepositoryLocal(List<WetlandPlantReference> plantReferencesDB)
        {
            // Create the Wetland Plant Reference table
            allWetlandPlantReferences = plantReferencesDB;
        }

        public void ClearWetlandReferencesLocal()
        {
            allWetlandPlantReferences = new List<WetlandPlantReference>();
        }

        // return a list of Wetland Plant References saved to the WetlandPlantReference table in the database
        public List<WetlandPlantReference> GetAllWetlandPlantReferences()
        {
            return allWetlandPlantReferences;
        }

        // return a list of Wetland Plant References for the plant specified
        public List<WetlandPlantReference> PlantReferences(int plantId)
        {
            return allWetlandPlantReferences.Where(p => p.PlantId.Equals(plantId)).ToList();
        }

    }
}