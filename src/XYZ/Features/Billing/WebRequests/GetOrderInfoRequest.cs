using System.ComponentModel.DataAnnotations;

namespace XYZ.Web.Features.Billing.WebRequests
{
    /// <summary>
    /// Request to get order associated with user.
    /// </summary>
    public class GetOrderInfoRequest
    {
        /// <summary>
        /// User main identifier.
        /// </summary>
        [Range(1, long.MaxValue, ErrorMessage = "User identifier must be greater than 0.")]
        public long UserId { get; set; }

        /// <summary>
        /// Order number (not identifier).
        /// </summary>
        [Range(1, long.MaxValue, ErrorMessage = "Order identifier must be greater than 0.")]
        public long OrderNumber { get; set; }
    }
}
