using Api.Services.IdentityService;
using Api.Services.RecipieService;
using Api.Services.WeatherService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Api.ServiceContainer
{
    public class DataServiceInstaller : IServiceInstaller
    {
        public void InstallService(IConfiguration configuration, IServiceCollection services)
        {
            services.AddScoped<IWeatherService, WeatherService>();
            services.AddScoped<IRecipieService, RecipieService>();
            services.AddScoped<IIdentityService, IdentityService>();

            //MOCKS
            // services.AddTransient<IRecipieService, MockRecepieService>();
            // services.AddTransient<IWeatherService, MockWeatherService>();
        }
    }
}