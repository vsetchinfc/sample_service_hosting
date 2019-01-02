using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace VSC.Services
{
    public class SampleService : IHostedService, IDisposable
    {
        public void Dispose()
        {
            // TODO: Do service clean up here
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // TODO: Do work here

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // stop service processes here
            
            return Task.CompletedTask;
        }
    }
}