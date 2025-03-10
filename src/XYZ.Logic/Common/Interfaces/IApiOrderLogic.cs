namespace XYZ.Logic.Common.Interfaces
{
    /// <summary>
    /// Main logic to access API directly.
    /// </summary>
    public interface IApiOrderLogic<T, K>
    {
        /// <summary>
        /// Process order and gets result.
        /// </summary>
        /// <param name="orderInfo">Specific order.</param>
        /// <returns>Order processing result.</returns>
        /// <exception cref="ArgumentNullException">Thrown if parameters are null.</exception>
        Task<K> GetProcessedOrderResultAsync(T orderInfo);
    }
}
