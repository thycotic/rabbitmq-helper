using System.Management.Automation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation.Workflow
{
    /// <summary>
    ///     Uses a guided connector installation
    /// </summary>
    /// <para type="synopsis">Installs the site connector by asking a series of questions about what to do. </para>
    /// <para type="description">The Use-GuidedInstallConnector cmdlet is designed to make the installation of a non-TLS and TLS RabbitMq even easier by asking a series of questions and then executing a finalized commandlet.</para>
    /// <para type="link">Install-Connector</para>
    [Cmdlet(VerbsOther.Use, "GuidedInstallConnector")]
    public class UseGuidedInstallConnectorCommand : PSCmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            throw new PSNotImplementedException();
            //using (var workflow = new CmdletWorkflow(this, "Installing"))
            //{
            //    workflow
            //        .ReportProgress("Checking Erlang pre-requisites", 5)
            //        .If(() => AgreeErlangLicense || ShouldContinue(
            //                      "Do you agree and accept the Erlang license (http://www.erlang.org/EPLICENSE)?",
            //                      "License"))
            //        .Then(() => new GetErlangInstallerCommand
            //        {
            //            CommandRuntime = CommandRuntime,
            //            OfflineErlangInstallerPath = OfflineErlangInstallerPath,
            //            Force = ForceDownload,
            //            UseThycoticMirror = UseThycoticMirror
            //        })

            //        .ReportProgress("Checking RabbitMq pre-requisites", 10)
            //        .If(() => AgreeRabbitMqLicense || ShouldContinue(
            //                      "Do you agree and accept the RabbitMq license (https://www.rabbitmq.com/mpl.html)?",
            //                      "License"))
            //        .Then(() => new GetRabbitMqInstallerCommand
            //        {
            //            CommandRuntime = CommandRuntime,
            //            OfflineRabbitMqInstallerPath = OfflineRabbitMqInstallerPath,
            //            Force = ForceDownload,
            //            UseThycoticMirror = UseThycoticMirror
            //        })

            //        .ReportProgress("Un-installing prior versions", 20)
            //        .Then(() => new UninstallRabbitMqCommand())
            //        .Then(() => new UninstallErlangCommand())

            //        .ReportProgress("Installing Erlang", 30)
            //        .Then(() => new SetErlangHomeEnvironmentalVariableCommand())
            //        .Then(() => new InstallErlangCommand())

            //        .ReportProgress("Preparing for RabbitMq installation Erlang", 50)
            //        .Then(() => new NewRabbitMqConfigDirectoryCommand())
            //        .Then(() => new SetRabbitMqBaseEnvironmentalVariableCommand())

            //        .ThenFork(UseSsl, tlsFlow =>
            //        {
            //            tlsFlow
            //                .Then(() => WriteVerbose("Configuring RabbitMq with TLS support"))
            //                .ReportProgress("Converting certificates and configuring", 60)
            //                .Then(() => new ConvertCaCerToPemCommand { CaCertPath = CaCertPath })
            //                .Then(() => new ConvertPfxToPemCommand { PfxPath = PfxPath, PfxCredential = PfxCredential })
            //                .Then(() => new NewRabbitMqTlsConfigFilesCommand())

            //                .ReportProgress("Installing RabbitMq", 70)
            //                .Then(() => new InstallRabbitMqCommand())
            //                .Then(() => new CopyErlangCookieFileCommand())
            //                .Then(() => new AssertRabbitIsRunningCommand())

            //                .ReportProgress("Final configurations", 90)

            //                .Then(() => new EnableRabbitMqManagementCommand())

            //                .Then(() => new NewRabbitMqUserCommand
            //                {
            //                    UserName = Credential.UserName,
            //                    Password = Credential.GetNetworkCredential().Password
            //                })
            //                .Then(() => new GrantRabbitMqUserPermissionCommand
            //                {
            //                    UserName = Credential.UserName
            //                })
            //                .Then(() => new AssertConnectivityCommand
            //                {
            //                    Hostname = Hostname,
            //                    Credential = Credential,
            //                    UseSsl = UseSsl
            //                })

            //                .Then(() => WriteVerbose(
            //                    "RabbitMq is ready to use with TLS. Please open port 5671 on the machine firewall"))

            //                .Then(() => new OpenRabbitMqManagementCommand())
            //                .Then(() => WriteObject("Installation completed"));

            //        }, nonTlsFlow =>
            //        {
            //            nonTlsFlow
            //                .Then(() => WriteVerbose("Configuring RabbitMq without TLS support"))
            //                .ReportProgress("Configuring", 60)
            //                .Then(() => new NewRabbitMqNonTlsConfigFilesCommand())

            //                .ReportProgress("Installing RabbitMq", 70)
            //                .Then(() => new InstallRabbitMqCommand())
            //                .Then(() => new CopyErlangCookieFileCommand())
            //                .Then(() => new AssertRabbitIsRunningCommand())

            //                .ReportProgress("Final configurations", 90)

            //                .Then(() => new EnableRabbitMqManagementCommand())

            //                .Then(() => new NewRabbitMqUserCommand
            //                {
            //                    UserName = Credential.UserName,
            //                    Password = Credential.GetNetworkCredential().Password
            //                })
            //                .Then(() => new GrantRabbitMqUserPermissionCommand
            //                {
            //                    UserName = Credential.UserName
            //                })
            //                .Then(() => new AssertConnectivityCommand
            //                {
            //                    Credential = Credential
            //                })

            //                .Then(() => WriteVerbose(
            //                    "RabbitMq is ready to use without TLS. Please open port 5672 on the machine firewall."))

            //                .Then(() => new OpenRabbitMqManagementCommand())
            //                .Then(() => WriteObject("Installation completed"));

            //        })
            //        .Invoke();
            //}
        }
    }
}