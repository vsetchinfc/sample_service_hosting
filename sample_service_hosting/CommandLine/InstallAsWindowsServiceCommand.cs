using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace VSC.CommandLine
{
    [Command(Name = "install", Description = "Install as Windows Service")]
    public class InstallAsWindowsServiceCommand
    {
        public async Task<int> OnExecuteAsync
        (
            CommandLineApplication app,
            CancellationToken cancellationToken = default
        )
        {
            WinServiceInstaller.Install(WinService.WinServiceName);
            return 0;
        }
    }
}