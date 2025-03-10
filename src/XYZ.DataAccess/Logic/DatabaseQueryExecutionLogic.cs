using Dapper;

using Microsoft.Data.SqlClient;

using System.Data;

using XYZ.DataAccess.Interfaces;

namespace XYZ.DataAccess.Logic
{
    /// <summary>
    /// Core query execution logic.
    /// </summary>
    public class DatabaseQueryExecutionLogic : IDatabaseQueryExecutionLogic
    {
        /// <summary>
        /// Connection string to db.
        /// </summary>
        private string _connectionString { get; }

        /// <summary>
        /// Core query execution logic.
        /// </summary>
        public DatabaseQueryExecutionLogic(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Simple async query to database.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="sql">Sql query.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <returns>IEnumerable sequence of elements.</returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<T>(sql, parameters);
            }
        }

        /// <summary>
        /// Simple async query to database.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="sql">Sql query.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <returns>One element, or null if not found.</returns>
        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<T?>(sql, parameters);
            }
        }
    }
}
