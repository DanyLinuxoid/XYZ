namespace XYZ.Models.Features.Billing.Data.Order
{
    public class OrderBase
    {
        public long UserId { get; set; }

        public long OrderNumber { get; set; }

        public string? Description { get; set; }

        public decimal PayableAmount { get; set; }
    }
}
