using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.Enums;

namespace XYZ.Logic.Billing.Factory
{
    /// <summary>
    /// Main billing gateway factory class for payment API's.
    /// </summary>
    public class BillingGatewayFactory : IBillingGatewayFactory
    {
        /// <summary>
        /// Payment API's container.
        /// </summary>
        private readonly Dictionary<PaymentGatewayType, IPaymentGatewayLogic> _gatewayLogics;

        /// <summary>
        /// Main billing gateway factory constructor for payment API's.
        /// </summary>
        /// <param name="paypalLogic">Paypal gateway processing logic.</param>
        /// <param name="payseraLogic">Paysera gateway processing logic.</param>
        public BillingGatewayFactory(
            IPaypalGatewayLogic paypalLogic, 
            IPayseraGatewayLogic payseraLogic)
        {
            _gatewayLogics = new Dictionary<PaymentGatewayType, IPaymentGatewayLogic>
            {
                { PaymentGatewayType.PayPal, paypalLogic },
                { PaymentGatewayType.Paysera, payseraLogic }
            };
        }

        /// <summary>
        /// Gets gateway logic based on type.
        /// </summary>
        /// <param name="gatewayType">Gateway API type.</param>
        /// <returns>Related gateway.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If gateway value not found in enum.</exception>
        public IPaymentGatewayLogic GetGatewayLogic(PaymentGatewayType gatewayType)
        {
            if (_gatewayLogics.TryGetValue(gatewayType, out var gatewayLogic))
                return gatewayLogic;

            throw new ArgumentOutOfRangeException(nameof(gatewayType), $"Unknown gateway type: {gatewayType}");
        }
    }
}
