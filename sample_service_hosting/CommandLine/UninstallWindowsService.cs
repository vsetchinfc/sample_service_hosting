using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace VSC.CommandLine
{
    [Command(Name = "uninstall", Description = "Uninstall Windows Service")]
    public class UninstallWindowsService
    {
        public async Task<int> OnExecuteAsync
        (
            CommandLineApplication app,
            CancellationToken cancellationToken = default
        )
        {
            WinServiceInstaller.Uninstall(WinService.WinServiceName);
            return 0;
        }
    }
}