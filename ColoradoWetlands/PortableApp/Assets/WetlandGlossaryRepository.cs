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

    public class WetlandGlossaryRepository : DBConnection
    { 

        public ObservableCollection<WetlandGlossary> terms;
        public List<string> alphabetList;

        public WetlandGlossaryRepository()
        {
            // Create the Wetland Glossary table
            conn.CreateTable<WetlandGlossary>();
        }

        // return a list of Wetland Glossary items (terms)
        public List<WetlandGlossary> GetAllWetlandTerms()
        {
            return (from p in conn.Table<WetlandGlossary>() select p).ToList();
        }

        public async Task AddTermAsync(WetlandGlossary term)
        {
            try
            {
                if (string.IsNullOrEmpty(term.name))
                    throw new Exception("Valid term required");
                await connAsync.InsertAsync(term);
            }
            catch
            {
                
            }

        }
        
    }
}
