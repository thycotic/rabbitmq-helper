using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class UninstallPriorRabbitMqCommand : ConsoleCommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof (UninstallPriorRabbitMqCommand));

        public override string Name
        {
            get { return "uninstallPriorRabbitMq"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Uninstalls prior installation of RabbitMq"; }
        }

        public UninstallPriorRabbitMqCommand()
        {

            Action = parameters =>
            {
                return 0;
            };
        }
    }
}