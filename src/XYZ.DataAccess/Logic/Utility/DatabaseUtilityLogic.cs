using Dapper;

using Microsoft.Data.SqlClient;

using System.Data;

using XYZ.DataAccess.Interfaces;

namespace XYZ.DataAccess.Logic.Utility
{
    public class DatabaseUtilityLogic : IDatabaseUtilityLogic
    {
        private readonly string _connectionString;

        public DatabaseUtilityLogic(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> PingAsync()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var result = await db.QueryFirstOrDefaultAsync<int>("SELECT 1");
                    return result == 1;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
