using SQLite;
using System.Collections.ObjectModel;

namespace PortableApp
{
    public class DBConnection
    {
        protected static SQLiteConnection conn { get; set; }

        // Initialize connection if it hasn't already been initialized
        public DBConnection(SQLiteConnection newConn = null)
        {
            if (conn == null) { conn = newConn; }
        }
    }
}