using XYZ.DataAccess.Base;
using XYZ.DataAccess.Interfaces;

namespace XYZ.DataAccess.Tables.APP_ERROR_TABLE
{
    /// <summary>
    /// Repository of CUD commands + async version.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public class APP_ERROR_CUD : CommandsBase<APP_ERROR>, ICommandRepository<APP_ERROR>
    {
    }
}
