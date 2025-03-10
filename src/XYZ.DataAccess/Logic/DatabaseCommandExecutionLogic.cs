using Dapper;

using Microsoft.Data.SqlClient;

using System.Data;

using XYZ.DataAccess.Interfaces;

namespace XYZ.DataAccess.Logic
{
    /// <summary>
    /// Session with actual executable methods
    /// </summary>
    public class DatabaseCommandExecutionLogic : IDatabaseCommandExecutionLogic
    {
        /// <summary>
        /// Connection string to db.
        /// </summary>
        private string _connectionString { get; }

        public DatabaseCommandExecutionLogic(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Async command to create object in database.
        /// </summary>
        /// <param name="sql">Command to execute.</param>
        /// <param name="parameters">Parameters for command.</param>
        /// <returns>Execution result object which contains inserted id, or error message if error occurred.</returns>
        public async Task<long> CreateAsync(string sql, DynamicParameters parameters) => await ExecuteCommandAsync(sql, parameters);

        /// <summary>
        /// Async command to update object in database.
        /// </summary>
        /// <param name="sql">Command to execute.</param>
        /// <param name="parameters">Parameters for command.</param>
        /// <returns>Execution result object which contains inserted id, or error message if error occurred.</returns>
        public async Task<long> UpdateAsync(string sql, DynamicParameters parameters) => await ExecuteCommandAsync(sql, parameters);

        /// <summary>
        /// Async command to delete object in database.
        /// </summary>
        /// <param name="sql">Command to execute.</param>
        /// <param name="parameters">Parameters for command.</param>
        /// <returns>Execution result object which contains inserted id, or error message if error occurred.</returns>
        public async Task<long> DeleteAsync(string sql, DynamicParameters parameters) => await ExecuteCommandAsync(sql, parameters);

        /// <summary>
        /// Centralized method to execute CUD commands.
        /// </summary>
        /// <param name="sql">Sql of command to execute.</param>
        /// <param name="parameters">Dynamic parameters for command.</param>
        /// <returns>Result of execution which contains id of processed object or error message if something went wrong.</returns>
        private async Task<long> ExecuteCommandAsync(string sql, DynamicParameters parameters)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.ExecuteScalarAsync<long>(sql: sql, param: new DynamicParameters(parameters));
            }
        }
    }
}
