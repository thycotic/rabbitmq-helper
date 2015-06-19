using System.ServiceProcess;
using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.MemoryMq.SiteConnector.Service.ConsoleCommands
{
    class RunServiceCommand : ConsoleCommandBase, IImmediateConsoleCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(RunServiceCommand));

        public override string Name
        {
            get { return Program.SupportedSwitches.RunService; }
        }

        public override string Area
        {
            get { return "Core"; }
        }

        public override string Description
        {
            get { return "Runs the service"; }
        }

        public RunServiceCommand()
        {

            Action = parameters =>
            {

                //string server;
                //string username;
                //string password;
                //if (!parameters.TryGet("server", out server)) return;
                //if (!parameters.TryGet("username", out username)) return;
                //if (!parameters.TryGet("password", out password)) return;

                //ConsoleConfigurationManager.AppSettings["Pipeline.ConnectionString"] = "net.tcp://localhost:8672";
                //ConsoleConfigurationManager.AppSettings["Pipeline.UseSsl"] = "false";
                //ConsoleConfigurationManager.AppSettings["Pipeline.Thumbprint"] =
                //    "f1faa2aa00f1350edefd9490e3fc95017db3c897";

                var servicesToRun = new ServiceBase[]
                {
                    new SiteConnectorService(), 
                };

                ServiceBase.Run(servicesToRun);


                return 0;

            };
        }
    }
}
