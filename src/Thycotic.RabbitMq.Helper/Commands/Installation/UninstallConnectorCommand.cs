using Autofac;
using Thycotic.CLI.Commands;
using Thycotic.CLI.Fragments;

namespace Thycotic.RabbitMq.Helper.Commands.Installation
{
    class UninstallConnectorCommand : WorkflowCommand, IImmediateCommand
    {
        //private readonly ILogWriter _log = Log.Get(typeof(UninstallConnectorCommand));

        public override string Name
        {
            get { return "uninstallConnector"; }
        }

        public override string Area
        {
            get { return CommandAreas.Installation; }
        }

        public override string Description
        {
            get { return "Uninstalls Erlang and RabbitMq"; }
        }

        public UninstallConnectorCommand(IComponentContext container)
        {
            Steps = new ICommandFragment[]
            {
                container.Resolve<UninstallPriorRabbitMqCommand>(),
                container.Resolve<UninstallPriorErlangCommand>(),
                new OutputCommandFragment
                {
                    Output = "Connector has been uninstalled."
                }
            };
        }
    }
}