using System.ComponentModel.DataAnnotations;

namespace XYZ.Web.Features.Billing.WebRequests
{
    public class GetOrderInfoRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = "User identifier must be greater than 0.")]
        public long UserId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Order identifier must be greater than 0.")]
        public long OrderNumber { get; set; }
    }
}
