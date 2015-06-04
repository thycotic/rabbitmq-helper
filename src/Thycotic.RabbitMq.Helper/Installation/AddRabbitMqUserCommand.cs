using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class AddRabbitMqUserCommand : ConsoleCommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof(InstallRabbitMqCommand));

        public override string Name
        {
            get { return "addRabbitMqUserCommand"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Adds a basic user to RabbitMq"; }
        }

        public AddRabbitMqUserCommand()
        {

            Action = parameters =>
            {
                return 0;
            };
        }
    }
}