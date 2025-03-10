using XYZ.DataAccess.Base;
using XYZ.DataAccess.Interfaces;

namespace XYZ.DataAccess.Tables.ORDER_TABLE
{
    /// <summary>
    /// Repository of CUD commands + async version.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public class ORDER_CUD : CommandsBase<ORDER>, ICommandRepository<ORDER>
    {
    }
}
