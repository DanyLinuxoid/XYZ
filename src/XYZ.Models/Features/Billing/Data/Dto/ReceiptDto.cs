namespace XYZ.Models.Features.Billing.Data.Dto
{
    public class ReceiptDto
    {
        public string ReceiptId { get; set; }

        public long UserId { get; set; }

        public long OrderNumber { get; set; }

        public string GatewayTransactionId { get; set; }
    }
}
