using Autofac;
using Thycotic.CLI;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Certificate;
using Thycotic.RabbitMq.Helper.Installation.Choice;
using Thycotic.RabbitMq.Helper.Management;

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
                new ErlangLicenseChoiceConsoleCommandFragment
                {
                    Name = "agreeToErlangLicense",
                    Prompt = "Do you agree and accept the Erlang license (http://www.erlang.org/EPLICENSE)?",
                    WhenTrue = new WorkflowConsoleCommand
                    {
                        Steps = new IConsoleCommandFragment[]
                        {
                            container.Resolve<DownloadErlangCommand>(),
                            new RabbitMqLicenseChoiceConsoleCommandFragment
                            {
                                Name = "agreeToRabbitMqLicense",
                                Prompt =
                                    "Do you agree and accept the RabbitMq license (https://www.rabbitmq.com/mpl.html)?",
                                WhenTrue = new WorkflowConsoleCommand
                                {
                                    Steps = new IConsoleCommandFragment[]
                                    {
                                        container.Resolve<DownloadRabbitMqCommand>(),
                                        container.Resolve<UninstallPriorRabbitMqCommand>(),
                                        container.Resolve<UninstallPriorErlangCommand>(),
                                        container.Resolve<InstallErlangCommand>(),
                                        container.Resolve<CreateRabbitMqConfigDirectoryCommand>(),
                                        container.Resolve<SetRabbitMqBaseEnvironmentalVariableCommand>(),
                                        new SslChoiceConsoleCommandFragment
                                        {
                                            Name = "sslChoice",
                                            Prompt = "Would you like to use SSL?",
                                            WhenTrue = new WorkflowConsoleCommand
                                            {
                                                Steps = new IConsoleCommandFragment[]
                                                {
                                                    container.Resolve<ConvertCaCerToPemCommand>(),
                                                    container.Resolve<ConvertPfxToPemCommand>(),
                                                    container.Resolve<CopyRabbitMqExampleConfigFileCommand>(),
                                                    container.Resolve<InstallRabbitMqCommand>(),
                                                    container.Resolve<AddRabbitMqUserCommand>(),
                                                    container.Resolve<EnableRabbitManagementPlugin>(),
                                                    new OutputConsoleCommandFragment
                                                    {
                                                        Output = "RabbitMq is ready to use with encryption"
                                                    }
                                                }
                                            },
                                            WhenFalse = new WorkflowConsoleCommand
                                            {
                                                Steps = new IConsoleCommandFragment[]
                                                {
                                                    container.Resolve<InstallRabbitMqCommand>(),
                                                    container.Resolve<AddRabbitMqUserCommand>(),
                                                    container.Resolve<EnableRabbitManagementPlugin>(),
                                                    new OutputConsoleCommandFragment
                                                    {
                                                        Output = "RabbitMq is ready to use without encryption"
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