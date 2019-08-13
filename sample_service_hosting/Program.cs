using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Hosting;

namespace VSC
{
    class Program
    {   
        static async Task Main(string[] args)
        {
            ArgsParser hostAction = new ArgsParser(args);

            HostedService service = new HostedService(hostAction);
            await service.Run(args);
        }
    }
}
