using System;
using System.Text;
using Api.Configuration;
using Api.Models;
using Api.Models.Identity.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Api.ServiceContainer
{
    public class IdentityServiceInstaller : IServiceInstaller
    {
        public void InstallService(IConfiguration configuration, IServiceCollection services)
        {
            //Identity
            var identityConfig = new IdentityConfiguration();
            configuration.Bind(nameof(IdentityConfiguration), identityConfig);
            services.AddIdentity<AppUser, AppRole>(option =>
                {
                    //User
                    option.User.RequireUniqueEmail = identityConfig.UniqueEmail;
                    //Pass
                    option.Password.RequireDigit = identityConfig.RequiredDigit;
                    option.Password.RequireLowercase = identityConfig.RequireLowercase;
                    option.Password.RequireNonAlphanumeric = identityConfig.RequireNonAlphanumeric;
                    option.Password.RequireUppercase = identityConfig.RequireUppercase;
                    option.Password.RequiredLength = identityConfig.RequiredLength;
                })
                .AddEntityFrameworkStores<RecipiesDbContext>();
            //JWT
            var jwtSettings = new JwtConfiguration();
            configuration.Bind(nameof(JwtConfiguration), jwtSettings);
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience =  jwtSettings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = true,
                ValidateAudience = true,
            };
            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });
            //To the container to use to validate tokens
            services.AddSingleton(tokenValidationParameters);
        }
    }
}