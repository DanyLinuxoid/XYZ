using Dapper;

namespace XYZ.DataAccess.Interfaces
{
    /// <summary>
    /// Session with actual executable methods
    /// </summary>
    public interface IDatabaseCommandExecutionLogic
    {
        /// <summary>
        /// Async command to create object in database.
        /// </summary>
        /// <param name="sql">Command to execute.</param>
        /// <param name="parameters">Parameters for command.</param>
        /// <returns>Execution result object which contains inserted id, or error message if error occurred.</returns>
        Task<long> CreateAsync(string sql, DynamicParameters parameters);

        /// <summary>
        /// Async command to delete object in database.
        /// </summary>
        /// <param name="sql">Command to execute.</param>
        /// <param name="parameters">Parameters for command.</param>
        /// <returns>Execution result object which contains inserted id, or error message if error occurred.</returns>
        Task<long> DeleteAsync(string sql, DynamicParameters parameters);

        /// <summary>
        /// Async command to update object in database.
        /// </summary>
        /// <param name="sql">Command to execute.</param>
        /// <param name="parameters">Parameters for command.</param>
        /// <returns>Execution result object which contains inserted id, or error message if error occurred.</returns>
        Task<long> UpdateAsync(string sql, DynamicParameters parameters);
    }
}