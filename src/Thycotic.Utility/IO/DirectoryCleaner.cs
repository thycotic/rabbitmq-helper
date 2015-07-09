using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;
using Thycotic.Logging;

namespace Thycotic.Utility.IO
{
    /// <summary>
    /// Directory cleaner
    /// </summary>
    public class DirectoryCleaner
    {

        private readonly ILogWriter _log = Log.Get(typeof (DirectoryCleaner));

        /// <summary>
        /// Cleans the specified path recursively.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="maxRetries">The maximum retries.</param>
        public void Clean(string path, int maxRetries = 5)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(path));
            Contract.Requires<ArgumentOutOfRangeException>(maxRetries > 1);

            var tries = 0;

            while (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, true);
                }
                catch (Exception ex)
                {
                    _log.Warn("Could not clean path. Will retry...", ex);
                    tries++;

                    if (tries >= maxRetries)
                    {
                        throw new ApplicationException(string.Format("Failed to clean path {0}", path));
                    }


                    Task.Delay(TimeSpan.FromSeconds(10)).Wait();
                }

            }
        }
    }
}
