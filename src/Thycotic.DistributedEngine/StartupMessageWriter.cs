using System;
using System.IO;
using System.Reflection;
using Autofac;
using Thycotic.Logging;
using Thycotic.Utility;

namespace Thycotic.DistributedEngine
{
    /// <summary>
    /// Startup message writer. Mostly to ensure Autofac is working properly.
    /// </summary>
    public class StartupMessageWriter : IStartable, IDisposable
    {
        private readonly ILogWriter _log = Log.Get(typeof(StartupMessageWriter));

        /// <summary>
        /// Perform once-off startup processing.
        /// </summary>
        public void Start()
        {
            _log.Info("Application is starting...");

            var logoStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Thycotic.DistributedEngine.logo.txt");

            if (logoStream != null)
            {
                var sr = new StreamReader(logoStream);
                var logoAscii = sr.ReadToEnd();

                logoAscii = logoAscii.Replace("{version}", ReleaseInformationHelper.Version.ToString());
                logoAscii = logoAscii.Replace("{architecture}", ReleaseInformationHelper.Architecture);

                //don't use the log since it just spams the table
                Console.WriteLine(logoAscii);
            }
            else
            {
                _log.Warn("Could not locate terminal logo");
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _log.Info("Application is stopping...");
        }
    }
}
