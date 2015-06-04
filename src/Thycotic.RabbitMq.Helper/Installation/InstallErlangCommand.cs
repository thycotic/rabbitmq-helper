using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class InstallErlangCommand : ConsoleCommandBase
    {
        
        private readonly ILogWriter _log = Log.Get(typeof (InstallErlangCommand));

        public override string Name
        {
            get { return "installErlang"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Installs Erlang"; }
        }

        public InstallErlangCommand()
        {

            Action = parameters =>
            {
                return 0;
            };
        }
    }
}