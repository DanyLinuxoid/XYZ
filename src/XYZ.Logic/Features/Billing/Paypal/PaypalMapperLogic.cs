using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.Api.Paypal;
using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Features.Billing.Paypal
{
    public class PaypalMapperLogic : IOrderMapperLogic<PaypalOrderInfo>
    {
        public PaypalOrderInfo ToMappedOrderInfo(OrderInfo order)
        {
            return new PaypalOrderInfo()
            {
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                Description = order.Description,
                PayableAmount = order.PayableAmount,
                PaymentGateway = GatewayType.PayPal,
                OrderStatus = order.OrderStatus,
            };
        }

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