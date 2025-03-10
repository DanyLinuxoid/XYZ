using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.PAYSERA_ORDER_TABLE;
using XYZ.Logic.Common.Interfaces;

namespace XYZ.Logic.Features.Billing.Paysera
{
    public class PayseraGatewayOrderSavingLogic : IPayseraGatewayOrderSavingLogic
    {
        private IDatabaseLogic _databaseLogic;

        public PayseraGatewayOrderSavingLogic(IDatabaseLogic databaseLogic)
        {
            _databaseLogic = databaseLogic;
        }

        public async Task<long> SaveOrder()
        {
            return await _databaseLogic.CommandAsync(new PAYSERA_GATEWAY_ORDER_CUD(), CommandTypes.Create, new PAYSERA_GATEWAY_ORDER());
        }
    }
}
