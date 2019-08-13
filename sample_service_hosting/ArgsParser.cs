namespace VSC
{
    internal class ArgsParser
    {
        private string[] _args = null;
        public ArgsParser(string[] args)
        {
            _args = args;
        }

        public HostAction GetHostAction()
        {
            HostAction action = HostAction.ShowUsage;

            if(_args == null || _args.Length == 0)
            {
                action = HostAction.RunWinService;
            }
            else if(_args.Length > 1)
            {
                action = HostAction.ShowUsage;
            }
            else
            {
                string argument = _args[0];

                if(argument == "-i")    // install
                {
                    action = HostAction.InstallWinService;
                }
                else if(argument == "-u")   // uninstall
                {
                    action = HostAction.UninstallWinService;
                }
                else if(argument == "-console")
                {
                    action = HostAction.RunConsole;
                }
                else if (argument == "-daemon")
                {
                    action = HostAction.RunLinuxDaemon;
                }
            }

            return action;
        }
    }
}