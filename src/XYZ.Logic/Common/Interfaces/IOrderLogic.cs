using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Common.Interfaces
{
    /// <summary>
    /// Main class for order CRUD.
    /// </summary>
    public interface IOrderLogic
    {
        /// <summary>
        /// Gets order associated with user by user main identifier and order number.
        /// </summary>
        /// <param name="userId">User main identifier.</param>
        /// <param name="orderNumber">Order number.</param>
        /// <returns></returns>
        Task<OrderDto?> GetOrderAsync(long userId, long orderNumber);

        /// <summary>
        /// Saves order associated with user.
        /// </summary>
        /// <param name="billingOrder">Order object.</param>
        /// <returns>Saved order id.</returns>
        /// <exception cref="ArgumentNullException">Thrown if parameter is nulll.</exception>
        Task<long> SaveOrder(OrderDto? billingOrder);

        /// <summary>
        /// Updates order fields in database with new provided.
        /// </summary>
        /// <param name="billingOrder">Order object.</param>
        /// <exception cref="ArgumentNullException">Thrown if parameters are null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if order for update was not found in database.</exception>
        Task UpdateOrder(OrderDto? billingOrder);
    }
}