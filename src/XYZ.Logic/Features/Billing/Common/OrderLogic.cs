using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.ORDER_TABLE;
using XYZ.DataAccess.Tables.ORDER_TBL.Queries;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.Billing.Mappers;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Features.Billing.Common
{
    /// <summary>
    /// Main class for order CRUD.
    /// </summary>
    public class OrderLogic : IOrderLogic
    {
        /// <summary>
        /// Database access.
        /// </summary>
        private IDatabaseLogic _databaseLogic;

        /// <summary>
        /// Main constructor for order CRUD.
        /// </summary>
        public OrderLogic(IDatabaseLogic databaseLogic)
        {
            _databaseLogic = databaseLogic;
        }

        /// <summary>
        /// Gets order associated with user by user main identifier and order number.
        /// </summary>
        /// <param name="userId">User main identifier.</param>
        /// <param name="orderNumber">Order number.</param>
        /// <returns>Order result.</returns>
        public async Task<OrderDto?> GetOrderAsync(long userId, long orderNumber)
        {
            var result = await _databaseLogic.QueryAsync(new OrderByOrderNumberAndUserIdGetQuery(userId, orderNumber));
            return result == null ? null : result.ToDto();
        }

        /// <summary>
        /// Saves order associated with user.
        /// </summary>
        /// <param name="billingOrder">Order object.</param>
        /// <returns>Saved order id.</returns>
        /// <exception cref="ArgumentNullException">Thrown if parameter is nulll.</exception>
        public async Task<long> SaveOrder(OrderDto? billingOrder)
        {
            if (billingOrder == null)
                throw new ArgumentNullException(nameof(billingOrder));

            return await _databaseLogic.CommandAsync(new ORDER_CUD(), CommandTypes.Create, billingOrder.ToDbo());
        }

        /// <summary>
        /// Updates order fields in database with new provided.
        /// </summary>
        /// <param name="billingOrder">Order object.</param>
        /// <exception cref="ArgumentNullException">Thrown if parameters are null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if order for update was not found in database.</exception>
        public async Task UpdateOrder(OrderDto? billingOrder)
        {
            if (billingOrder == null)
                throw new ArgumentNullException(nameof(billingOrder));

            ORDER? order = await _databaseLogic.QueryAsync(new OrderByOrderNumberAndUserIdGetQuery(billingOrder.UserId, billingOrder.OrderNumber));
            if (order == null)
                throw new InvalidOperationException($"Order with id {billingOrder.OrderNumber} for user {billingOrder.UserId} not found for update");

            // Only some fields are allowed to be modified
            order.DESCRIPTION = billingOrder.Description;
            order.ORDER_STATUS = (int)billingOrder.OrderStatus;
            await _databaseLogic.CommandAsync(new ORDER_CUD(), CommandTypes.Update, order);
        }
    }
}
