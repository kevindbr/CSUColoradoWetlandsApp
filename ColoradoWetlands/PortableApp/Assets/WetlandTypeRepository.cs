using System;
using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using SQLite;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp
{

    public class WetlandTypeRepository : DBConnection
	{
        public WetlandTypeRepository()
        {
            // Create the Type table
            conn.CreateTable<WetlandType>();
        }
        
        public List<WetlandType> GetAllWetlandTypes()
        {
            // return a list of plants saved to the Type table in the database
            return (from p in conn.Table<WetlandType>() select p).ToList();
        }
        
    }
}