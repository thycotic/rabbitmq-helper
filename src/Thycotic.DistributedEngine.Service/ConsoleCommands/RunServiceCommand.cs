using System.ServiceProcess;
using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Service.ConsoleCommands
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

                //ConsoleConfigurationManager.AppSettings["EngineToServerCommunication.ConnectionString"] =
                //    "http://localhost/ihawu";

                //ConsoleConfigurationManager.AppSettings["EngineToServerCommunication.UseSsl"] = "false";
                //ConsoleConfigurationManager.AppSettings["EngineToServerCommunication.SiteId"] = "3";
                //ConsoleConfigurationManager.AppSettings["EngineToServerCommunication.OrganizationId"] = "1";

                var servicesToRun = new ServiceBase[]
                {
                    new EngineService()
                };

                ServiceBase.Run(servicesToRun);


                return 0;

            };
        }
    }
}
