using XYZ.DataAccess.Interfaces;
using XYZ.Logic.Common.Interfaces;

namespace XYZ.Logic.System.SmokeTest
{
    public class SmokeTestLogic : ISmokeTestLogic
    {
        private readonly IDatabaseUtilityLogic _databaseUtilityLogic;

        public SmokeTestLogic(IDatabaseUtilityLogic databaseUtilityLogic)
        {
            _databaseUtilityLogic = databaseUtilityLogic;
        }

        public async Task<bool> IsSystemFunctional()
        {
            bool isDbFunctional = await _databaseUtilityLogic.PingAsync();
            return isDbFunctional;
        }
    }
}
