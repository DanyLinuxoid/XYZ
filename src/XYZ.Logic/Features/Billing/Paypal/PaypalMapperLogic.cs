using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.Api.Paypal;
using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Features.Billing.Paypal
{
    /// <summary>
    /// Paysera mapping logic for dto<->dbo.
    /// </summary>
    public class PaypalMapperLogic : IOrderMapperLogic<PaypalOrderInfo>
    {
        /// <summary>
        /// Maps generic order info to paypal specific order.
        /// </summary>
        /// <param name="order">Generic order info.</param>
        /// <returns>Paysera order info.</returns>
        public PaypalOrderInfo ToMappedOrderInfo(OrderInfo order)
        {
            return new PaypalOrderInfo()
            {
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                Description = order.Description,
                PayableAmount = order.PayableAmount,
                PaymentGateway = PaymentGatewayType.PayPal,
                OrderStatus = order.OrderStatus,
            };
        }

        /// <summary>
        /// Maps Paypal order info to dto which later can be saved to db (after dbo map).
        /// </summary>
        /// <param name="order">Paypal specific order info.</param>
        /// <returns>Order generic dto.</returns>
        public OrderDto ToMappedOrderDto(PaypalOrderInfo order)
        {
            return new OrderDto()
            {
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                Description = order.Description,
                PayableAmount = order.PayableAmount,
                OrderStatus = order.OrderStatus,
            };
        }
    }
}