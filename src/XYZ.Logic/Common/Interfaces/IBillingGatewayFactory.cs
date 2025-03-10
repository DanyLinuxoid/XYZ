using XYZ.Models.Common.Enums;

namespace XYZ.Logic.Common.Interfaces
{
    /// <summary>
    /// Main billing gateway factory class for payment API's.
    /// </summary>
    public interface IBillingGatewayFactory
    {
        /// <summary>
        /// Gets gateway logic based on type.
        /// </summary>
        /// <param name="gatewayType">Gateway API type.</param>
        /// <returns>Related gateway.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If gateway value not found in enum.</exception>
        IPaymentGatewayLogic GetPaymentGateway(PaymentGatewayType gatewayType);
    }
}