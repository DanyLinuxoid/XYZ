using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.ORDER_TABLE;
using XYZ.DataAccess.Tables.ORDER_TBL.Queries;
using XYZ.Logic.Common.Interfaces;
using XYZ.Logic.Features.Billing.Mappers;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Features.Billing.Common
{
    public class OrderLogic : IOrderLogic
    {
        private IDatabaseLogic _databaseLogic;

        public OrderLogic(IDatabaseLogic databaseLogic)
        {
            _databaseLogic = databaseLogic;
        }

        public async Task<OrderDto?> GetOrderAsync(long userId, long orderNumber)
        {
            var result = await _databaseLogic.QueryAsync(new OrderByOrderNumberAndUserIdGetQuery(userId, orderNumber));
            return result == null ? null : result.ToDto();
        }

        public async Task<long> SaveOrder(OrderDto? billingOrder)
        {
            if (billingOrder == null)
                throw new ArgumentNullException(nameof(billingOrder));

            return await _databaseLogic.CommandAsync(new ORDER_CUD(), CommandTypes.Create, billingOrder.ToDbo());
        }

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
