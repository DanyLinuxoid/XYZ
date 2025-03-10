using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.Api.Paysera;
using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Dto;

namespace XYZ.Logic.Features.Billing.Paysera
{
    public class PayseraMapperLogic : IOrderMapperLogic<PayseraOrderInfo>
    {
        public PayseraOrderInfo ToMappedOrderInfo(OrderInfo order)
        {
            return new PayseraOrderInfo()
            {
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                Description = order.Description,
                PayableAmount = order.PayableAmount,
                PaymentGateway = GatewayType.Paysera,
            };
        }

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
