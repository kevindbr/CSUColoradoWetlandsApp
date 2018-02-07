using System;
using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace PortableApp
{

    public class WetlandGlossaryRepositoryLocal 
    {

        private List<WetlandGlossary> allWetlandTerms;
        
        public WetlandGlossaryRepositoryLocal(List<WetlandGlossary> termsDB)
        {
            // Create the Wetland Glossary table
            //conn.CreateTable<WetlandGlossary>();
            allWetlandTerms = termsDB;
        }

        // return a list of Wetland Glossary items (terms)
        public List<WetlandGlossary> GetAllWetlandTerms()
        {
            return allWetlandTerms;
        }

        public void ClearWetlandGlossaryLocal()
        {
            allWetlandTerms = new List<WetlandGlossary>();
        }
    }
}
