using System.Threading.Tasks;
using Api.Extensions;
using Api.Services.RecipieService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters.RecipieFilters
{
    public class EnsureUserOwnsRecipie : IAsyncActionFilter
    {
        private readonly IRecipieService _recipiesService;

        public EnsureUserOwnsRecipie(IRecipieService recipiesService)
        {
            _recipiesService = recipiesService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //GET USE ID
            var userId = context.HttpContext.GetUserIdByClaim();
            //GET RECIPIE ID
            var recipieId = (int) context.ActionArguments["id"];
            //FIND IF USER OWNS RECIPIE
            var ownsRecipie = await _recipiesService.UserOwnsRecipie(userId, recipieId);
            // HANDLE USE DONT OWN RECIPIE
            if (!ownsRecipie)
            {
                var error = new
                {
                    Error = "You dont own these recipie"
                };
                context.Result = new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                };
                return;
            }
            //CONTINUE WITH THE PIPELINE
            await next();
        }
    }

    public class EnsureUserOwnsRecipieFilter : TypeFilterAttribute
    {
        public EnsureUserOwnsRecipieFilter() : base(typeof(EnsureUserOwnsRecipie))
        {
        }
    }
}