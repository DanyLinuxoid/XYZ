using Microsoft.AspNetCore.Mvc;

using XYZ.Logic.Common.Interfaces;

namespace XYZ.Web.Utility.Controllers
{
    [ApiController]
    [Route("api/utility")]
    public class UtilityController : Controller
    {
        private ISmokeTestLogic _smokeTestLogic;

        public UtilityController(ISmokeTestLogic smokeTestLogic)
        {
            _smokeTestLogic = smokeTestLogic;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        [HttpPost("smoketest")] // This should not be available for public/API use, auth/internal only, but we will skip this part for the sake of ease/access.
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
