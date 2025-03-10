using XYZ.DataAccess.Base;
using XYZ.DataAccess.Interfaces;

namespace XYZ.DataAccess.Tables.PAYPAL_ORDER_TABLE
{
    /// <summary>
    /// Repository of CUD commands + async version.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public class PAYPAL_GATEWAY_ORDER_CUD : CommandsBase<PAYPAL_GATEWAY_ORDER>, ICommandRepository<PAYPAL_GATEWAY_ORDER>
    {
    }
}
