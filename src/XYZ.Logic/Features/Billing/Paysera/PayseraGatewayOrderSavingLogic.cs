using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.PAYSERA_ORDER_TABLE;
using XYZ.Logic.Common.Interfaces;

namespace XYZ.Logic.Features.Billing.Paysera
{
    /// <summary>
    /// Gateway specific order saving logic.
    /// </summary>
    public class PayseraGatewayOrderSavingLogic : IPayseraGatewayOrderSavingLogic
    {
        /// <summary>
        /// Database access.
        /// </summary>
        private IDatabaseLogic _databaseLogic;

        /// <summary>
        /// Gateway specific order saving constructor.
        /// </summary>
        /// <param name="databaseLogic">Database access.</param>
        public PayseraGatewayOrderSavingLogic(IDatabaseLogic databaseLogic)
        {
            _databaseLogic = databaseLogic;
        }

        /// <summary>
        /// Saves gateway specific order.
        /// </summary>
        /// <returns>Saved order main identifier.</returns>
        public async Task<long> SaveOrder()
        {
            return await _databaseLogic.CommandAsync(new PAYSERA_GATEWAY_ORDER_CUD(), CommandTypes.Create, new PAYSERA_GATEWAY_ORDER());
        }
    }
}
