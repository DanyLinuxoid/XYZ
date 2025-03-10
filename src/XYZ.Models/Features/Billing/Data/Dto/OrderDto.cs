using XYZ.Models.Common.Enums;

namespace XYZ.Models.Features.Billing.Data.Dto
{
    public class OrderDto
    {
        public decimal PayableAmount { get; set; }

        public string? Description { get; set; }

        public long OrderNumber { get; set; }

        public long UserId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public long? PaypalOrderId { get; set; }

        public long? PayseraOrderId { get; set; }
    }
}
