using System;
using Thycotic.CLI.Commands;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Service.Commands
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

                
                string updatePath;
                if (!parameters.TryGet("updatePath", out updatePath))
                {
                    throw new ArgumentException("Update path required");
                }

                bool isLegacyAgent;
                parameters.TryGetBoolean("isLegacyAgent", out isLegacyAgent);
                
                var eub = new EngineUpdateBootstrapper();

                eub.Bootstrap(updatePath, isLegacyAgent);


                _log.Info("Boostrap completed");

                return 0;

            };
        }
    }
}
