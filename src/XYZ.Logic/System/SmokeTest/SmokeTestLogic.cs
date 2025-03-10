using XYZ.DataAccess.Interfaces;
using XYZ.Logic.Common.Interfaces;

namespace XYZ.Logic.System.SmokeTest
{
    /// <summary>
    /// System smoke test logic.
    /// </summary>
    public class SmokeTestLogic : ISmokeTestLogic
    {
        /// <summary>
        /// Database functionality and helpfull methods.
        /// </summary>
        private readonly IDatabaseUtilityLogic _databaseUtilityLogic;

        /// <summary>
        /// System smoke test constructor.
        /// </summary>
        /// <param name="databaseUtilityLogic"></param>
        public SmokeTestLogic(IDatabaseUtilityLogic databaseUtilityLogic)
        {
            _databaseUtilityLogic = databaseUtilityLogic;
        }

        /// <summary>
        /// Main method to determine if system is functional.
        /// </summary>
        /// <returns>Ok if system is functional, false if not.</returns>
        public async Task<bool> IsSystemFunctional()
        {
            bool isDbFunctional = await _databaseUtilityLogic.PingAsync();
            return isDbFunctional;
        }
    }
}
