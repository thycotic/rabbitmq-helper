﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace Thycotic.Utility.IO
{
    /// <summary>
    /// Directory cleaner
    /// </summary>
    public class DirectoryCleaner
    {
        /// <summary>
        /// Cleans the specified path recursively.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="maxRetries">The maximum retries.</param>
        public void Clean(string path, int maxRetries = 5)
        {
            var tries = 0;

            while (Directory.Exists(path))
            {
                if (tries >= maxRetries)
                {
                    throw new ApplicationException(string.Format("Failed to clean path {0}", path));
                }

                try
                {
                    Directory.Delete(path, true);
                }
                catch (Exception)
                {
                    //_log.Warn("Could not clean path", ex);
                    tries++;

                    Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                }

            }
        }
    }
}
