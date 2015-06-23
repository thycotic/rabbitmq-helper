using System;
using Thycotic.CLI;
using Thycotic.CLI.Commands;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Service.ConsoleCommands
{
    class BoostrapUpdateCommand : CommandBase, IImmediateCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(BoostrapUpdateCommand));

        public override string Name
        {
            get { return Program.SupportedSwitches.Boostrap; }
        }

        public override string Area {
            get { return "Update"; }
        }

        public override string Description
        {
            get { return "Bootstraps the update using a supplied MSI"; }
        }

        public BoostrapUpdateCommand()
        {
            
            Action = parameters =>
            {
                _log.Info("Beginning bootstrap");

                string msiPath;
                if (!parameters.TryGet("msiPath", out msiPath))
                {
                    throw new ArgumentException("MSI path required");
                }
                
                var eub = new EngineUpdateBootstrapper();

                eub.Bootstrap(msiPath);


                _log.Info("Boostrap completed");

                return 0;

            };
        }
    }
}
