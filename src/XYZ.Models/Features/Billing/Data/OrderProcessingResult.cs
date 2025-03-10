using XYZ.Models.Common.ExceptionHandling;
using XYZ.Models.Features.Billing.Data.Dto;
using XYZ.Models.Features.Billing.Validation;

namespace XYZ.Models.Features.Billing.WebResults
{
    public class OrderProcessingResult
    {
        public ReceiptDto Receipt { get; set; }

        public OrderValidationResult OrderValidationResult { get; set; } = new OrderValidationResult();

        public ProblemDetailed? ProblemDetails { get; set; }
    }
}
