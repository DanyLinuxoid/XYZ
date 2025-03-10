using Microsoft.AspNetCore.Mvc;

namespace XYZ.Web.Common.Base
{
    /// <summary>
    /// Base api controller with helpfull functions.
    /// </summary>
    public class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// Modified OkObjectResult with additional fields.
        /// </summary>
        /// <param name="result">Main result response from logic.</param>
        /// <returns>Modified OkObjectResult which contains additional info/fields.</returns>
        protected new IActionResult Ok(object? result)
        {
            return base.Ok(new
            {
                IsSuccess = true,
                Result = result
            });
        }
    }
}
