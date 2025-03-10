using Microsoft.AspNetCore.Mvc;

using XYZ.Logic.Common.Interfaces;
using XYZ.Web.Common.Base;

namespace XYZ.Web.Features.User.Controllers
{
    /// <summary>
    /// User related actions.
    /// </summary>
    [ApiController]
    [Route("api/user")]
    public class UserController : ApiControllerBase
    {
        /// <summary>
        /// User related logic.
        /// </summary>
        private readonly IUserLogic _userLogic;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="userLogic">User related logic.</param>
        public UserController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        /// <summary>
        /// Create new user without parameters.
        /// </summary>
        /// <returns>User identifier.</returns>
        [HttpPost("create-new-user")]
        public async Task<IActionResult> CreateUser()
        {
            var result = await _userLogic.CreateUser();
            return CreatedAtAction(nameof(CreateUser), new { id = result });
        }
    }
}
