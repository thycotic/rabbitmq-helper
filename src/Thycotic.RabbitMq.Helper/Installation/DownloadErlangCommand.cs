using System;
using System.IO;
using System.Net;
using System.Threading;
using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{

    internal class DownloadErlangCommand : ConsoleCommandBase, IImmediateConsoleCommand
    {
        public static readonly string ErlangInstallerPath = Path.Combine(Path.GetTempPath(), "erlang.exe");

        private readonly ILogWriter _log = Log.Get(typeof(DownloadErlangCommand));

        public override string Name
        {
            get { return "downloadErlang"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Installs Erlang"; }
        }

        public DownloadErlangCommand()
        {

            Action = parameters =>
            {
                //string path;
                //string password;
                string forceDownloadString;
                var forceDownload = false;
                //if (!parameters.TryGet("path", out path)) return;
                //if (!parameters.TryGet("pw", out password)) return;
                if (parameters.TryGet("force", out forceDownloadString) &&
                    bool.TryParse(forceDownloadString, out forceDownload))
                {
                    if (forceDownload)
                    {
                        _log.Info("Forcing download");
                    }
                }

                var downloader = new InstallerDownloader();

                downloader.DownloadInstaller(CancellationToken.None, InstallationConstants.Erlang.DownloadUrl,
                    ErlangInstallerPath, forceDownload);

                return 0;
            };
        }
    }
}
