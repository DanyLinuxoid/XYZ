using XYZ.Models.Common.Enums;

namespace XYZ.Models.Features.Billing.Data.Dto
{
    /// <summary>
    /// Receipt that is directly mapped to Dbo.
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// Amount that was paid for order.
        /// </summary>
        public decimal PayableAmount { get; set; }

        /// <summary>
        /// Order optional description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Order number (not main identfier).
        /// </summary>
        public long OrderNumber { get; set; }

        /// <summary>
        /// User main identifier.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Order status result.
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// Paypal order id, is filled if was processed by paypal.
        /// </summary>
        public long? PaypalOrderId { get; set; }

        /// <summary>
        /// paysera order id, is filled if was processed by paysera.
        /// </summary>
        public long? PayseraOrderId { get; set; }
    }
}
