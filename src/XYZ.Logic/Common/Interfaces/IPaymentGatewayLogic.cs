using XYZ.Models.Common.Enums;
using XYZ.Models.Features.Billing.Data;
using XYZ.Models.Features.Billing.Data.Order.Order;

namespace XYZ.Logic.Common.Interfaces
{
    /// <summary>
    /// Gateway specific logic.
    /// </summary>
    public interface IPaymentGatewayLogic
    {
        /// <summary>
        /// Specific gateway type
        /// </summary>
        PaymentGatewayType GatewayType { get; }

        /// <summary>
        /// Main gateway public access entrypoint.
        /// </summary>
        /// <param name="order">Generic order from logic.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Is thrown if null parameters.</exception>
        /// <exception cref="InvalidOperationException">Is thrown when order cannot be processed.</exception>
        Task<OrderResult> GetGatewayOrderProcessResultAsync(OrderInfo orderInfo);
    }
}
