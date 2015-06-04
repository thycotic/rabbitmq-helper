using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class EditRabbitMqConfigFileMqCommand : ConsoleCommandBase
    {
        
        private readonly ILogWriter _log = Log.Get(typeof (EditRabbitMqConfigFileMqCommand));

        public override string Name
        {
            get { return "editRabbitMqConfigFileMq"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Edits the RabbitMq configuration file"; }
        }

        public EditRabbitMqConfigFileMqCommand()
        {

            Action = parameters =>
            {



                return 0;
            };
        }
    }
}