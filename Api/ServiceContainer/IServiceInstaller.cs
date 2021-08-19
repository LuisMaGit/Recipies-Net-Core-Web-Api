using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.ServiceContainer
{
    public interface IServiceInstaller
    {
        void InstallService(IConfiguration configuration, IServiceCollection services);
    }
}