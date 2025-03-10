using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data.Order;

namespace XYZ.Models.Features.Billing.Data
{
    /// <summary>
    /// Additional order info for our server.
    /// </summary>
    public class OrderInfo : OrderBase
    {
        /// <summary>
        /// Payment gateway/API that was used to process order.
        /// </summary>
        public PaymentGatewayType PaymentGateway { get; set; }

        /// <summary>
        /// Order status result.
        /// </summary>
        public OrderStatus OrderStatus { get; set; }
    }
}
