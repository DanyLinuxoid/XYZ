namespace XYZ.DataAccess.Interfaces
{
    /// <summary>
    /// Repository of CUD commands + async version.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public interface ICommandRepository<T> where T : class
    {
        /// <summary>
        /// Async command to create record in database.
        /// </summary>
        /// <param name="dbo">Model to create.</param>
        /// <returns>Execution result which conatins created row id or error message if failed.</returns>
        Task<long> CreateAsync(T dbo, IDatabaseCommandExecutionLogic databaseCommandExecutionLogic);

        /// <summary>
        /// Async command to create record in database.
        /// </summary>
        /// <param name="dbo">Model to update.</param>
        /// <returns>Execution result which conatins created row id or error message if failed.</returns>
        Task<long> UpdateAsync(T dbo, IDatabaseCommandExecutionLogic databaseCommandExecutionLogic);

        /// <summary>
        /// Async command to delete record in database.
        /// </summary>
        /// <param name="dbo">Model to delete.</param>
        /// <returns>Execution result which conatins created row id or error message if failed.</returns>
        Task<long> DeleteAsync(T dbo, IDatabaseCommandExecutionLogic databaseCommandExecutionLogic);
    }
}
