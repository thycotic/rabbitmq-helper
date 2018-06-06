using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.Logic.Workflow;
using Thycotic.RabbitMq.Helper.PSCommands.Certificate;
using Thycotic.RabbitMq.Helper.PSCommands.Management;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation.Workflow
{
    /// <summary>
    ///     Installs the site connector
    /// </summary>
    /// <para type="synopsis">Installs the site connector. </para>
    /// <para type="description">The Install-Connector cmdlet is designed to make the installation of a non-SSL and SSL site connector easier.</para>
    /// <para type="description">
    ///     It will install both Erlang and RabbitMq provided that the appropriate parameters are
    ///     supplied.
    /// </para>
    /// <para type="description">
    ///     The cmdlet requires that a basic user also be created. This user is strictly for putting and
    ///     pulling messages from RabbitMq.
    /// </para>
    /// <para type="link">Convert-CaCertToPem</para>
    /// <para type="link">Convert-PfxToPem</para>
    /// <para type="link">Get-DownloadLocations</para>
    /// <para type="link">Get-ErlangInstaller</para>
    /// <para type="link">Get-RabbitMqInstaller</para>
    /// <para type="link">Install-Erlang</para>
    /// <para type="link">Install-RabbitMq</para>
    /// <para type="link">New-RabbitMqConfigDirectory</para>
    /// <para type="link">New-RabbitMqNonTlsConfigFiles</para>
    /// <para type="link">New-RabbitMqTlsConfigFiles</para>
    /// <para type="link">Set-ErlangHomeEnvironmentalVariable</para>
    /// <para type="link">Set-RabbitMqBaseEnvironmentalVariable</para>
    /// <para type="link">Uninstall-Connector</para>
    /// <para type="link">Uninstall-Erlang</para>
    /// <para type="link">Uninstall-RabbitMq</para>
    /// <example>
    ///     <para>The most basic use case to install RabbitMq is to have a single node without using encryption.</para>
    ///     <para>This is generally useful during development or during POC stages.</para>
    ///     <para>To do so, you could use the following:</para>
    ///     <para>PS C:\></para> 
    ///     <code>Install-Connector -agreeErlangLicense -agreeRabbitMqLicense</code>
    /// </example>
    [Cmdlet(VerbsLifecycle.Install, "Connector")]
    [Alias("installConnector")]
    public class InstallConnectorCommand : PSCmdlet
    {
        private static class ParameterSets
        {
            public const string Offline = "Offline";
            public const string Online = "Online";
            public const string NonSsl = "NonSsl";
            public const string Ssl = "Ssl";
        }

        /// <summary>
        ///     Gets or sets the agree rabbit mq license. If omitted, the user will not be prompted to agree to the license.
        /// </summary>
        /// <value>
        ///     The agree rabbit mq license.
        /// </value>
        /// <para type="description">
        ///     Gets or sets the agree rabbit mq license. If omitted, the user will not be prompted to agree
        ///     to the license.
        /// </para>
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public SwitchParameter AgreeRabbitMqLicense { get; set; }

        /// <summary>
        ///     Gets or sets the agree Erlang license. If omitted, the user will not be prompted to agree to the license.
        /// </summary>
        /// <value>
        ///     The agree Erlang license.
        /// </value>
        /// <para type="description">
        ///     Gets or sets the agree Erlang license. If omitted, the user will not be prompted to agree to
        ///     the license.
        /// </para>
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public SwitchParameter AgreeErlangLicense { get; set; }

        /// <summary>
        ///     Gets or sets the offline Erlang installer path. If omitted, the installer will be downloaded.
        /// </summary>
        /// <value>
        ///     The offline Erlang installer path.
        /// </value>
        /// <para type="description">Gets or sets the offline Erlang installer path. If omitted, the installer will be downloaded.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Offline)]
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Ssl)]
        public string OfflineErlangInstallerPath { get; set; }


        /// <summary>
        ///     Gets or sets the offline RabbitMq installer path to use. If omitted, the installer will be downloaded.
        /// </summary>
        /// <value>
        ///     The offline RabbitMq installer path to use.
        /// </value>
        /// <para type="description">
        ///     Gets or sets the offline RabbitMq installer path to use. If omitted, the installer will be
        ///     downloaded.
        /// </para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Offline)]
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Ssl)]
        public string OfflineRabbitMqInstallerPath { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether force download (even they already exist) the pre-requisites. This value has
        ///     no effect when using an offline installer.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [force download]; otherwise, <c>false</c>.
        /// </value>
        /// <para type="description">
        ///     Gets or sets a value indicating whether force download (even they already exist) the
        ///     pre-requisites. This value has no effect when using an offline installer.
        /// </para>
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Online)]
        [Alias("Force")]
        public SwitchParameter ForceDownload { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to use the Thycotic Mirror even if the file exists.
        /// </summary>
        /// <value>
        ///     <c>true</c> if mirror will be used; otherwise, <c>false</c>.
        /// </value>
        /// <para type="description">Gets or sets a value indicating whether to use the Thycotic Mirror even if the file exists.</para>
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Online)]
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSets.Ssl)]
        [Alias("Mirror")]
        public SwitchParameter UseThycoticMirror { get; set; }

        /// <summary>
        ///     Gets or sets the credential of the rabbit mq user.
        /// </summary>
        /// <value>
        ///     The credential of the rabbit mq user.
        /// </value>
        /// <para type="description">Gets or sets the name of the rabbit mq user.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public PSCredential Credential { get; set; }

        /// <summary>
        ///     Gets or sets whether to use SSL or not.
        /// </summary>
        /// <value>
        ///     The use SSL.
        /// </value>
        /// <para type="description">Gets or sets whether to use SSL or not.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Ssl)]
        [ValidateNotNullOrEmpty]
        public SwitchParameter UseSsl { get; set; }

        /// <summary>
        ///     Gets or sets the hostname or FQDN of the server which will host the RabbitMq node.
        /// </summary>
        /// <value>
        ///     The hostname.
        /// </value>
        /// <para type="description">Gets or sets the hostname or FQDN of the server which will host the RabbitMq node.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Ssl)]
        [Alias("SubjectName", "FQDN")]
        [ValidateNotNullOrEmpty]
        public string Hostname { get; set; }

        /// <summary>
        ///     Gets or sets the CA certificate path. This certificate is use to establish the trust chain to the CA.
        /// </summary>
        /// <value>
        ///     The ca cert path.
        /// </value>
        /// <para type="description">
        ///     Gets or sets the CA certificate path. This certificate is use to establish the trust chain to
        ///     the CA.
        /// </para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Ssl)]
        [ValidateNotNullOrEmpty]
        public string CaCertPath { get; set; }

        /// <summary>
        ///     Gets or sets the PFX path. This could be a self-signed or a certificate from a public CA.
        ///     If self-signed, the certificate should be installed on all client/engine machines. It does NOT to be installed on
        ///     the RabbitMq node.
        /// </summary>
        /// <value>
        ///     The PFX path.
        /// </value>
        /// <para type="description">Gets or sets the PFX path. This could be a self-signed or a certificate from a public CA.</para>
        /// <para type="description">
        ///     If self-signed, the certificate should be installed on all client/engine machines. It does NOT
        ///     to be installed on the RabbitMq node.
        /// </para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Ssl)]
        [ValidateNotNullOrEmpty]
        public string PfxPath { get; set; }

        /// <summary>
        ///     Gets or sets the PFX password. Username part is ignored.
        /// </summary>
        /// <value>
        ///     The credential for the PFX.
        /// </value>
        /// <para type="description">Gets or set the credential for the PFX. Username part is ignored.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Ssl)]
        public PSCredential PfxCredential { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            using (var workflow = new CmdletWorkflow(this, "Installing"))
            {
                workflow
                    .ReportProgress("Checking Erlang pre-requisites", 5)
                    .If(() => AgreeErlangLicense || ShouldContinue(
                                  "Do you agree and accept the Erlang license (http://www.erlang.org/EPLICENSE)?",
                                  "License"))
                    .Then(() => new GetErlangInstallerCommand
                    {
                        CommandRuntime = CommandRuntime,
                        OfflineErlangInstallerPath = OfflineErlangInstallerPath,
                        Force = ForceDownload,
                        UseThycoticMirror = UseThycoticMirror
                    })

                    .ReportProgress("Checking RabbitMq pre-requisites", 10)
                    .If(() => AgreeRabbitMqLicense || ShouldContinue(
                                  "Do you agree and accept the RabbitMq license (https://www.rabbitmq.com/mpl.html)?",
                                  "License"))
                    .Then(() => new GetRabbitMqInstallerCommand
                    {
                        CommandRuntime = CommandRuntime,
                        OfflineRabbitMqInstallerPath = OfflineRabbitMqInstallerPath,
                        Force = ForceDownload,
                        UseThycoticMirror = UseThycoticMirror
                    })

                    .ReportProgress("Un-installing prior versions", 20)
                    .Then(() => new UninstallRabbitMqCommand())
                    .Then(() => new UninstallErlangCommand())

                    .ReportProgress("Installing Erlang", 30)
                    .Then(() => new SetErlangHomeEnvironmentalVariableCommand())
                    .Then(() => new InstallErlangCommand())

                    .ReportProgress("Preparing for RabbitMq installation Erlang", 50)
                    .Then(() => new NewRabbitMqConfigDirectoryCommand())
                    .Then(() => new SetRabbitMqBaseEnvironmentalVariableCommand())

                    .ThenFork(UseSsl, tlsFlow =>
                    {
                        tlsFlow
                            .Then(() => WriteVerbose("Configuring RabbitMq with TLS support"))
                            .ReportProgress("Converting certificates and configuring", 60)
                            .Then(() => new ConvertCaCerToPemCommand { CaCertPath = CaCertPath })
                            .Then(() => new ConvertPfxToPemCommand { PfxPath = PfxPath, PfxCredential = PfxCredential })
                            .Then(() => new NewRabbitMqTlsConfigFilesCommand())

                            .ReportProgress("Installing RabbitMq", 70)
                            .Then(() => new InstallRabbitMqCommand())
                            .Then(() => new CopyErlangCookieFileCommand())
                            .Then(() => new AssertRabbitIsRunningCommand())

                            .ReportProgress("Final configurations", 90)

                            .Then(() => new EnableRabbitMqManagementCommand())

                            .Then(() => new NewRabbitMqUserCommand
                            {
                                UserName = Credential.UserName,
                                Password = Credential.GetNetworkCredential().Password
                            })
                            .Then(() => new GrantRabbitMqUserPermissionCommand
                            {
                                UserName = Credential.UserName
                            })
                            .Then(() => new AssertConnectivityCommand
                            {
                                Hostname = Hostname,
                                Credential = Credential,
                                UseSsl = UseSsl
                            })

                            .Then(() => WriteVerbose(
                                "RabbitMq is ready to use with TLS. Please open port 5671 on the machine firewall"))

                            .Then(() => new OpenRabbitMqManagementCommand())
                            .Then(() => WriteObject("Installation completed"));

                    }, nonTlsFlow =>
                    {
                        nonTlsFlow
                            .Then(() => WriteVerbose("Configuring RabbitMq without TLS support"))
                            .ReportProgress("Configuring", 60)
                            .Then(() => new NewRabbitMqNonTlsConfigFilesCommand())

                            .ReportProgress("Installing RabbitMq", 70)
                            .Then(() => new InstallRabbitMqCommand())
                            .Then(() => new CopyErlangCookieFileCommand())
                            .Then(() => new AssertRabbitIsRunningCommand())

                            .ReportProgress("Final configurations", 90)

                            .Then(() => new EnableRabbitMqManagementCommand())

                            .Then(() => new NewRabbitMqUserCommand
                            {
                                UserName = Credential.UserName,
                                Password = Credential.GetNetworkCredential().Password
                            })
                            .Then(() => new GrantRabbitMqUserPermissionCommand
                            {
                                UserName = Credential.UserName
                            })
                            .Then(() => new AssertConnectivityCommand
                            {
                                Credential = Credential
                            })

                            .Then(() => WriteVerbose(
                                "RabbitMq is ready to use without TLS. Please open port 5672 on the machine firewall."))

                            .Then(() => new OpenRabbitMqManagementCommand())
                            .Then(() => WriteObject("Installation completed"));

                    })
                    .Invoke();
            }
        }
    }
}