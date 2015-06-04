using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class InstallRabbitMqCommand : ConsoleCommandBase
    {
        
        private readonly ILogWriter _log = Log.Get(typeof (InstallRabbitMqCommand));

        public override string Name
        {
            get { return "installRabbitMq"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Installs RabbitMq"; }
        }

        public InstallRabbitMqCommand()
        {

            Action = parameters =>
            {
                return 0;
            };
        }
    }
}
