using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.ServiceContainer
{
    public static class ServiceInstallerExtension
    {
        public static void AddInstallers(this IServiceCollection services, IConfiguration configuration)
        {
            //All the classes that implents IServiceInstaller
            var classesInstallers =
                typeof(Startup).Assembly.ExportedTypes.Where(x =>
                    typeof(IServiceInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

            //Get the Instances of IServiceInstaller
            var installers = classesInstallers.Select(x => Activator.CreateInstance(x)).Cast<IServiceInstaller>()
                .ToList();
            
            //Install services
            installers.ForEach(i => i.InstallService(configuration, services));
        }
    }
}