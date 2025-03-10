using XYZ.DataAccess.Enums;
using XYZ.DataAccess.Interfaces;
using XYZ.DataAccess.Tables.PAYPAL_ORDER_TABLE;
using XYZ.Logic.Common.Interfaces;

namespace XYZ.Logic.Features.Billing.Paypal
{
    /// <summary>
    /// Gateway specific order saving logic.
    /// </summary>
    public class PaypalGatewayOrderSavingLogic : IPaypalGatewayOrderSavingLogic
    {
        /// <summary>
        /// Database access.
        /// </summary>
        private IDatabaseLogic _databaseLogic;

        /// <summary>
        /// Gateway specific order saving constructor.
        /// </summary>
        /// <param name="databaseLogic">Database access.</param>
        public PaypalGatewayOrderSavingLogic(IDatabaseLogic databaseLogic)
        {
            _databaseLogic = databaseLogic;
        }

        /// <summary>
        /// Saves gateway specific order.
        /// </summary>
        /// <returns>Saved order main identifier.</returns>
        public async Task<long> SaveOrder()
        {
            return await _databaseLogic.CommandAsync(new PAYPAL_GATEWAY_ORDER_CUD(), CommandTypes.Create, new PAYPAL_GATEWAY_ORDER());
        }
    }
}
