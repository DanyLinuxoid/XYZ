using System.ComponentModel.DataAnnotations;

using XYZ.Models.Common.Enums;

namespace XYZ.Web.Features.Billing.WebRequests
{
    /// <summary>
    /// Billing/Order processing info.
    /// </summary>
    public class BillingOrderProcessingRequest
    {
        /// <summary>
        /// Amount to pay for order.
        /// </summary>
        [Required(ErrorMessage = "Payable amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Payable amount must be greater than 0.")]
        public decimal PayableAmount { get; set; }

        /// <summary>
        /// Order number (not identifier).
        /// </summary>
        [Required(ErrorMessage = "Order number is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "OrderNumber must be greater than 0.")]
        public long OrderNumber { get; set; }

        /// <summary>
        /// User main identifier.
        /// </summary>
        [Required(ErrorMessage = "User identifier is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public long UserId { get; set; }

        /// <summary>
        /// Payment gateway (Paysera, Paypal, etc).
        /// </summary>
        [Required(ErrorMessage = "Gateway is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Gateway cannot be Unknown.")] 
        public PaymentGatewayType PaymentGateway { get; set; }

        /// <summary>
        /// Optional description.
        /// </summary>
        public string? Description { get; set; } = null;
    }
}
