using XYZ.Logic.Common.Interfaces;
using XYZ.Models.Common.Enums;

namespace XYZ.Logic.Billing.Factory
{
    public class BillingGatewayFactory : IBillingGatewayFactory
    {
        private readonly Dictionary<GatewayType, IPaymentGatewayLogic> _gatewayLogics;

        public BillingGatewayFactory(
            IPaypalGatewayLogic paypalLogic, 
            IPayseraGatewayLogic payseraLogic)
        {
            _gatewayLogics = new Dictionary<GatewayType, IPaymentGatewayLogic>
            {
                { GatewayType.PayPal, paypalLogic },
                { GatewayType.Paysera, payseraLogic }
            };
        }

        public IPaymentGatewayLogic GetGatewayLogic(GatewayType gatewayType)
        {
            if (_gatewayLogics.TryGetValue(gatewayType, out var gatewayLogic))
                return gatewayLogic;

            throw new ArgumentOutOfRangeException(nameof(gatewayType), $"Unknown gateway type: {gatewayType}");
        }
    }
}
