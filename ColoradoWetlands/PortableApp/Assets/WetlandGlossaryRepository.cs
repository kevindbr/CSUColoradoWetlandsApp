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

        public async Task AddOrUpdateTermAsync(WetlandGlossary term)
        {
            try
            {
                if (string.IsNullOrEmpty(term.name))
                    throw new Exception("Valid term required");
                await connAsync.InsertOrReplaceAsync(term);
            }
            catch
            {
                
            }

        }

        public async Task AddOrUpdateAllTermsAsync(IList<WetlandGlossary> terms)
        {
            try
            {
                // await connAsync.InsertOrReplaceWithChildrenAsync(plant);
                await connAsync.RunInTransactionAsync((SQLite.Net.SQLiteConnection tran) =>
                {
                    connAsync.InsertOrReplaceAllAsync(terms);
                });
            }
            catch (Exception ex)
            {
               // StatusMessage = string.Format("Failed to add {0}. Error: {1}", "plants", ex.Message);
            }

        }

    }
}
