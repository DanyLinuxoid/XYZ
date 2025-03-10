using Dapper;

using Microsoft.Data.SqlClient;

using System.Data;

using XYZ.DataAccess.Interfaces;

namespace XYZ.DataAccess.Logic.Utility
{
    /// <summary>
    /// Utility operations to check stability, uptime, avaiability, etc.
    /// </summary>
    public class DatabaseUtilityLogic : IDatabaseUtilityLogic
    {
        /// <summary>
        /// Connection string.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Utility operations to check stability, uptime, avaiability, etc.
        /// </summary>
        public DatabaseUtilityLogic(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Checks if database is available.
        /// </summary>
        /// <returns>True if available, false otherwise.</returns>
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
