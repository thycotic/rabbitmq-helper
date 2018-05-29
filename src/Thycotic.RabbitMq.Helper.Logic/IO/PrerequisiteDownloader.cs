using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Thycotic.RabbitMq.Helper.Logic.IO
{
    /// <summary>
    ///     Pre-requisite downloader
    /// </summary>
    public class PrerequisiteDownloader
    {
        /// <summary>
        ///     Static constructor to allow TLS
        /// </summary>
        static PrerequisiteDownloader()
        {
            //ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = 
                //SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | 
                SecurityProtocolType.Tls12;
        }

        /// <summary>
        ///     Downloads the prerequisite.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="downloadUri">The download URI.</param>
        /// <param name="installerPath">The installer path.</param>
        /// <param name="checksum">The installer checksum.</param>
        /// <param name="forceDownload">if set to <c>true</c> [force download].</param>
        /// <param name="maxRetries">The maximum retries.</param>
        /// <param name="debugHandler">The debug handler.</param>
        /// <param name="infoHandler">The information handler.</param>
        /// <param name="warnHandler">The warn handler.</param>
        /// <param name="progressHandler">The progress handler.</param>
        /// <exception cref="System.IO.FileNotFoundException">Failed to download</exception>
        public void Download(CancellationToken token, Uri downloadUri, string installerPath, string checksum,
            bool forceDownload = false, int maxRetries = 5, Action<string> debugHandler = null,
            Action<string> infoHandler = null, Action<string, Exception> warnHandler = null,
            Action<PrerequisiteDownloaderProgress> progressHandler = null)
        {
            debugHandler = debugHandler ?? (str => { });
            infoHandler = infoHandler ?? (str => { });
            warnHandler = warnHandler ?? ((str, ex) => { });
            progressHandler = progressHandler ?? (progress => { });

            if (!forceDownload && File.Exists(installerPath) && CalculateMD5(installerPath) == checksum)
            {
                infoHandler(string.Format("File already exists in {0}. Skipping download", installerPath));
                return;
            }

            //check for outbound connectivity
            using (var client = new TcpClient())
            {
                try
                {
                    infoHandler($"Checking connectivity to {downloadUri.Host}...");
                    
                    client.Connect(downloadUri.Host, downloadUri.Port);

                    infoHandler($"{downloadUri.Host} appears online and ready.");
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to connect to download host", ex);
                }

            }

            var tries = 0;

            infoHandler($"Downloading installer from {downloadUri}. Please wait...");

            var downloaded = false;

            while (!downloaded && (tries < maxRetries))
                try
                {
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();

                    var client = new WebClient();

                    //use a queue to be able to store and report back progress form the original thread.
                    var progressQueue = new ConcurrentQueue<PrerequisiteDownloaderProgress>();

                    var lastReportedProgressPercentage = 0;

                    client.DownloadProgressChanged += (sender, args) =>
                    {
                        var progressPercentage = args.ProgressPercentage;

                        if (progressPercentage <= lastReportedProgressPercentage)
                            return;

                        progressQueue.Enqueue(new PrerequisiteDownloaderProgress
                        {
                            ProgressPercentage = progressPercentage,
                            BytesReceived = args.BytesReceived,
                            TotalBytesToReceive = args.TotalBytesToReceive
                        });

                        lastReportedProgressPercentage = progressPercentage;
                    };

                    var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.bin");
                    var task = client.DownloadFileTaskAsync(downloadUri, tempPath);
                    var awaiter = task.GetAwaiter();

                    while (!awaiter.IsCompleted)
                    {
                        PrerequisiteDownloaderProgress prerequisiteDownloaderProgress;
                        while (progressQueue.TryDequeue(out prerequisiteDownloaderProgress))
                        {
                            progressHandler(prerequisiteDownloaderProgress);
                            Task.Delay(TimeSpan.FromMilliseconds(100), token).GetAwaiter().GetResult();
                        }

                        Task.Delay(TimeSpan.FromSeconds(1), token).GetAwaiter().GetResult();
                    }

                    if (task.Exception != null)
                    {
                    }

                    if (File.Exists(installerPath))
                        File.Delete(installerPath);

                    stopWatch.Stop();
                    debugHandler(string.Format("File downloaded in {0}", stopWatch.Elapsed));

                    File.Move(tempPath, installerPath);

                    if (File.Exists(tempPath))
                        warnHandler(string.Format("Temp installer files still exists at {0}", tempPath), null);

                    downloaded = true;
                }
                catch (Exception ex)
                {
                    warnHandler("Failed to download will retry...", ex);
                    tries++;
                }
                finally
                {
                }

            if (!downloaded)
                throw new FileNotFoundException("Failed to download");

            if (CalculateMD5(installerPath) != checksum)
            {
                throw new FileNotFoundException("Checksum does not match");
            }
        }


        /// <summary>
        ///    Calculates the MD5 hash of a file
        /// </summary>
        public static string CalculateMD5(string path)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }

    /// <summary>
    ///     Prerequisite downloader
    /// </summary>
    public class PrerequisiteDownloaderProgress
    {
        /// <summary>
        ///     Gets or sets the progress percentage.
        /// </summary>
        /// <value>
        ///     The progress percentage.
        /// </value>
        public int ProgressPercentage { get; set; }

        /// <summary>
        ///     Gets or sets the total bytes to receive.
        /// </summary>
        /// <value>
        ///     The total bytes to receive.
        /// </value>
        public long TotalBytesToReceive { get; set; }

        /// <summary>
        ///     Gets or sets the bytes received.
        /// </summary>
        /// <value>
        ///     The bytes received.
        /// </value>
        public long BytesReceived { get; set; }
    }
}