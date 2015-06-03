using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Thycotic.CLI;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Installation;

namespace Thycotic.RabbitMq.Helper
{

    class InstallRabbitMqCommand : ConsoleCommandBase, IImmediateConsoleCommand
    {
        private readonly string ErlangInstallerPath = Path.Combine(Path.GetTempPath(), "erlang.exe");
        private readonly string RabbitMqInstallerPath = Path.Combine(Path.GetTempPath(), "rabbitMq.exe");

        private readonly ILogWriter _log = Log.Get(typeof(InstallRabbitMqCommand));

        public override string Name
        {
            get { return "installRabbitMq"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Installs RabbitMq"; }
        }

        public InstallRabbitMqCommand()
        {

            Action = parameters =>
            {
                //string path;
                //string password;
                string forceDownloadString;
                bool forceDownload = false;
                //if (!parameters.TryGet("path", out path)) return;
                //if (!parameters.TryGet("pw", out password)) return;
                if (parameters.TryGet("force", out forceDownloadString) && bool.TryParse(forceDownloadString, out forceDownload))
                {
                    if (forceDownload)
                    {
                        _log.Info("Forcing download");
                    }
                }

                DownloadErlang(forceDownload);
                DownloadRabbitMq(forceDownload);

            };
        }

        private void DownloadInstaller(CancellationToken token, string downloadUrl, string installerPath, bool forceDownload = false, int maxRetries = 5)
        {
            if (!forceDownload && File.Exists(installerPath))
            {
                _log.Info(string.Format("File already exists in {0}. Skipping download", installerPath));
                return;
            }

            var downloaded = false;
            var retries = 0;

            while (!downloaded && retries < maxRetries)
            {
                try
                {


                    var client = new WebClient();

                    client.DownloadProgressChanged += (sender, args) =>
                    {
                        lock (Console.Out)
                        {
                            Console.SetCursorPosition(0, 0);
                            //_log.Debug(string.Format("Downloaded {0}/{1}", args.BytesReceived, args.TotalBytesToReceive));
                            Console.WriteLine("{0}/{1} megabytes downloaded", args.BytesReceived / 1024,
                                args.TotalBytesToReceive / 1024);
                        }

                    };

                    var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                    client.DownloadFileTaskAsync(new Uri(downloadUrl, UriKind.Absolute), tempPath).Wait(token);

                    File.Move(tempPath, installerPath);

                    downloaded = true;
                }
                catch (Exception ex)
                {
                    _log.Error("Failed to download", ex);
                    retries++;
                }
            }

        }

        private void DownloadErlang(bool forceDownload)
        {
            DownloadInstaller(CancellationToken.None, InstallationConstants.Erlang.DownloadUrl, ErlangInstallerPath, forceDownload);
        }

        private void DownloadRabbitMq(bool forceDownload)
        {
            DownloadInstaller(CancellationToken.None, InstallationConstants.RabbitMq.DownloadUrl, RabbitMqInstallerPath, forceDownload);
        }
    }
}
