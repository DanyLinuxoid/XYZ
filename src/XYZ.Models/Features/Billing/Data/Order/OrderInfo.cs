using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data.Order;

namespace XYZ.Models.Features.Billing.Data
{
    public class OrderInfo : OrderBase
    {
        public GatewayType PaymentGateway { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
