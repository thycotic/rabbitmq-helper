using System.IO;
using Thycotic.CLI.Commands;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Installation;

namespace Thycotic.RabbitMq.Helper.Commands.Installation
{
    internal class CreateRabbitMqConfigDirectoryCommand : CommandBase, IImmediateCommand
    {
        
        private readonly ILogWriter _log = Log.Get(typeof (CreateRabbitMqConfigDirectoryCommand));

        public override string Area
        {
            get { return CommandAreas.Installation; }
        }

        public override string Description
        {
            get { return "Creates a RabbitMq configuration directory"; }
        }

        public CreateRabbitMqConfigDirectoryCommand()
        {

            Action = parameters =>
            {
                if (Directory.Exists(InstallationConstants.RabbitMq.ConfigurationPath))
                {
                    return 0;
                }

                _log.Info("Creating RabbitMq configuration folder");

                Directory.CreateDirectory(InstallationConstants.RabbitMq.ConfigurationPath);

                return 0;
            };
        }
    }
}