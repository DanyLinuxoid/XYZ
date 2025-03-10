namespace XYZ.Models.Features.Billing.Data.Order
{
    /// <summary>
    /// Order base that is shared by all API's.
    /// </summary>
    public class OrderBase
    {
        /// <summary>
        /// User main identifier.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Order number (not main identier).
        /// </summary>
        public long OrderNumber { get; set; }

        /// <summary>
        /// Optional description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Amount that was paid for order.
        /// </summary>
        public decimal PayableAmount { get; set; }
    }
}
