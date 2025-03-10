using System.ComponentModel;

using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;

namespace XYZ.DataAccess.Logic
{
    /// <summary>
    /// Database operations first layer.
    /// </summary>
    public class DatabaseLogic : IDatabaseLogic
    {
        private IDatabaseCommandExecutionLogic _databaseCommandExecutionLogic;
        private IDatabaseQueryExecutionLogic _databaseQueryExecutionLogic;

        public DatabaseLogic(IDatabaseCommandExecutionLogic databaseCommandExecutionLogic, IDatabaseQueryExecutionLogic databaseQueryExecutionLogic)
        {
            _databaseCommandExecutionLogic = databaseCommandExecutionLogic;
            _databaseQueryExecutionLogic = databaseQueryExecutionLogic;
        }

        /// <summary>
        /// Async query to database.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="sqlQuery">Query.</param>
        /// <returns>Entity that was queried.</returns>
        public Task<T?> QueryAsync<T>(IQuery<T> sqlQuery)
        {
            return sqlQuery.ExecuteAsync(_databaseQueryExecutionLogic);
        }

        /// <summary>
        /// Async command to database.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="command">Command to execute.</param>
        /// <param name="commandType">Command type (create, update, delete, async versions).</param>
        /// <param name="model">Model that should be manipulated.</param>
        /// <returns>Execution result that contains id of manipulated element and error message (if error occurred).</returns>
        public async Task<long> CommandAsync<T>(ICommandRepository<T> command, CommandTypes commandType, T model) where T : class
        {
            switch (commandType)
            {
                case CommandTypes.Create:
                    return await command.CreateAsync(model, _databaseCommandExecutionLogic);
                case CommandTypes.Update:
                    return await command.UpdateAsync(model, _databaseCommandExecutionLogic);
                case CommandTypes.Delete:
                    return await command.DeleteAsync(model, _databaseCommandExecutionLogic);
                default:
                    throw new InvalidEnumArgumentException($"{commandType} is unknown type");
            }
        }
    }
}
