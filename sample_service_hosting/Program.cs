using System;
using System.Threading.Tasks;
using NLog;
using System.Reflection;

namespace VSC
{
    class Program
    {   
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        static async Task Main(string[] args)
        {
            ArgsParser argsParser = new ArgsParser(args);

            WinService.WinServiceName = "The Sample Service Host";
            WinServiceInstaller.WinServiceStatus += new WinServiceInstaller.WinServiceStatusHandler(PrintWinServiceStatus);

            _logger.Info("Version: " + Assembly.GetEntryAssembly().GetName().Version.ToString());

            HostedService service = new HostedService(argsParser);
            await service.Run(args);

            _logger.Info("Shutting down logger...");
            // Flush buffered log entries before program exit; then shutdown the logger before program exit.
            LogManager.Flush(TimeSpan.FromSeconds(15));
            LogManager.Shutdown();
        }

        private static void PrintWinServiceStatus(string status)
        {
            _logger.Info(status);
        }
    }
}
