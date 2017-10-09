using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;

namespace PortableApp
{

    public class WetlandPlantReferenceRepository : DBConnection
    {

        public WetlandPlantReferenceRepository()
        {
            // Create the Wetland Plant Reference table
            conn.DropTable<WetlandPlantReference>();
            conn.CreateTable<WetlandPlantReference>();
        }

        // return a list of Wetland Plant References saved to the WetlandPlantReference table in the database
        public List<WetlandPlantReference> GetAllWetlandPlantReferences()
        {
            return (from p in conn.Table<WetlandPlantReference>() select p).ToList();
        }

        // return a list of Wetland Plant References for the plant specified
        public List<WetlandPlantReference> PlantReferences(int plantId)
        {
            return conn.Table<WetlandPlantReference>().Where(p => p.PlantId.Equals(plantId)).ToList();
        }

    }
}