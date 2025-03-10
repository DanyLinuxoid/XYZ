using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Order.Order;

namespace XYZ.Logic.Common.Interfaces
{
    public interface IPaymentGatewayLogic
    {
        Task<OrderResult> GetGatewayOrderProcessResultAsync(OrderInfo orderInfo);
    }
}
