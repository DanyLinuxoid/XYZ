namespace XYZ.Logic.Common.Interfaces
{
    /// <summary>
    /// Gateway specific order saving logic.
    /// </summary>
    public interface IGatewayOrderSavingLogic
    {
        /// <summary>
        /// Saves gateway specific order.
        /// </summary>
        /// <returns>Saved order main identifier.</returns>
        Task<long> SaveOrder();
    }
}
