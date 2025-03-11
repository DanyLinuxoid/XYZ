using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.Enums;

namespace XYZ.Logic.Billing.Factory
{
    /// <summary>
    /// Main billing gateway factory class for payment APIs.
    /// </summary>
    public class BillingGatewayFactory : IBillingGatewayFactory
    {
        /// <summary>
        /// Payment APIs container.
        /// </summary>
        private readonly Dictionary<PaymentGatewayType, IPaymentGatewayLogic> _paymentGateways;

        /// <summary>
        /// Main billing gateway factory constructor for payment APIs.
        /// </summary>
        /// <param name="gatewayLogics">Collection of registered payment gateways.</param>
        public BillingGatewayFactory(IEnumerable<IPaymentGatewayLogic> gatewayLogics)
        {
            if (gatewayLogics == null) 
                throw new ArgumentNullException(nameof(gatewayLogics));

            _paymentGateways = gatewayLogics.ToDictionary(g => g.GatewayType);
        }

        /// <summary>
        /// Gets gateway logic based on type.
        /// </summary>
        /// <param name="gatewayType">Gateway API type.</param>
        /// <returns>Related gateway.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If gateway value not found in enum.</exception>
        public IPaymentGatewayLogic GetPaymentGateway(PaymentGatewayType gatewayType)
        {
            if (_paymentGateways.TryGetValue(gatewayType, out var gatewayLogic))
                return gatewayLogic;

            throw new ArgumentOutOfRangeException(nameof(gatewayType), $"Unknown gateway type: {gatewayType}");
        }
    }
}