using XYZ.Models.Common.Enums;
using XYZ.Models.Common.ExceptionHandling;
using XYZ.Models.Features.Billing.Validation;

namespace XYZ.Models.Features.Billing.Data.Order.Order
{
    /// <summary>
    /// Order result that comes from 3-rd party API.
    /// </summary>
    public class OrderResult
    {
        /// <summary>
        /// If order result is successfull.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Transaction id associated with this order and -rd party API that processed this order.
        /// </summary>
        public string GatewayTransactionId { get; set; }

        /// <summary>
        /// Order validation container with field->error mapping.
        /// </summary>
        public OrderValidationResult OrderValidationResult { get; set; } = new OrderValidationResult();

        /// <summary>
        /// Additional details that come from 3-rd party API or from our server if order was not processed.
        /// </summary>
        public ProblemDetailed? ProblemDetails { get; set; }

        /// <summary>
        /// Simple order status result.
        /// </summary>

        public OrderStatus OrderStatus { get; set; }
    }
}
