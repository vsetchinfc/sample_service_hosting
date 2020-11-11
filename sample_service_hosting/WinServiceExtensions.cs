using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VSC
{
    public static class WinServiceExtensions
    {
        internal static IHostBuilder UseServiceBaseLifetime
        (
            this IHostBuilder hostBuilder
        )
        {
            return hostBuilder.ConfigureServices((hostContext, services) =>
                services.AddSingleton<IHostLifetime, WinService>());
        }

        public static Task RunAsWindowsServiceAsync
        (
            this IHostBuilder hostBuilder,
            CancellationToken cancellationToken = default
        )
        {
            return hostBuilder.UseServiceBaseLifetime()
                .Build().RunAsync(cancellationToken);
        }
    }
}