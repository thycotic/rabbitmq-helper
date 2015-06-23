using System.Linq;
using System.ServiceProcess;
using Thycotic.CLI;
using Thycotic.CLI.Commands;
using Thycotic.CLI.Configuration;
using Thycotic.Logging;

namespace Thycotic.MemoryMq.SiteConnector.Service.ConsoleCommands
{
    class RunServiceCommand : CommandBase, IImmediateCommand
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
                parameters.AllKeys.ToList().ForEach(k => CommandLineConfigurationManager.AppSettings[k] = parameters[k]);

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
