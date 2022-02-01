using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(AddAppConfiguration)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        public static void AddAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            var env = hostingContext.HostingEnvironment; // se configura en el launch.json
            Console.WriteLine(env.EnvironmentName);
            //Depende del orden 
            config
                .AddJsonFile(path: "appsettings.json", optional: true)
                // .AddJsonFile(path: $"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}