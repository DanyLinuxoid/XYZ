using XYZ.Models.Common.ExceptionHandling;
using XYZ.Models.Features.Billing.Validation;

namespace XYZ.Logic.Common.Interfaces
{
    public interface IOrderResult
    {
        bool IsSuccess { get; }
        string TransactionId { get; }
        OrderValidationResult OrderValidationResult { get; }
        ProblemDetailed ProblemDetails { get; }
    }
}
