namespace XYZ.DataAccess.Interfaces
{
    public interface IQuery<T>
    {
        Task<T?> ExecuteAsync(IDatabaseQueryExecutionLogic databaseQueryExecutionLogic);
    }
}
