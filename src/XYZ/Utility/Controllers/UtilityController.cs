using Microsoft.AspNetCore.Mvc;

using XYZ.Logic.Common.Interfaces;

namespace XYZ.Web.Utility.Controllers
{
    /// <summary>
    /// Controller for utilities and service/stability checks.
    /// </summary>
    [ApiController]
    [Route("api/utility")]
    public class UtilityController : Controller
    {
        private ISmokeTestLogic _smokeTestLogic;

        public UtilityController(ISmokeTestLogic smokeTestLogic)
        {
            _smokeTestLogic = smokeTestLogic;
        }

        /// <summary>
        /// Simple ping.
        /// </summary>
        /// <returns>Ok if service available.</returns>
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        /// <summary>
        /// Various methods to smoketest system and check if it is operational.
        /// NOTE: This should not be available for public/API use, auth/internal only, but we will skip this part for the sake of ease/access.
        /// </summary>
        /// <returns>Boolean stating if system is ready.</returns>
        [HttpPost("smoketest")] 
        public async Task<IActionResult> SmokeTest()
        {
            var result = await _smokeTestLogic.IsSystemFunctional();
            return Ok(new
            {
                IsSystemFunctional = result,
            });
        }
    }
}
