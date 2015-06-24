using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Installation
{
    class InstallerDownloader
    {

        private readonly ILogWriter _log = Log.Get(typeof(InstallerDownloader));

        public void DownloadInstaller(CancellationToken token, string downloadUrl, string installerPath, bool forceDownload = false, int maxRetries = 5)
        {
            if (!forceDownload && File.Exists(installerPath))
            {
                _log.Info(string.Format("File already exists in {0}. Skipping download", installerPath));
                return;
            }

            var tries = 0;

            _log.Info(string.Format("Downloading installer from {0}. Please wait...", downloadUrl));

            var downloaded = false;

            while (!downloaded && tries < maxRetries)
            {
                var oldConsoleTop = Console.CursorTop;
                var oldConsoleLeft = Console.CursorLeft;


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

                    if (File.Exists(installerPath))
                    {
                        File.Delete(installerPath);
                    }

                    File.Move(tempPath, installerPath);

                    if (File.Exists(tempPath))
                    {
                        _log.Warn(string.Format("Temp installer files still exists at {0}", tempPath));
                    }

                    downloaded = true;
                }
                catch (Exception ex)
                {
                    Console.SetCursorPosition(oldConsoleLeft, oldConsoleTop);

                    _log.Warn("Failed to download will retry...", ex);
                    tries++;
                }
                finally
                {
                    Console.SetCursorPosition(oldConsoleLeft, oldConsoleTop);
                }
            }

            if (!downloaded)
            {
                throw new FileNotFoundException("Failed to download");
            }
        }
    }
}
