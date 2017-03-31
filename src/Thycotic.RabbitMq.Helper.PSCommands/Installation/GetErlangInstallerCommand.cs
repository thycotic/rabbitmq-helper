using System.IO;
using System.Management.Automation;
using System.Threading;
using Thycotic.CLI.Commands;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{

    /// <summary>
    /// Downloads Erlang
    /// </summary>
    /// <para type="synopsis">TODO: This is the cmdlet synopsis.</para>
    /// <para type="description">TODO: This is part of the longer cmdlet description.</para>
    /// <para type="description">TODO: Also part of the longer cmdlet description.</para>
    /// <para type="link" uri="http://tempuri.org">TODO: Thycotic</para>
    /// <para type="link">TODO: Get-Help</para>
    /// <example>
    ///   <para>TODO: This is part of the first example's introduction.</para>
    ///   <para>TODO: This is also part of the first example's introduction.</para>
    ///   <code>TODO: New-Thingy | Write-Host</code>
    ///   <para>TODO: This is part of the first example's remarks.</para>
    ///   <para>TODO: This is also part of the first example's remarks.</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "ErlangInstaller")]
    public class GetErlangInstallerCommand : Cmdlet
    {
        private static class ParameterSets
        {
            public const string Offline = "Offline";
            public const string Online = "Online";
        }

        /// <summary>
        /// The erlang installer path
        /// </summary>
        public static readonly string ErlangInstallerPath = Path.Combine(Path.GetTempPath(), "erlang.exe");

        /// <summary>
        /// Gets or sets the offline erlang installer path.
        /// </summary>
        /// <value>
        /// The offline erlang installer path.
        /// </value>
        /// <para type="description">TODO: Property description.</para>
        [Parameter(
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Offline)]
        [Alias("OfflinePath")]
        public string OfflineErlangInstallerPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [force download].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [force download]; otherwise, <c>false</c>.
        /// </value>
        /// <para type="description">TODO: Property description.</para>
        [Parameter(
        ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Online)]
        [Alias("Force")]
        public SwitchParameter ForceDownload { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the Thycotic Mirror.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [force download]; otherwise, <c>false</c>.
        /// </value>
        /// <para type="description">TODO: Property description.</para>
        [Parameter(
        ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
             ParameterSetName = ParameterSets.Online)]
        [Alias("Mirror")]
        public SwitchParameter UseThycoticMirror { get; set; }

        /// <summary>
        /// Processes the record.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Installer does not exist</exception>
        protected override void ProcessRecord()
        {
            if (!string.IsNullOrWhiteSpace(OfflineErlangInstallerPath))
            {
                WriteDebug(string.Format("Using offline installer path {0}", OfflineErlangInstallerPath));

                if (!File.Exists(OfflineErlangInstallerPath))
                {
                    throw new FileNotFoundException("Installer does not exist");
                }
                else
                {
                    if (File.Exists(ErlangInstallerPath))
                    {
                        File.Delete(ErlangInstallerPath);
                    }

                    File.Copy(OfflineErlangInstallerPath, ErlangInstallerPath);
                }
            }
            else
            {
                if (ForceDownload)
                {
                    WriteVerbose("Forcing download");
                }

                WriteVerbose("Downloading Erlang");

                var downloader = new PrerequisiteDownloader();

                var downloadUrl = UseThycoticMirror ? InstallationConstants.Erlang.ThycoticMirrorDownloadUrl : InstallationConstants.Erlang.DownloadUrl;

                downloader.Download(CancellationToken.None, downloadUrl,
                    ErlangInstallerPath, ForceDownload, 5, WriteDebug, WriteVerbose, (s, exception) =>
                    {
                        throw exception;
                    }, progress =>
                    {
                        WriteProgress(new ProgressRecord(1, "Erlang download in progress", "Downloading")
                        {
                            PercentComplete = progress.ProgressPercentage
                        });
                    });
            }

        }
    }
}
