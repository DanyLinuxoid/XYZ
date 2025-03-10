namespace XYZ.Logic.Common.Interfaces
{
    public interface IApiOrderLogic<T, K>
    {
        Task<K> GetProcessedOrderResultAsync(T orderInfo);
    }
}
