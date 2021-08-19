using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters.WeatherFilters
{
    //Después del filtro de autorización
    public class ExampleResourceFilter : Attribute, IResourceFilter
    {
        private readonly bool _isEnabled;

        public ExampleResourceFilter(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }


        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine($"EXAMPLE RESOURCE FILTER: Executing, isEnabled: {_isEnabled}");
            if (!_isEnabled)
            {
                context.Result = new BadRequestResult();
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine("EXAMPLE RESOURCE FILTER: Executed");
        }
    }
}