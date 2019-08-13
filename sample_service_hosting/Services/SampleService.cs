using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VSC.Services
{
    public class SampleService : IHostedService, IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource;
        private Task _executingTask;
        // IApplicationLifetime _appLifetime;
        ILogger<SampleService> _logger;
        // IHostingEnvironment _environment;
        // IConfiguration _configuration;

        public SampleService(
            // IConfiguration configuration,
            // IHostingEnvironment environment,
            ILogger<SampleService> logger//, 
            // IApplicationLifetime appLifetime
        )
        {
            // _configuration = configuration;
            // _environment = environment;
            _logger = logger;
            // _appLifetime = appLifetime;
        }

        public void Dispose()
        {
            // TODO: Do service clean up here
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("SampleService StartAsync method called.");

            // _appLifetime.ApplicationStarted.Register(OnStarted);
            // _appLifetime.ApplicationStopping.Register(OnStopping);
            // _appLifetime.ApplicationStopped.Register(OnStopped);

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _executingTask = Run(_cancellationTokenSource.Token);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("SampleService StopAsync method called.");

            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            // stop service processes here
            _cancellationTokenSource.Cancel();

            _logger.LogInformation("Sample Service stopped.");

            // Wait until the task completes or the stop token triggers
            await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));
        }

        // private void OnStarted()
        // {
        //     _logger.LogTrace("SampleService OnStarted method called.");

        //     // Post-startup code goes here
        // }

        // private void OnStopping()
        // {
        //     _logger.LogTrace("SampleService OnStopping method called.");

        //     // On-stopping code goes here
        // }

        // private void OnStopped()
        // {
        //     _logger.LogTrace("SampleService OnStopped method called.");

        //     // Post-stopped code goes here
        // }

        private async Task Run(CancellationToken cancellationToken)
        {
            _logger.LogTrace("Starting iteration count Run");

            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);

            int iterationCount = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                iterationCount++;

                _logger.LogInformation(string.Format("Running round {0}", iterationCount));

                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }
    }
}