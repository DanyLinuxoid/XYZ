namespace XYZ.DataAccess.Interfaces
{
    public interface IDatabaseUtilityLogic
    {
        Task<bool> PingAsync();
    }
}