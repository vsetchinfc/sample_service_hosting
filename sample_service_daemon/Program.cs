using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Hosting;

namespace VSC
{
    class Program
    {
        static void Main(string[] args)
        {
            IHostBuilder hostBuilder = CreateHostBuilder(args);
            if(hostBuilder != null)
            {
                IHost host = hostBuilder.Build();
                host.RunAsync();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            try
            {
                // string workingDirectory = Path.GetDirectoryName(Utility.GetExecutingAssemblyLocation());
                // Directory.SetCurrentDirectory(workingDirectory);

                var builder = new HostBuilder()
                    .UseNLog()
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("appsettings.json", optional: true);
                        config.AddJsonFile(
                            $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", 
                            optional: true);
                        config.AddCommandLine(args);
                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<Services.SampleService>();
                        services.Configure<HostOptions>(option =>
                        {
                            option.ShutdownTimeout = System.TimeSpan.FromSeconds(20);
                        });
                    })
                    .ConfigureLogging((hostingContext, logging) => 
                    {
                        logging.ClearProviders();
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                    });

                return builder;
            }
            catch { }

            return null;
        }
    }
}
