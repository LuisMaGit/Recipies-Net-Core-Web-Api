using System.Threading.Tasks;
using Api.Services.RecipieService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters.RecipieFilters
{
    public class EnsureRecipieExistsLogic : IAsyncActionFilter
    {
        private readonly IRecipieService _recipieService;

        public EnsureRecipieExistsLogic(IRecipieService recipieService)
        {
            _recipieService = recipieService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Before action
            var id = (int) context.ActionArguments["id"];
            var exits = await _recipieService.DoesRecipieExistsAsync(id);
            if (exits == false)
            {
                var error = new
                {
                    Error = "Recipie dont exists"
                };
                context.Result = new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
                return;
            }
            //Continue with the pipiline
            await next();
        }
    }

    //Atributo, delega la logica a otra clase, de esta forma se puede utilizar DI por constructor
    public class EnsureRecipieExistsFilter : TypeFilterAttribute
    {
        public EnsureRecipieExistsFilter() : base(typeof(EnsureRecipieExistsLogic))
        {
        }
    }
}