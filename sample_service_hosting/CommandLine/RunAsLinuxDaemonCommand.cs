using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;

namespace VSC.CommandLine
{
    [Command(Name = "run-daemon", Description = "Run as Linux Daemon")]
    public class RunAsLinuxDaemonCommand
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
                await builder.Build().RunAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("Could not start as Linux daemon service. " 
                    + ex.Message);
                app.ShowHelp();
            }

            return 0;
        }
    }
}