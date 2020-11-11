using System.Diagnostics;

namespace VSC
{
    public static class WinServiceInstaller
    {
        private static NLog.Logger _logger
            = NLog.LogManager.GetCurrentClassLogger();

        public static string APP_EXECUTABLE_PATH = string.Empty;
        private const string ServiceControllerEXE = "sc.exe";

        public delegate void WinServiceStatusHandler(string status);
        public static event WinServiceStatusHandler WinServiceStatus;

        public static void Uninstall(string serviceName)
        {
            Stop(serviceName); // stop service before uninstall

            RaiseWinServiceStatus("Uninstall Service");

            RunProcess(string.Format("delete \"{0}\"", serviceName));
        }

        private static void Stop(string serviceName)
        {
            RaiseWinServiceStatus("Stopping Service");

            RunProcess(string.Format("stop \"{0}\"", serviceName));
        }

        public static void Install(string serviceName)
        {
            if (!string.IsNullOrEmpty(APP_EXECUTABLE_PATH))
            {
                RaiseWinServiceStatus("Install Service");

                string processArguments = string.Format(
                    "create \"{0}\" displayname= \"{1}\" binpath= \"{2}\"",
                    serviceName, serviceName, APP_EXECUTABLE_PATH);

                RunProcess(processArguments);
            }
            else
            {
                _logger.Error(
                    "Cannot install service. Path to exe cannot be empty.");
            }
        }

        private static void RaiseWinServiceStatus(string status)
        {
            if (WinServiceStatus != null)
            {
                WinServiceStatus(status);
            }
        }

        private static void RunProcess(string arguments)
        {
            _logger.Trace("Arguments: " + arguments);

            var process = new Process();
            var processInfo = new ProcessStartInfo();
            processInfo.WindowStyle
                = System.Diagnostics.ProcessWindowStyle.Hidden;
            processInfo.FileName = ServiceControllerEXE;
            processInfo.Arguments = arguments;
            process.StartInfo = processInfo;
            process.Start();
            process.WaitForExit();
        }
    }
}