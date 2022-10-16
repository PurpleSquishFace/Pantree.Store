using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using Pantree.Data.Models;
using System.Data.Common;

namespace Pantree.Data.Access
{
    /// <summary>
    /// Provides basic CRUD functionality using the Dapper ORM.
    /// </summary>
    public class DatabaseOperations
    {
        /// <summary>
        /// Returns a database connection.
        /// </summary>
        public DbConnection Connection => GetConnection();

        /// <summary>
        /// The connection string used to create the connection to the database.
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Instantiates the Database Operations class.
        /// </summary>
        /// <param name="connectionString">The database connection string used to created a connection.</param>
        public DatabaseOperations(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Creates a new database connection using the instantiated connection string.
        /// </summary>
        /// <returns>A new database connection.</returns>
        private DbConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        /// <summary>
        /// Inserts a new record in the database.
        /// </summary>
        /// <param name="record">The record object to insert.</param>
        /// <returns>The ID of the newly created record.</returns>
        public int InsertSingle<T>(IDatabaseTable record) where T : class
        {
            using var connection = Connection;
            return (int)connection.Insert((T)record);
        }

        /// <summary>
        /// Inserts a list of new records in the database.
        /// </summary>
        /// <param name="records">The list of record objects to insert.</param>
        public void InsertMultiple<T>(List<IDatabaseTable> records) where T : class
        {
            var parsedRecords = records.Select(i => (T)i).ToList();
            using var connection = Connection;
            connection.Insert(parsedRecords);
        }

        /// <summary>
        /// Updates a record in the database, by matching the primary key field of the provided object and updating the other fields.
        /// </summary>
        /// <param name="record">The record object to update.</param>
        /// <returns>If the update operation was successful.</returns>
        public bool UpdateSingle<T>(IDatabaseTable record) where T : class
        {
            using var connection = Connection;
            return connection.Update((T)record);
        }

        /// <summary>
        /// Updates a list of records in the database, by matching the primary key field of each provided object and updating the other fields.
        /// </summary>
        /// <param name="records">The list of record objects to update</param>
        public void UpdateMultiple<T>(List<IDatabaseTable> records) where T : class
        {
            var parsedRecords = records.Select(i => (T)i).ToList();
            using var connection = Connection;
            connection.Update(parsedRecords);
        }

        /// <summary>
        /// Deletes a record in the database, by matching the primary key field of the provided object and removing the record.
        /// </summary>
        /// <param name="record">The record object to delete.</param>
        /// <returns>If the delete operation was successful.</returns>
        public bool DeleteSingle<T>(IDatabaseTable record) where T : class
        {
            using var connection = Connection;
            return connection.Delete((T)record);
        }

        /// <summary>
        /// Deletes a list of records in the database, by matching the primary key of each provided object and removing the record.
        /// </summary>
        /// <param name="records">The list of record objects to delete.</param>
        public void DeleteMultiple<T>(List<T> records) where T : class
        {
            var parsedRecords = records.Select(i => (T)i).ToList();
            using var connection = Connection;
            connection.Delete(parsedRecords);
        }

        /// <summary>
        /// Gets a record from the database.
        /// </summary>
        /// <typeparam name="T">The object type to map the retrieved record to.</typeparam>
        /// <param name="id">The primary key of the record to retrieve.</param>
        /// <returns>The record, if found.</returns>
        public T GetSingle<T>(int id) where T : class
        {
            using var connection = Connection;
            return connection.Get<T>(id);
        }

        /// <summary>
        /// Gets a list of records from the database.
        /// </summary>
        /// <typeparam name="T">The object type to map the retrieved records to.</typeparam>
        /// <param name="ids">The primary keys of the records to retrieve.</param>
        /// <returns>The records, if found.</returns>
        public List<T> GetMultiple<T>(List<int> ids) where T : class
        {
            using var connection = Connection;
            return connection.Get<List<T>>(ids);
        }

        /// <summary>
        /// Executes a sql query against the database.
        /// </summary>
        /// <param name="sql">The query string to be executed.</param>
        /// <param name="param">A dynamic object representing the parameters used in the query.</param>
        public void ExecuteQuery(string sql, object param = null)
        {
            using var connection = Connection;
            connection.Query(sql, param);
        }

        /// <summary>
        /// Executes a sql query against the database, and returns the result.
        /// </summary>
        /// <typeparam name="T">The object type to map the retrieved result to.</typeparam>
        /// <param name="sql">The query string to be executed.</param>
        /// <param name="param">A dynamic object representing the parameters used in the query.</param>
        /// <returns>The result of the executed query.</returns>
        public T ExecuteQuery<T>(string sql, object param = null) 
        {
            using var connection = Connection;
            return connection.QuerySingleOrDefault<T>(sql, param);
        }
        
        /// <summary>
        /// Executes a sql query against the database, and returns the result as a list.
        /// </summary>
        /// <typeparam name="T">The object type to map the retrieved results to.</typeparam>
        /// <param name="sql">The query string to be executed.</param>
        /// <param name="param">A dynamic object representing the parameters used in the query.</param>
        /// <returns>The result of the executed query as a list.</returns>
        public List<T> ExecuteQueryList<T>(string sql, object param = null)
        {
            using var connection = Connection;
            var result = connection.Query<T>(sql, param);

            return result.ToList();
        }
    }
}
