using Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Api.ConfigurationExtension
{
    public static class ConfigurationExtension
    {
        public static void UseSwaggerAndUI(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerConfig = new SwaggerConfiguration();
            //Binds the configuration json to swaggerConfig
            configuration.Bind(nameof(SwaggerConfiguration), swaggerConfig);

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseSwagger(
                option => { option.RouteTemplate = swaggerConfig.JsonRoute; }
            );

            app.UseSwaggerUI(
                option => { option.SwaggerEndpoint(swaggerConfig.UiEndpoint, swaggerConfig.Description); });
        }
    }
}