namespace XYZ.DataAccess.Interfaces
{
    /// <summary>
    /// Query command.
    /// </summary>
    /// <typeparam name="T">Command to execute.</typeparam>
    public interface IQuery<T>
    {
        /// <summary>
        /// Query command.
        /// </summary>
        /// <typeparam name="T">Command to execute.</typeparam>
        Task<T?> ExecuteAsync(IDatabaseQueryExecutionLogic databaseQueryExecutionLogic);
    }
}
