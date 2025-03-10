namespace XYZ.DataAccess.Interfaces
{
    /// <summary>
    /// Core query execution logic.
    /// </summary>
    public interface IDatabaseQueryExecutionLogic
    {
        /// <summary>
        /// Simple async query to database.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="sql">Sql query.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <returns>IEnumerable sequence of elements.</returns>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null);

        /// <summary>
        /// Simple async query to database.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="sql">Sql query.</param>
        /// <param name="parameters">Query parameters.</param>
        /// <returns>One element, or null if not found.</returns>
        Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null);
    }
}