using XYZ.Models.Common.Enums;

namespace XYZ.Logic.Common.Interfaces
{
    public interface IBillingGatewayFactory
    {
        IPaymentGatewayLogic GetGatewayLogic(GatewayType gatewayType);
    }
}