using System.IO;
using System.Threading;
using Thycotic.CLI.Commands;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Commands.Installation
{

    internal class DownloadErlangCommand : CommandBase, IImmediateCommand
    {
        public static readonly string ErlangInstallerPath = Path.Combine(Path.GetTempPath(), "erlang.exe");

        private readonly ILogWriter _log = Log.Get(typeof(DownloadErlangCommand));

        public override string Area
        {
            get { return CommandAreas.Installation; }
        }

        public override string Description
        {
            get { return "Installs Erlang"; }
        }

        public DownloadErlangCommand()
        {

            Action = parameters =>
            {
                string offlineErlangInstallerPath;
                if (parameters.TryGet("offlineErlangInstallerPath", out offlineErlangInstallerPath) &&
                    !string.IsNullOrWhiteSpace(offlineErlangInstallerPath))
                {
                    _log.Info(string.Format("Using offline installer path {0}", offlineErlangInstallerPath));

                    if (!File.Exists(offlineErlangInstallerPath))
                    {
                        _log.Error("Installer does not exist");
                    }
                    else
                    {
                        if (File.Exists(ErlangInstallerPath))
                        {
                            File.Delete(ErlangInstallerPath);
                        }

                        File.Copy(offlineErlangInstallerPath, ErlangInstallerPath);
                    }
                }
                else
                {
                    bool forceDownload;
                    if (parameters.TryGetBoolean("forceDownload", out forceDownload) &&
                        forceDownload)
                    {
                        _log.Info("Forcing download");
                    }


                    _log.Info("Downloading Erlang");

                    var downloader = new InstallerDownloader();

                    downloader.DownloadInstaller(CancellationToken.None, InstallationConstants.Erlang.DownloadUrl,
                        ErlangInstallerPath, forceDownload);
                }
                return 0;
            };
        }
    }
}
