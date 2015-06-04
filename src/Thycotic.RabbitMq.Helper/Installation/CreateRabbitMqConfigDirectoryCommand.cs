using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class CreateRabbitMqConfigDirectoryCommand : ConsoleCommandBase
    {
        
        private readonly ILogWriter _log = Log.Get(typeof (CreateRabbitMqConfigDirectoryCommand));

        public override string Name
        {
            get { return "createRabbitMqConfigDirectory"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Creates a RabbitMq configuration directory"; }
        }

        public CreateRabbitMqConfigDirectoryCommand()
        {

            Action = parameters =>
            {
                return 0;
            };
        }
    }
}