using Autofac;
using Thycotic.CLI.Commands;
using Thycotic.CLI.Fragments;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Certificate;
using Thycotic.RabbitMq.Helper.Commands.Management;
using Thycotic.RabbitMq.Helper.Installation;
using Thycotic.RabbitMq.Helper.Installation.Choice;

namespace Thycotic.RabbitMq.Helper.Commands.Installation
{
    class InstallConnectorCommand : WorkflowCommand, IImmediateCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(InstallConnectorCommand));

        public override string Name
        {
            get { return "installConnector"; }
        }

        public override string Area
        {
            get { return CommandAreas.Installation; }
        }

        public override string Description
        {
            get { return "Installs Erlang and RabbitMq"; }
        }

        public InstallConnectorCommand(IComponentContext container)
        {
            Steps = new ICommandFragment[]
            {
                new ErlangLicenseChoiceConsoleCommandFragment
                {
                    Name = "agreeToErlangLicense",
                    Prompt = "Do you agree and accept the Erlang license (http://www.erlang.org/EPLICENSE)?",
                    WhenTrue = new WorkflowCommand
                    {
                        Steps = new ICommandFragment[]
                        {
                            container.Resolve<DownloadErlangCommand>(),
                            new RabbitMqLicenseChoiceConsoleCommandFragment
                            {
                                Name = "agreeToRabbitMqLicense",
                                Prompt =
                                    "Do you agree and accept the RabbitMq license (https://www.rabbitmq.com/mpl.html)?",
                                WhenTrue = new WorkflowCommand
                                {
                                    Steps = new ICommandFragment[]
                                    {
                                        container.Resolve<DownloadRabbitMqCommand>(),
                                        container.Resolve<UninstallPriorRabbitMqCommand>(),
                                        container.Resolve<UninstallPriorErlangCommand>(),
                                        container.Resolve<SetErlangHomeEnvironmentalVariableCommand>(),
                                        container.Resolve<InstallErlangCommand>(),
                                        container.Resolve<CreateRabbitMqConfigDirectoryCommand>(),
                                        container.Resolve<SetRabbitMqBaseEnvironmentalVariableCommand>(),
                                        new SslChoiceConsoleCommandFragment
                                        {
                                            Name = "sslChoice",
                                            Prompt = "Would you like to use SSL?",
                                            WhenTrue = new WorkflowCommand
                                            {
                                                Steps = new ICommandFragment[]
                                                {
                                                    new OutputCommandFragment
                                                    {
                                                        Output = "Configuring RabbitMq with encryption support"
                                                    },
                                                    container.Resolve<ConvertCaCerToPemCommand>(),
                                                    container.Resolve<ConvertPfxToPemCommand>(),
                                                    container.Resolve<CopyRabbitMqExampleSslConfigFileCommand>(),
                                                    container.Resolve<InstallRabbitMqCommand>(),
                                                    container.Resolve<AddRabbitMqUserCommand>(),
                                                    container.Resolve<ValidateConnectivityCommand>(),
                                                    container.Resolve<EnableRabbitManagementPlugin>(),
                                                    new OutputCommandFragment
                                                    {
                                                        Output = "RabbitMq is ready to use with encryption. Please open port 5671 on the machine firewall."
                                                    }
                                                }
                                            },
                                            WhenFalse = new WorkflowCommand
                                            {
                                                Steps = new ICommandFragment[]
                                                {
                                                    container.Resolve<CopyRabbitMqExampleNonSslConfigFileCommand>(),
                                                    container.Resolve<InstallRabbitMqCommand>(),
                                                    container.Resolve<AddRabbitMqUserCommand>(),
                                                    container.Resolve<ValidateConnectivityCommand>(),
                                                    container.Resolve<EnableRabbitManagementPlugin>(),
                                                    new OutputCommandFragment
                                                    {
                                                        Output = "RabbitMq is ready to use without encryption. Please open port 5672 on the machine firewall."
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}