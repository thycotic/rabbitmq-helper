using System.Management.Automation;
using Thycotic.RabbitMq.Helper.PSCommands.Certificate;
using Thycotic.RabbitMq.Helper.PSCommands.Management;
using Thycotic.RabbitMq.Helper.PSCommands.Utility;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
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
    /// <para type="link">Copy-RabbitMqExampleNonSslConfigFile</para>
    /// <para type="link">Copy-RabbitMqExampleSslConfigFile</para>
    /// <para type="link">Get-DownloadLocations</para>
    /// <para type="link">Get-ErlangInstaller</para>
    /// <para type="link">Get-RabbitMqInstaller</para>
    /// <para type="link">Install-Erlang</para>
    /// <para type="link">Install-RabbitMq</para>
    /// <para type="link">New-RabbitMqConfigDirectory</para>
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
    ///     <code>Install-Connector -rabbitMqUsername SITEUN -rabbitMqPw SITEPW -agreeErlangLicense -agreeRabbitMqLicense</code>
    /// </example>
    /// <example>
    ///     <para>You can avoid being prompted to agree to Erlang or RabbitMq licenses (during automated deployment) use</para>
    ///     <para>To do so, you could use the following:</para>
    ///     <para>PS C:\></para> 
    ///     <code>Install-Connector -rabbitMqUsername SITEUN -rabbitMqPw SITEPW -agreeErlangLicense -agreeRabbitMqLicense</code>
    /// </example>
    /// <example>
    ///     <para>The leverage secure communication between RabbitMq and its clients, you should use encryption.</para>
    ///     <para>To do so, you could use the following:</para>
    ///     <para>PS C:\></para> 
    ///     <code>Install-Connector -verbose -hostname RABBITHOST1.FQDN -useSsl -rabbitMqUsername SITEUN -rabbitMqPw SITEPW -cacertpath $path\sc.cer -pfxPath $path\sc.pfx -pfxPw SOMEPASSWORD</code>
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
        [Alias("Mirror")]
        public SwitchParameter UseThycoticMirror { get; set; }

        /// <summary>
        ///     Gets or sets the name of the RabbitMq user name of the initial user.
        /// </summary>
        /// <value>
        ///     The name of the RabbitMq user.
        /// </value>
        /// <para type="description">Gets or sets the name of the RabbitMq user name of the initial user.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        [Alias("RabbitMqUserName")]
        public string UserName { get; set; }

        /// <summary>
        ///     Gets or sets the RabbitMq password of the initial user.
        /// </summary>
        /// <value>
        ///     The RabbitMq password.
        /// </value>
        /// <para type="description">Gets or sets the RabbitMq password of the initial user.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        [Alias("RabbitMqPw", "RabbitMqPassword")]
        public string Password { get; set; }

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
        ///     Gets or sets the PFX password.
        /// </summary>
        /// <value>
        ///     The PFX password.
        /// </value>
        /// <para type="description">Gets or sets the PFX password.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Ssl)]
        [ValidateNotNullOrEmpty]
        [Alias("PfxPw")]
        public string PfxPassword { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            const int activityid = 7;
            const string activity = "Installing";

            WriteProgress(new ProgressRecord(activityid, activity, "Checking Erlang pre-requisites")
            {
                PercentComplete = 5
            });

            if (!AgreeErlangLicense &&
                !ShouldContinue("Do you agree and accept the Erlang license (http://www.erlang.org/EPLICENSE)?",
                    "License"))
                return;

            new GetErlangInstallerCommand
            {
                CommandRuntime = CommandRuntime,
                OfflineErlangInstallerPath = OfflineErlangInstallerPath,
                Force = ForceDownload,
                UseThycoticMirror = UseThycoticMirror
            }.InvokeImmediate();

            WriteProgress(new ProgressRecord(activityid, activity, "Checking RabbitMq pre-requisites")
            {
                PercentComplete = 10
            });

            if (!AgreeRabbitMqLicense &&
                !ShouldContinue("Do you agree and accept the RabbitMq license (https://www.rabbitmq.com/mpl.html)?",
                    "License"))
                return;

            new GetRabbitMqInstallerCommand
            {
                CommandRuntime = CommandRuntime,
                OfflineRabbitMqInstallerPath = OfflineRabbitMqInstallerPath,
                Force = ForceDownload,
                UseThycoticMirror = UseThycoticMirror
            }.InvokeImmediate();

            WriteProgress(new ProgressRecord(activityid, activity, "Uninstalling prior versions")
            {
                PercentComplete = 20
            });

            new UninstallRabbitMqCommand().AsChildOf(this).InvokeImmediate();
            new UninstallErlangCommand().AsChildOf(this).InvokeImmediate();

            WriteProgress(new ProgressRecord(activityid, activity, "Installing Erlang") { PercentComplete = 30 });

            new SetErlangHomeEnvironmentalVariableCommand().AsChildOf(this).InvokeImmediate();
            new InstallErlangCommand().AsChildOf(this).InvokeImmediate();

            WriteProgress(new ProgressRecord(activityid, activity, "Preparing for RabbitMq installation Erlang")
            {
                PercentComplete = 50
            });

            new NewRabbitMqConfigDirectoryCommand().AsChildOf(this).InvokeImmediate();
            new SetRabbitMqBaseEnvironmentalVariableCommand().AsChildOf(this).InvokeImmediate();

            if (UseSsl)
            {
                WriteVerbose("Configuring RabbitMq with encryption support");

                WriteProgress(new ProgressRecord(activityid, activity, "Converting certificates and configuring")
                {
                    PercentComplete = 60
                });

                new ConvertCaCerToPemCommand { CaCertPath = CaCertPath }.AsChildOf(this).InvokeImmediate();
                new ConvertPfxToPemCommand { PfxPath = PfxPath, PfxPassword = PfxPassword }.AsChildOf(this)
                    .InvokeImmediate();
                new CopyRabbitMqExampleSslConfigFileCommand().AsChildOf(this).InvokeImmediate();

                WriteProgress(new ProgressRecord(activityid, activity, "Installing RabbitMq") { PercentComplete = 70 });

                new InstallRabbitMqCommand().AsChildOf(this).InvokeImmediate();

                WriteProgress(new ProgressRecord(activityid, activity, "Final configurations")
                {
                    PercentComplete = 90
                });

                new NewBasicRabbitMqUserCommand
                {
                    UserName = UserName,
                    Password = Password
                }
                    .AsChildOf(this).InvokeImmediate();

                new EnableRabbitMqManagementPluginCommand().AsChildOf(this).InvokeImmediate();

                new AssertConnectivityCommand
                {
                    Hostname = Hostname,
                    UserName = UserName,
                    Password = Password,
                    UseSsl = UseSsl
                }.AsChildOf(this).InvokeImmediate();

                WriteProgress(new ProgressRecord(activityid, activity, "Installation completed")
                {
                    PercentComplete = 100,
                    RecordType = ProgressRecordType.Completed
                });

                WriteVerbose(
                    "RabbitMq is ready to use with encryption. Please open port 5671 on the machine firewall");

                WriteObject("Installation completed");
            }
            else
            {
                WriteVerbose("Configuring RabbitMq without encryption support");

                WriteProgress(new ProgressRecord(activityid, activity, "Configuring") { PercentComplete = 60 });

                new CopyRabbitMqExampleNonSslConfigFileCommand().AsChildOf(this).InvokeImmediate();

                WriteProgress(new ProgressRecord(activityid, activity, "Installing RabbitMq") { PercentComplete = 70 });

                new InstallRabbitMqCommand().AsChildOf(this).InvokeImmediate();

                WriteProgress(new ProgressRecord(activityid, activity, "Final configurations")
                {
                    PercentComplete = 90
                });

                new NewBasicRabbitMqUserCommand
                {
                    UserName = UserName,
                    Password = Password
                }.AsChildOf(this).InvokeImmediate();

                new EnableRabbitMqManagementPluginCommand().AsChildOf(this).InvokeImmediate();

                new AssertConnectivityCommand
                {
                    UserName = UserName,
                    Password = Password
                }.AsChildOf(this).InvokeImmediate();

                WriteProgress(new ProgressRecord(activityid, activity, "Installation completed")
                {
                    PercentComplete = 100,
                    RecordType = ProgressRecordType.Completed
                });

                WriteVerbose(
                    "RabbitMq is ready to use without encryption. Please open port 5672 on the machine firewall.");

                WriteObject("Installation completed");
            }
        }
    }
}