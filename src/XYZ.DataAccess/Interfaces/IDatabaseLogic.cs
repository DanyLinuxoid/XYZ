using XYZ.DataAccess.Enums;

namespace XYZ.DataAccess.Interfaces
{
    /// <summary>
    /// Database operations first layer (Wrapper).
    /// </summary>
    public interface IDatabaseLogic
    {
        /// <summary>
        /// Async query to database (Wrapper).
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="sqlQuery">Query.</param>
        /// <returns>Entity that was queried.</returns>
        Task<T?> QueryAsync<T>(IQuery<T> sqlQuery);

        /// <summary>
        /// Async command to database (Wrapper).
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="command">Command to execute.</param>
        /// <param name="commandType">Command type (create, update, delete, async versions).</param>
        /// <param name="model">Model that should be manipulated.</param>
        /// <returns>Execution result that contains id of manipulated element and error message (if error occurred).</returns>
        Task<long> CommandAsync<T>(ICommandRepository<T> command, CommandTypes commandType, T model) where T : class;
    }
}
