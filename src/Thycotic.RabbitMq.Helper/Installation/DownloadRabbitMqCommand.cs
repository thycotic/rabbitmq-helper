
using System;
using System.IO;
using System.Net;
using System.Threading;
using Thycotic.CLI;
using Thycotic.CLI.Commands;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{

    class DownloadRabbitMqCommand : CommandBase, IImmediateCommand
    {
        public static readonly string RabbitMqInstallerPath = Path.Combine(Path.GetTempPath(), "rabbitMq.exe");

        private readonly ILogWriter _log = Log.Get(typeof(DownloadRabbitMqCommand));

        public override string Name
        {
            get { return "downloadRabbitMq"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Downloads RabbitMq Installer"; }
        }

        public DownloadRabbitMqCommand()
        {

            Action = parameters =>
            {
                string offlineRabbitMqInstallerPath;
                if (parameters.TryGet("offlineRabbitMqInstallerPath", out offlineRabbitMqInstallerPath) &&
                    !string.IsNullOrWhiteSpace(offlineRabbitMqInstallerPath))
                {
                    _log.Info(string.Format("Using offline installer path {0}", offlineRabbitMqInstallerPath));

                    if (!File.Exists(offlineRabbitMqInstallerPath))
                    {
                        _log.Error("Installer does not exist");
                    }
                    else
                    {
                        if (File.Exists(RabbitMqInstallerPath))
                        {
                            File.Delete(RabbitMqInstallerPath);
                        }

                        File.Move(offlineRabbitMqInstallerPath, RabbitMqInstallerPath);
                    }
                }
                else
                {
                    var forceDownload = false;
                    if (parameters.TryGetBoolean("forceDownload", out forceDownload) &&
                        forceDownload)
                    {
                        _log.Info("Forcing download");
                    }

                    _log.Info("Downloading RabbitMq");

                    var downloader = new InstallerDownloader();

                    downloader.DownloadInstaller(CancellationToken.None, InstallationConstants.RabbitMq.DownloadUrl,
                        RabbitMqInstallerPath, forceDownload);
                }
                return 0;
            };
        }

    }
}
