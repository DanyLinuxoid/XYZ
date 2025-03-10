using System.ComponentModel.DataAnnotations;

using XYZ.Models.Common.Enums;

namespace XYZ.Web.Features.Billing.WebRequests
{
    public class BillingOrderProcessingRequest
    {
        [Required(ErrorMessage = "Payable amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Payable amount must be greater than 0.")]
        public decimal PayableAmount { get; set; }

        [Required(ErrorMessage = "Order number is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "OrderNumber must be greater than 0.")]
        public long OrderNumber { get; set; }

        [Required(ErrorMessage = "User identifier is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "Gateway is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Gateway cannot be Unknown.")] 
        public GatewayType PaymentGateway { get; set; }

        public string? Description { get; set; } = null;
    }
}
