using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Threading.Tasks;
using System;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;

namespace PortableApp
{

    public class WetlandMapOverlayRepository : DBConnection
	{

        public string StatusMessage { get; set; }

        public WetlandMapOverlayRepository()
		{
            // Create the WetlandMapOverlay table (only if it's not yet created)
            conn.DropTable<WetlandMapOverlay>();
            conn.CreateTable<WetlandMapOverlay>();
            SeedDB();
		}

        // return a list of WetlandMapOverlays saved to the WetlandMapPolygon table in the database
        public List<WetlandMapOverlay> GetAllWetlandMapOverlays()
        {
            return conn.GetAllWithChildren<WetlandMapOverlay>();
        }

        // Seed database with essential settings
        public void SeedDB()
        {
            conn.Insert(new WetlandMapOverlay { mapKeyName = "PEMC", legendName = "Emergent" });
            conn.Insert(new WetlandMapOverlayCoordinate { overlayid = 1, latitude = 37.797513, longitude = -122.402058 });
            conn.Insert(new WetlandMapOverlayCoordinate { overlayid = 1, latitude = 37.798433, longitude = -122.402256 });
            conn.Insert(new WetlandMapOverlayCoordinate { overlayid = 1, latitude = 37.798582, longitude = -122.401071 });
            conn.Insert(new WetlandMapOverlayCoordinate { overlayid = 1, latitude = 37.798082, longitude = -122.400971 });

            conn.Insert(new WetlandMapOverlay { mapKeyName = "PEMG", legendName = "Forested" });
            conn.Insert(new WetlandMapOverlayCoordinate { overlayid = 2, latitude = 37.787513, longitude = -122.402058 });
            conn.Insert(new WetlandMapOverlayCoordinate { overlayid = 2, latitude = 37.788433, longitude = -122.402256 });
            conn.Insert(new WetlandMapOverlayCoordinate { overlayid = 2, latitude = 37.788582, longitude = -122.401071 });
            conn.Insert(new WetlandMapOverlayCoordinate { overlayid = 2, latitude = 37.788082, longitude = -122.400971 });

            conn.Insert(new WetlandMapOverlay { mapKeyName = "PEMG", legendName = "Lake" });
            conn.Insert(new WetlandMapOverlayCoordinate { overlayid = 3, latitude = 37.777513, longitude = -122.402058 });
            conn.Insert(new WetlandMapOverlayCoordinate { overlayid = 3, latitude = 37.778433, longitude = -122.403256 });
            conn.Insert(new WetlandMapOverlayCoordinate { overlayid = 3, latitude = 37.778582, longitude = -122.401071 });
            conn.Insert(new WetlandMapOverlayCoordinate { overlayid = 3, latitude = 37.778082, longitude = -122.400971 });
            conn.Insert(new WetlandMapOverlayCoordinate { overlayid = 3, latitude = 37.776082, longitude = -122.400971 });
        }

    }

    public class WetlandMapOverlayCoordinateRepository : DBConnection
    {

        public string StatusMessage { get; set; }

        public WetlandMapOverlayCoordinateRepository()
        {
            // Create the WetlandMapOverlayCoordinate table (only if it's not yet created)
            conn.DropTable<WetlandMapOverlayCoordinate>();
            conn.CreateTable<WetlandMapOverlayCoordinate>();
        }

    }
}