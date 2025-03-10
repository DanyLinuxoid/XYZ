using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using XYZ.Web.Common.Attributes;

namespace XYZ.Web.Common.Filters
{
    public class ValidateModelFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var hasValidateAttribute = context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(ValidateModelAttribute));

            if (hasValidateAttribute || context.ModelState.IsValid)
            {
                await next();
                return;
            }

            var errors = context.ModelState
                .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

            var result = new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                title = "Validation Error",
                status = StatusCodes.Status400BadRequest,
                errors = errors
            };

            context.Result = new BadRequestObjectResult(result);
        }
    }
}
