using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Hosting;

namespace VSC
{
    internal class HostedService
    {
        internal static string[] Args { get; set; }

        internal static IHostBuilder CreateHostBuilder()//string[] args)
        {
            try
            {
                var builder = new HostBuilder()
                    .UseNLog()
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("appsettings.json", optional: true);
                        config.AddJsonFile(
                            $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                            optional: true);
                        if(Args != null) 
                        {
                            config.AddCommandLine(Args);
                        }
                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<Services.SampleService>();
                        services.Configure<HostOptions>(option =>
                        {
                            option.ShutdownTimeout
                                = System.TimeSpan.FromSeconds(20);
                        });
                    })
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.AddConfiguration(
                            hostingContext.Configuration.GetSection("Logging")
                        );
                        logging.AddConsole();
                    });

                return builder;
            }
            catch { }

            return null;
        }
    }
}