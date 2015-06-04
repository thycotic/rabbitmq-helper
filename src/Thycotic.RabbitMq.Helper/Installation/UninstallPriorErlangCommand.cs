using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class UninstallPriorErlangCommand : ConsoleCommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof(UninstallPriorErlangCommand));

        public override string Name
        {
            get { return "uninstallPriorErlang"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Uninstalls prior installation of Erlang"; }
        }

        public UninstallPriorErlangCommand()
        {

            Action = parameters =>
            {
                return 0;
            };
        }
    }
}