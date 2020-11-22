using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;

namespace VSC.CommandLine
{
    [Command(Name = "run-console", Description = "Install as Windows Service")]
    public class RunAsConsoleAppCommand
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<int> OnExecuteAsync
        (
            CommandLineApplication app,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var builder = HostedService.CreateHostBuilder();
                await builder.RunConsoleAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("Could not run as console app. " + ex.Message);
                app.ShowHelp();
            }

            return 0;
        }
    }
}