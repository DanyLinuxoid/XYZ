using Dapper;

namespace XYZ.DataAccess.Interfaces
{
    public interface IDatabaseCommandExecutionLogic
    {
        Task<long> CreateAsync(string sql, DynamicParameters parameters);
        Task<long> DeleteAsync(string sql, DynamicParameters parameters);
        Task<long> UpdateAsync(string sql, DynamicParameters parameters);
    }
}