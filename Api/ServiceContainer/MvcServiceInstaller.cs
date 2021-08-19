using Api.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Api.ServiceContainer
{
    public class MvcServiceInstaller : IServiceInstaller
    {
        public void InstallService(IConfiguration configuration, IServiceCollection services)
        {
            services.AddControllers();
            //Burned
            // var useRealValues = bool.Parse(_configuration["WeatherConfig:WeatherTestConfigurationValue"]);
            // Access configuration with IOptions<> interface
            services.Configure<WeatherConfiguration>(configuration.GetSection(nameof(WeatherConfiguration)));
            services.Configure<JwtConfiguration>(configuration.GetSection(nameof(JwtConfiguration)));
            //Swagger
            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Dommy api",
                    Version = "v1"
                });

                //Adding Authorization Header
                cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Bearer space token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }
    }
}