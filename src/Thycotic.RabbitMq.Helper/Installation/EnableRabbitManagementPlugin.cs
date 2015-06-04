using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class EnableRabbitManagementPlugin : ConsoleCommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof(InstallRabbitMqCommand));

        public override string Name
        {
            get { return "enableRabbitMqManagementPlugin"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Enables the RabbitMq management plugin (https://www.rabbitmq.com/management.html)"; }
        }

        public EnableRabbitManagementPlugin()
        {

            Action = parameters =>
            {
                return 0;
            };
        }
    }
}