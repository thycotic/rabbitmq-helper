using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using Autofac;
using Thycotic.Logging;
using Thycotic.Utility;

namespace Thycotic.MemoryMq.SiteConnector.Service
{
    /// <summary>
    /// Startup message writer. Mostly to ensure Autofac is working properly.
    /// </summary>
    public class StartupMessageWriter : IStartable, IDisposable
    {
        /// <summary>
        /// The startup message delay
        /// </summary>
        public static readonly TimeSpan StartupMessageDelay = TimeSpan.FromMilliseconds(500);

        private readonly ILogWriter _log = Log.Get(typeof(StartupMessageWriter));

        /// <summary>
        /// Perform once-off startup processing.
        /// </summary>
        public void Start()
        {
            _log.Debug("Application is starting...");
            _log.Info(string.Format("Running as {0}", GetUserName()));

            var delayTask = Task.Delay(StartupMessageDelay);

            Contract.Assume(delayTask != null);
            
            delayTask.ContinueWith(task => 
            {
                //from http://patorjk.com/software/taag/#p=display&f=Graffiti&t=Dobri is awesome
                var logoStream =
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("Thycotic.MemoryMq.SiteConnector.Service.logo.txt");

                if (logoStream != null)
                {
                    var sr = new StreamReader(logoStream);
                    var logoAscii = sr.ReadToEnd();

                    logoAscii = logoAscii.Replace("{version}", typeof(SiteConnectorService).Assembly.GetName().Version.ToString());

                    //don't use the log since it just spams the table
                    Console.WriteLine();
                    Console.WriteLine(logoAscii);
                    Console.WriteLine();
                }
                else
                {
                    _log.Warn("Could not locate terminal logo");
                }
            });
        }

        [SuppressMessage("Microsoft.Contracts", "TestAlwaysEvaluatingToAConstant", Justification = "Identity is checked for null")]
        private static string GetUserName()
        {
            var windowsIdentity = WindowsIdentity.GetCurrent();
            if (windowsIdentity == null)
            {
                throw new ApplicationException("Could not determine the user running the application");
            }

            return windowsIdentity.Name;
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _log.Debug("Application is stopping...");
        }
    }
}
