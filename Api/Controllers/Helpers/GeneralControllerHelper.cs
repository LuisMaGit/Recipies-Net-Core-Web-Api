using Microsoft.AspNetCore.Http;

namespace Api.Controllers.Helpers
{
    public static class GeneralControllerHelper
    {
        public static string GetCreatedLocation(HttpContext httpContext, string action)
        {
            return  $"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToUriComponent()}/{action}";
        }
    }
}