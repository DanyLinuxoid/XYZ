using XYZ.DataAccess.Base;
using XYZ.DataAccess.Interfaces;

namespace XYZ.DataAccess.Tables.USER_TABLE
{
    /// <summary>
    /// Repository of CUD commands + async version.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public class USER_CUD : CommandsBase<USER>, ICommandRepository<USER>
    {
    }
}
