namespace XYZ.Logic.Common.Interfaces
{
    public interface IGatewayOrderSavingLogic
    {
        Task<long> SaveOrder();
    }
}
