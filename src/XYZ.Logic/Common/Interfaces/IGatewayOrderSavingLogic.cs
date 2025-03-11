using XYZ.DataAccess.Tables.Base;

namespace XYZ.Logic.Common.Interfaces
{
    /// <summary>
    /// Gateway specific order saving logic.
    /// </summary>
    public interface IGatewayOrderSavingLogic<T> where T : TABLE_BASE, new()
    {
        /// <summary>
        /// Saves gateway specific order.
        /// </summary>
        /// <returns>Saved order main identifier.</returns>
        Task<long> SaveOrder(T model);
    }
}
