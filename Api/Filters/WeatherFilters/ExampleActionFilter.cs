using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters.WeatherFilters
{
    //Después del filtro de recursos, despues de model binding
    public class ExampleActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            Console.WriteLine("EXAMPLE ACTION FILTER: Executing");
        }
    }
}