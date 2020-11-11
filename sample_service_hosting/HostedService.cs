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
        private static NLog.Logger _logger
            = NLog.LogManager.GetCurrentClassLogger();

        private ArgsParser _argsParser = null;

        public HostedService(ArgsParser argsParser)
        {
            _argsParser = argsParser;
        }

        public async Task Run(string[] args)
        {
            if (_argsParser != null)
            {
                var builder = this.CreateHostBuilder(args);

                if (builder == null) return;

                WinServiceInstaller.APP_EXECUTABLE_PATH
                    = Utility.GetExecutingAssemblyLocation()
                        .Remove(
                            Utility.GetExecutingAssemblyLocation().Length - 4
                        ) + ".exe";

                switch (_argsParser.GetHostAction())
                {
                    case HostAction.InstallWinService:
                        {
                            WinServiceInstaller
                                .Install(WinService.WinServiceName);
                        }
                        break;
                    case HostAction.UninstallWinService:
                        {
                            WinServiceInstaller
                                .Uninstall(WinService.WinServiceName);
                        }
                        break;
                    case HostAction.RunWinService:
                        {
                            try
                            {
                                await builder
                                    .RunAsWindowsServiceAsync();
                            }
                            catch (Exception ex)
                            {
                                _logger.Error(
                                        "Could not start as windows service. "
                                        + ex.Message);
                                ShowUsage();
                            }
                        }
                        break;
                    case HostAction.RunConsole:
                        {
                            try
                            {
                                await builder.RunConsoleAsync();
                            }
                            catch (Exception ex)
                            {
                                _logger.Error(
                                    "Could not run as console app. "
                                    + ex.Message);
                                ShowUsage();
                            }
                        }
                        break;
                    case HostAction.RunLinuxDaemon:
                        {
                            try
                            {
                                await builder.Build().RunAsync();
                            }
                            catch (Exception ex)
                            {
                                _logger.Error(
                                    "Could not start as Linux daemon service. "
                                    + ex.Message);
                                ShowUsage();
                            }
                        }
                        break;
                    default:
                        {
                            ShowUsage();
                        }
                        break;
                }
            }
        }

        private void ShowUsage()
        {
            Console.WriteLine(
                string.Format("{0} [options]", WinService.WinServiceName)
            );
            Console.WriteLine("Options:\n"
                        + "  'no options'\tStart as Windows Service\n"
                        + "  -i\t\tInstall as Windows Service\n"
                        + "  -u\t\tUninstall Windows Service\n"
                        + "  -console\tRun as console app\n"
                        + "  -daemon\tRun as Linux daemon service\n"
                        + "  -h\t\tShow command line switch help\n");
        }
        private IHostBuilder CreateHostBuilder(string[] args)
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
                        config.AddCommandLine(args);
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