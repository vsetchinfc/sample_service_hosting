using System;
using System.Threading.Tasks;
using NLog;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using System.Threading;
using VSC.CommandLine;

namespace VSC
{
    [Command(name: "sample_service_hosting")]
    [Subcommand(typeof(InstallAsWindowsServiceCommand))]
    [Subcommand(typeof(UninstallWindowsService))]
    [Subcommand(typeof(RunAsConsoleAppCommand))]
    [Subcommand(typeof(RunAsLinuxDaemonCommand))]
    [HelpOption("-? | --help")]
    class Program
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        static async Task Main(string[] args)
        {
            HostedService.Args = args;
            WinService.WinServiceName = "The Sample Service Host";
            WinServiceInstaller.WinServiceStatus
                += new WinServiceInstaller.WinServiceStatusHandler(
                    PrintWinServiceStatus);

            _logger.Info("Version: "
                + Assembly.GetEntryAssembly().GetName().Version.ToString());

            WinServiceInstaller.APP_EXECUTABLE_PATH
                = Utility.GetExecutingAssemblyLocation()
                    .Remove(
                        Utility.GetExecutingAssemblyLocation().Length - 4
                    ) + ".exe";

            var app = await CommandLineApplication.ExecuteAsync<Program>(args);

            _logger.Info("Shutting down logger...");
            // Flush buffered log entries before program exit; 
            // then shutdown the logger before program exit.
            LogManager.Flush(TimeSpan.FromSeconds(15));
            LogManager.Shutdown();

            // ArgsParser argsParser = new ArgsParser(args);

            // WinService.WinServiceName = "The Sample Service Host";
            // WinServiceInstaller.WinServiceStatus
            //     += new WinServiceInstaller.WinServiceStatusHandler(
            //         PrintWinServiceStatus);

            // _logger.Info("Version: "
            //     + Assembly.GetEntryAssembly().GetName().Version.ToString());

            // HostedService service = new HostedService(argsParser);
            // await service.Run(args);

            // _logger.Info("Shutting down logger...");
            // // Flush buffered log entries before program exit; 
            // // then shutdown the logger before program exit.
            // LogManager.Flush(TimeSpan.FromSeconds(15));
            // LogManager.Shutdown();
        }

        private static void PrintWinServiceStatus(string status)
        {
            _logger.Info(status);
        }

        public async Task<int> OnExecuteAsync
        (
            CommandLineApplication app,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                _logger.Info("Starting as windows service...");
                var builder = HostedService.CreateHostBuilder();
                await builder.RunAsWindowsServiceAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(
                        "Could not start as windows service. "
                        + ex.Message);
                app.ShowHelp();
            }

            return 0;
        }
    }
}
