using SQLite.Net;
using SQLite.Net.Async;

namespace PortableApp
{
    // This class initializes a synchronous and asynchronous SQLite connection that can be used when a class inherits from this one
    public class DBConnection
    {
        protected static SQLiteConnection conn { get; set; }
        protected static SQLiteAsyncConnection connAsync { get; set; }

        // Initialize connection if it hasn't already been initialized
        public DBConnection(dynamic newConn = null)
        {
            if (conn == null && newConn.GetType() == typeof(SQLiteConnection)) { conn = newConn; }
            if (connAsync == null && newConn.GetType() == typeof(SQLiteAsyncConnection)) { connAsync = newConn; }
        }
        
    }
}