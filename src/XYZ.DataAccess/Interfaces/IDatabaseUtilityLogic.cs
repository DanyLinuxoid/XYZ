namespace XYZ.DataAccess.Interfaces
{
    /// <summary>
    /// Utility operations to check stability, uptime, avaiability, etc.
    /// </summary>
    public interface IDatabaseUtilityLogic
    {
        /// <summary>
        /// Checks if database is available.
        /// </summary>
        /// <returns>True if available, false otherwise.</returns>
        Task<bool> PingAsync();
    }
}