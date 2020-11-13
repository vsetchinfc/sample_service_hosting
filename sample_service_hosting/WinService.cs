using System;
using System.Runtime.Versioning;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace VSC
{
    public class WinService : ServiceBase, IHostLifetime
    {
        public static string WinServiceName = "Default Service Name";
        private readonly TaskCompletionSource<object> _delayStart
            = new TaskCompletionSource<object>();

        private IHostApplicationLifetime ApplicationLifetime { get; }

        [SupportedOSPlatform("windows")]
        public WinService(IHostApplicationLifetime applicationLifetime)
        {
            ServiceName = WinServiceName;

            ApplicationLifetime = applicationLifetime ??
                throw new ArgumentNullException(nameof(applicationLifetime));
        }

        [SupportedOSPlatform("windows")]
        public void Start()
        {
            try
            {
                Run(this); // This blocks until the service is stopped.
                _delayStart.TrySetException(
                    new InvalidOperationException("Stopped without starting"));
            }
            catch (Exception ex)
            {
                _delayStart.TrySetException(ex);
            }

            this.OnStart(null);
        }

        [SupportedOSPlatform("windows")]
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Stop();
            return Task.CompletedTask;
        }

        [SupportedOSPlatform("windows")]
        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => _delayStart.TrySetCanceled());
            ApplicationLifetime.ApplicationStopping.Register(Stop);

            new Thread(Start).Start(); // Otherwise this would block and prevent IHost.StartAsync from finishing.
            return _delayStart.Task;
        }

        protected override void OnStart(string[] args)
        {
            _delayStart.TrySetResult(null);
            OnStart(args);
        }

        protected override void OnStop()
        {
            ApplicationLifetime.StopApplication();
            OnStop();
        }
    }
}