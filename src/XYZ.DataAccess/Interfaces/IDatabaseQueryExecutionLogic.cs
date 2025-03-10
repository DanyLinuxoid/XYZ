namespace XYZ.DataAccess.Interfaces
{
    public interface IDatabaseQueryExecutionLogic
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null);
        Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null);
    }
}