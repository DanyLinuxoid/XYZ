using Microsoft.AspNetCore.Mvc;

using XYZ.Logic.Common.Interfaces;
using XYZ.Web.Common.Base;

namespace XYZ.Web.Features.User.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ApiControllerBase
    {
        private readonly IUserLogic _userLogic;

        public UserController(IUserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [HttpPost("create-new-user")]
        public async Task<IActionResult> CreateUser()
        {
            var result = await _userLogic.CreateUser();
            return CreatedAtAction(nameof(CreateUser), new { id = result });
        }
    }
}
