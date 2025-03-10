using XYZ.Models.Common.Enums;
using XYZ.Models.Common.ExceptionHandling;
using XYZ.Models.Features.Billing.Validation;

namespace XYZ.Models.Features.Billing.Data.Order.Order
{
    public class OrderResult
    {
        public bool IsSuccess { get; set; }

        public string GatewayTransactionId { get; set; }

        public OrderValidationResult OrderValidationResult { get; set; } = new OrderValidationResult();

        public ProblemDetailed? ProblemDetails { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
