using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters.RecipieFilters
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var errror = new
            {
                Error = $"EXPTION IN FILTER: {context.Exception.Message}",
            };
            context.Result = new ObjectResult(errror)
            {
                StatusCode = 500,
            };
            // Marca la excepcion como manejada, previene q se propague fuera
            // de este middleware
            context.ExceptionHandled = true;
        }
    }
}