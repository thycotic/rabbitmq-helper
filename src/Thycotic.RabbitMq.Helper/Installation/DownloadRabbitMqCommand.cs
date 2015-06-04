
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
                //string path;
                //string password;
                string forceDownloadString;
                var forceDownload = false;
                //if (!parameters.TryGet("path", out path)) return;
                //if (!parameters.TryGet("pw", out password)) return;
                if (parameters.TryGet("force", out forceDownloadString) && bool.TryParse(forceDownloadString, out forceDownload))
                {
                    if (forceDownload)
                    {
                        _log.Info("Forcing download");
                    }
                }

                var downloader = new InstallerDownloader();

                downloader.DownloadInstaller(CancellationToken.None, InstallationConstants.Erlang.DownloadUrl, RabbitMqInstallerPath, forceDownload);

                return 0;
            };
        }
      
    }
}
