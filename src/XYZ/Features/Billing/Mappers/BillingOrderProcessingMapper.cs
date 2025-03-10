using XYZ.Models.Features.Billing.Data;
using XYZ.Web.Features.Billing.WebRequests;

namespace XYZ.Web.Features.Billing.Mappers
{
    public static class BillingOrderProcessingMapper
    {
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
