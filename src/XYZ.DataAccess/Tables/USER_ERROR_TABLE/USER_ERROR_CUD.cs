using XYZ.DataAccess.Base;
using XYZ.DataAccess.Interfaces;

namespace XYZ.DataAccess.Tables.USER_ERROR_TABLE
{
    /// <summary>
    /// Repository of CUD commands + async version.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public class USER_ERROR_CUD : CommandsBase<USER_ERROR>, ICommandRepository<USER_ERROR>
    {
    }
}
