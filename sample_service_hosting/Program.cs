using System.Threading.Tasks;

namespace VSC
{
    class Program
    {   
        static async Task Main(string[] args)
        {
            ArgsParser argsParser = new ArgsParser(args);

            HostedService service = new HostedService(argsParser);
            await service.Run(args);
        }
    }
}
