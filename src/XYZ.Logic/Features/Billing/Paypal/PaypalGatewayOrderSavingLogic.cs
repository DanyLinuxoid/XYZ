using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.PAYPAL_ORDER_TABLE;
using XYZ.Logic.Common.Interfaces;

namespace XYZ.Logic.Features.Billing.Paypal
{
    public class PaypalGatewayOrderSavingLogic : IPaypalGatewayOrderSavingLogic
    {
        private IDatabaseLogic _databaseLogic;

        public PaypalGatewayOrderSavingLogic(IDatabaseLogic databaseLogic)
        {
            _databaseLogic = databaseLogic;
        }

        public async Task<long> SaveOrder()
        {
            return await _databaseLogic.CommandAsync(new PAYPAL_GATEWAY_ORDER_CUD(), CommandTypes.Create, new PAYPAL_GATEWAY_ORDER());
        }
    }
}
