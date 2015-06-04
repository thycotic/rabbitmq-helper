using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class CopyRabbitMqExampleConfigFileCommand : ConsoleCommandBase
    {
        
        private readonly ILogWriter _log = Log.Get(typeof (CopyRabbitMqExampleConfigFileCommand));

        public override string Name
        {
            get { return "copyRabbitMqExampleConfigFile"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Copies RabbitMq example configuration file"; }
        }

        public CopyRabbitMqExampleConfigFileCommand()
        {

            Action = parameters =>
            {
                return 0;
            };
        }
    }
}