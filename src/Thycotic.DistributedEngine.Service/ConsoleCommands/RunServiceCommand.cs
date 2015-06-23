using System.Linq;
using System.ServiceProcess;
using Thycotic.CLI;
using Thycotic.CLI.Configuration;
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
                //pass all parameters to service
                parameters.AllKeys.ToList().ForEach(k => ConsoleConfigurationManager.AppSettings[k] = parameters[k]);

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
