using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Api.Extensions
{
    public static class General
    {
        public static int GetUserIdByClaim(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return -1;
            }

            var claim = httpContext.User.Claims.Single(c => c.Type == "id");

            return int.Parse(claim.Value);
        }
    }
}