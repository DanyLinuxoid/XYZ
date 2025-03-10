using XYZ.Models.Features.Billing.Data;
using XYZ.Web.Features.Billing.WebRequests;

namespace XYZ.Web.Features.Billing.Mappers
{
    /// <summary>
    /// Mapper for Dto/Dbo/Web relationships.
    /// </summary>
    public static class BillingOrderProcessingMapper
    {
        /// <summary>
        /// Maps web request to logic object.
        /// </summary>
        /// <param name="billingOrderProcessingRequest">Web request.</param>
        /// <returns>Logic object.</returns>
        public static OrderInfo ToOrderInfo(this BillingOrderProcessingRequest billingOrderProcessingRequest)
        {
            return new OrderInfo()
            {
                Description = billingOrderProcessingRequest.Description,
                OrderNumber = billingOrderProcessingRequest.OrderNumber,
                PayableAmount = billingOrderProcessingRequest.PayableAmount,
                PaymentGateway = billingOrderProcessingRequest.PaymentGateway,
                UserId = billingOrderProcessingRequest.UserId,
            };
        }
    }
}
