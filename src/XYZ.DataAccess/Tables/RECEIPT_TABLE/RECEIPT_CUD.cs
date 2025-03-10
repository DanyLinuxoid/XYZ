using XYZ.DataAccess.Base;
using XYZ.DataAccess.Interfaces;

namespace XYZ.DataAccess.Tables.RECEIPT_TABLE
{
    /// <summary>
    /// Repository of CUD commands + async version.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public class RECEIPT_CUD : CommandsBase<RECEIPT>, ICommandRepository<RECEIPT>
    {
    }
}
