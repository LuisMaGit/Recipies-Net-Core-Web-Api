using System;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.ServiceContainer
{
    public class ExternalServiceInstaller : IServiceInstaller
    {
        public void InstallService(IConfiguration configuration, IServiceCollection services)
        {
            //EF
            services.AddDbContext<RecipiesDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("RecipiesSqlite")));
            //AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}