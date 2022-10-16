using Pantree.Data.Access;

namespace Pantree.Services
{
    /// <summary>
    /// Class that services in the Pantree project inherit from, providing a database connection.
    /// </summary>
    public class PantreeService
    {
        internal readonly DatabaseAccess db;

        /// <summary>
        /// Creates a new instance of the PantreeService class, creating a new database connection.
        /// </summary>
        /// <param name="dbConnectionString">The connection string to the database.</param>
        public PantreeService(string dbConnectionString)
        {
            db = new DatabaseAccess(dbConnectionString);
        }
    }
}
