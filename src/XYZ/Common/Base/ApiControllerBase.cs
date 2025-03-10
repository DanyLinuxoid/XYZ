using Microsoft.AspNetCore.Mvc;

namespace XYZ.Web.Common.Base
{
    public class ApiControllerBase : ControllerBase
    {
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
