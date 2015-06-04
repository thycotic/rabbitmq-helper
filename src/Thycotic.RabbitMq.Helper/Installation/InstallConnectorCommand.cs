using Autofac;
using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{
    class InstallConnectorCommand : WorkflowConsoleCommand, IImmediateConsoleCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(InstallConnectorCommand));

        public override string Name
        {
            get { return "installConnector"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Installs Erlang and RabbitMq"; }
        }

        public InstallConnectorCommand(IComponentContext container)
        {
            Steps = new IConsoleCommandFragment[]
            {
                container.Resolve<DownloadRabbitMqCommand>(),
                container.Resolve<DownloadErlangCommand>(),
                container.Resolve<UninstallPriorRabbitMqCommand>(),
                container.Resolve<UninstallPriorErlangCommand>(),
                container.Resolve<InstallErlangCommand>(),
                container.Resolve<CreateRabbitMqConfigDirectoryCommand>(),
                container.Resolve<SetRabbitMqBaseEnvironmentalVariableCommand>(),
                container.Resolve<InstallRabbitMqCommand>(),
                new BinaryConsoleCommandFragment
                {
                    Prompt = "Would you like to use SSL?",
                    WhenTrue = new WorkflowConsoleCommand
                    {
                        Steps = new IConsoleCommand[]
                        {
                            container.Resolve<CopyRabbitMqExampleConfigFileCommand>(),
                            container.Resolve<EditRabbitMqConfigFileMqCommand>(),
                            container.Resolve<UninstallPriorRabbitMqCommand>(),
                            container.Resolve<InstallRabbitMqCommand>()
                        }
                    }
                }


            };
        }
    }
}