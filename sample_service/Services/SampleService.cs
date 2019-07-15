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
        IApplicationLifetime _appLifetime;
        ILogger<SampleService> _logger;
        IHostingEnvironment _environment;
        IConfiguration _configuration;

        public SampleService(
            IConfiguration configuration,
            IHostingEnvironment environment,
            ILogger<SampleService> logger, 
            IApplicationLifetime appLifetime
        )
        {
            _configuration = configuration;
            _environment = environment;
            _logger = logger;
            _appLifetime = appLifetime;
        }

        public void Dispose()
        {
            // TODO: Do service clean up here
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("SampleService StartAsync method called.");

            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("SampleService StopAsync method called.");
            
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogTrace("SampleService OnStarted method called.");

            // Post-startup code goes here
        }

        private void OnStopping()
        {
            _logger.LogTrace("SampleService OnStopping method called.");

            // On-stopping code goes here
        }

        private void OnStopped()
        {
            _logger.LogTrace("SampleService OnStopped method called.");

            // Post-stopped code goes here
        }
    }
}