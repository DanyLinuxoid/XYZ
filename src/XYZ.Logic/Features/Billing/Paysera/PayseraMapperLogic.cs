using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.Api.Paysera;
using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Features.Billing.Paysera
{
    /// <summary>
    /// Paysera mapping logic for dto<->dbo.
    /// </summary>
    public class PayseraMapperLogic : IOrderMapperLogic<PayseraOrderInfo>
    {
        /// <summary>
        /// Maps generic order info to paysera specific order.
        /// </summary>
        /// <param name="order">Generic order info.</param>
        /// <returns>Paysera order info.</returns>
        public PayseraOrderInfo ToMappedOrderInfo(OrderInfo order)
        {
            return new PayseraOrderInfo()
            {
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                Description = order.Description,
                PayableAmount = order.PayableAmount,
                PaymentGateway = PaymentGatewayType.Paysera,
            };
        }

        /// <summary>
        /// Maps Paysera order info to dto which later can be saved to db (after dbo map).
        /// </summary>
        /// <param name="order">Paysera specific order info.</param>
        /// <returns>Order generic dto.</returns>
        public OrderDto ToMappedOrderDto(PayseraOrderInfo order)
        {
            return new OrderDto()
            {
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                Description = order.Description,
                PayableAmount = order.PayableAmount,
            };
        }
    }
}
