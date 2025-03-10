namespace XYZ.Models.Features.Billing.Data.Dto
{
    /// <summary>
    /// Receipt that is directly mapped to Dbo.
    /// </summary>
    public class ReceiptDto
    {
        /// <summary>
        /// Receipt identifier (not main).
        /// </summary>
        public string ReceiptId { get; set; }

        /// <summary>
        /// User main identifier.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Order number.
        /// </summary>
        public long OrderNumber { get; set; }

        /// <summary>
        /// Transaction id associated with 3-rd party API that processed order.
        /// </summary>
        public string GatewayTransactionId { get; set; }
    }
}
