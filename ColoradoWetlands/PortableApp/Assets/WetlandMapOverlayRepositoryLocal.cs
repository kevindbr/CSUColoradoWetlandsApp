using System.Collections.Generic;
using PortableApp.Models;

namespace PortableApp
{

    public class WetlandMapOverlayRepositoryLocal 
	{

        public string StatusMessage { get; set; }
        private List<WetlandMapOverlay> allWetlandMapOverlays;

        public WetlandMapOverlayRepositoryLocal(List<WetlandMapOverlay> overlaysDB)
		{
            allWetlandMapOverlays = overlaysDB;
		}

        // return a list of WetlandMapOverlays saved to the WetlandMapPolygon table in the database
        public List<WetlandMapOverlay> GetAllWetlandMapOverlays()
        {
            return allWetlandMapOverlays;
        }

    }

}