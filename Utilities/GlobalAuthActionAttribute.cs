using AuthSystem.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AuthSystem.Utilities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class GlobalAuthActionAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var skipGlobalAuth = context.ActionDescriptor.EndpointMetadata
                .OfType<SkipGlobalAuthActionAttribute>().Any();

            if (skipGlobalAuth)
            {
                await next();
                return;
            }

            var httpContext = context.HttpContext;

            if (!httpContext.User.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new JsonResult(new ServiceResponse<string>
                {
                    HasError = true,
                    Message = "Unauthorized user.",
                    HttpStatusCode = System.Net.HttpStatusCode.Unauthorized

                });
            }

            var claims = httpContext.User.Identity as ClaimsIdentity;

            if (claims == null || !claims.Claims.Any())
            {
                context.Result = new JsonResult(new ServiceResponse<string>
                {
                    HasError = true,
                    Message = "Unauthorized user.",
                    HttpStatusCode = System.Net.HttpStatusCode.Unauthorized
                });
            }

            if (claims != null)
            {
                httpContext.Items["UserName"] = claims.FindFirst("UserName")?.Value;
                httpContext.Items["Email"] = claims.FindFirst("Email")?.Value;
            }

            try
            {
                await next();
            }
            catch (Exception ex)
            {
                context.Result = new JsonResult(new ServiceResponse<string>
                {
                    HasError = true,
                    Message = $"An error occurred while processing the request: {ex.Message}",
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError
                });
            }
        }
    }
}
