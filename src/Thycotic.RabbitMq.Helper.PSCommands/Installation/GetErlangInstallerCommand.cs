using System;
using System.IO;
using System.Management.Automation;
using System.Threading;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.Logic.IO;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Downloads Erlang
    /// </summary>
    /// <para type="synopsis">Downloads Erlang</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Get-DownloadLocations</para>
    /// <para type="link">Get-RabbitMqInstaller</para>
    /// <para type="link">Install-Erlang</para>
    /// <example>
    ///     <para>Download from erlang's web site</para>
    ///     <para>PS C:\></para> 
    ///     <code>Get-ErlangInstaller</code>
    /// </example>
    /// <example>
    ///     <para>Download from Thycotic's mirror web site</para>
    ///     <para>PS C:\></para> 
    ///     <code>Get-ErlangInstaller -UseThycoticMirror</code>
    /// </example>
    /// <example>
    ///     <para>Force download from Thycotic's mirror web site even if the file already exists</para>
    ///     <para>PS C:\></para> 
    ///     <code>Get-ErlangInstaller -UseThycoticMirror -Force</code>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "ErlangInstaller")]
    public class GetErlangInstallerCommand : Cmdlet
    {
        /// <summary>
        ///     The erlang installer path
        /// </summary>
        public static readonly string ErlangInstallerPath = Path.Combine(Path.GetTempPath(), string.Format("erlang{0}.exe", InstallationConstants.Erlang.Version));

        /// <summary>
        ///     Gets or sets the offline erlang installer path.
        /// </summary>
        /// <value>
        ///     The offline erlang installer path.
        /// </value>
        /// <para type="description">Gets or sets the offline erlang installer path.</para>
        [Parameter(
             Position = 0,
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Offline)]
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSets.Prepare)]
        [Alias("OfflinePath")]
        [ValidateNotNullOrEmpty]
        public string OfflineErlangInstallerPath { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to force download even if the file exists.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [force download]; otherwise, <c>false</c>.
        /// </value>
        /// <para type="description">Gets or sets a value indicating whether to force download even if the file exists.</para>
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Online)]
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSets.Prepare)]
        [Alias("ForceDownload")]
        public SwitchParameter Force { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to use the Thycotic Mirror during download.
        /// </summary>
        /// <value>
        ///     <c>true</c> if mirror will be used; otherwise, <c>false</c>.
        /// </value>
        /// <para type="description">Gets or sets a value indicating whether to use the Thycotic Mirror during download.</para>
        [Parameter(
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Online)]
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSets.Prepare)]
        [Alias("Mirror")]
        public SwitchParameter UseThycoticMirror { get; set; }

        /// <summary>
        /// Gets or sets whether to prepare for offline install.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [force download]; otherwise, <c>false</c>.
        /// </value>
        /// <para type="description">Gets or sets whether to prepare for offline install.</para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParameterSets.Prepare)]
        public SwitchParameter PrepareForOfflineInstall { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Installer does not exist</exception>
        protected override void ProcessRecord()
        {

            if (!string.IsNullOrWhiteSpace(OfflineErlangInstallerPath))
            {
                if (PrepareForOfflineInstall)
                {
                    WriteDebug($"Preparing offline installer path {OfflineErlangInstallerPath}");

                    if (Force)
                        WriteVerbose("Forcing download");

                    WriteVerbose("Downloading Erlang");

                    var downloader = new PrerequisiteDownloader();

                    var downloadUrl = UseThycoticMirror
                        ? InstallationConstants.Erlang.ThycoticMirrorDownloadUrl
                        : InstallationConstants.Erlang.DownloadUrl;

                    downloader.Download(CancellationToken.None, new Uri(downloadUrl, UriKind.Absolute),
                        OfflineErlangInstallerPath, InstallationConstants.Erlang.InstallerChecksum, Force, 5, WriteDebug, WriteVerbose, (s, exception) => throw exception,
                        progress =>
                        {
                            WriteProgress(new ProgressRecord(1, "Erlang download in progress", "Downloading")
                            {
                                PercentComplete = progress.ProgressPercentage
                            });
                        });

                }
                else
                {
                    WriteDebug($"Using offline installer path {OfflineErlangInstallerPath}");

                    if (!File.Exists(OfflineErlangInstallerPath))
                    {
                        throw new FileNotFoundException("Installer does not exist");
                    }

                    if (PrerequisiteDownloader.CalculateMD5(OfflineErlangInstallerPath) !=
                        InstallationConstants.Erlang.InstallerChecksum)
                    {
                        throw new FileNotFoundException("Installer checksum does not match");
                    }
                    if (File.Exists(ErlangInstallerPath))
                        File.Delete(ErlangInstallerPath);

                    File.Copy(OfflineErlangInstallerPath, ErlangInstallerPath);
                }
            }
            else
            {
                if (Force)
                    WriteVerbose("Forcing download");

                WriteVerbose("Downloading Erlang");

                var downloader = new PrerequisiteDownloader();

                var downloadUrl = UseThycoticMirror
                    ? InstallationConstants.Erlang.ThycoticMirrorDownloadUrl
                    : InstallationConstants.Erlang.DownloadUrl;

                downloader.Download(CancellationToken.None, new Uri(downloadUrl, UriKind.Absolute),
                    ErlangInstallerPath, InstallationConstants.Erlang.InstallerChecksum, Force, 5, WriteDebug, WriteVerbose, (s, exception) => throw exception,
                    progress =>
                    {
                        WriteProgress(new ProgressRecord(1, "Erlang download in progress", "Downloading")
                        {
                            PercentComplete = progress.ProgressPercentage
                        });
                    });

            }
        }

        private static class ParameterSets
        {
            public const string Offline = "Offline";
            public const string Online = "Online";
            public const string Prepare = "Prepare";
        }
    }
}