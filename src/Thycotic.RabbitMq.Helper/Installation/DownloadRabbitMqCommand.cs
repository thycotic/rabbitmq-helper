
using System;
using System.IO;
using System.Net;
using System.Threading;
using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{

    class DownloadRabbitMqCommand : ConsoleCommandBase, IImmediateConsoleCommand
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
                var forceDownload = false;
                if (parameters.TryGetBoolean("forceDownload", out forceDownload) &&
                    forceDownload)
                {
                    _log.Info("Forcing download");
                }

                _log.Info("Downloading RabbitMq");

                var downloader = new InstallerDownloader();

                downloader.DownloadInstaller(CancellationToken.None, InstallationConstants.RabbitMq.DownloadUrl, RabbitMqInstallerPath, forceDownload);

                return 0;
            };
        }

    }
}
