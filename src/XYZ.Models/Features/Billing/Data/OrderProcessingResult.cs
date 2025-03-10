using XYZ.Models.Common.ExceptionHandling;
using XYZ.Models.Features.Billing.Data.Dto;
using XYZ.Models.Features.Billing.Validation;

namespace XYZ.Models.Features.Billing.WebResults
{
    /// <summary>
    /// Result of order proces.
    /// </summary>
    public class OrderProcessingResult
    {
        /// <summary>
        /// Receipt if request was successfully handled.
        /// </summary>
        public ReceiptDto Receipt { get; set; }

        /// <summary>
        /// Order validation result container.
        /// </summary>
        public OrderValidationResult OrderValidationResult { get; set; } = new OrderValidationResult();

        /// <summary>
        /// Additional information that can be filled from server or from 3-rd party API during errors.
        /// </summary>
        public ProblemDetailed? ProblemDetails { get; set; }
    }
}
