using System;
using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using SQLite;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{

    public class WetlandPlantImageRepositoryLocal
    {
        private List<WetlandPlantImage> allWetlandPlantImages;

        public WetlandPlantImageRepositoryLocal(List<WetlandPlantImage> imagesDB)
        {
            allWetlandPlantImages = imagesDB;
        }

        // return a list of Wetland Plant Images saved to the WetlandPlantImage table in the database
        public List<WetlandPlantImage> GetAllWetlandPlantImages()
        {
            return allWetlandPlantImages;
        }

        // return a list of Wetland Plant Images for the plant specified
        public List<WetlandPlantImage> PlantImages(int plantId)
        {
            return allWetlandPlantImages.Where(p => p.PlantId.Equals(plantId)).ToList();
        }

    }
}