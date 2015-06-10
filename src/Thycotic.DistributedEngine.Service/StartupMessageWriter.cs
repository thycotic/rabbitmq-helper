using System;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using Autofac;
using Thycotic.DistributedEngine.Service.Configuration;
using Thycotic.Logging;
using Thycotic.Utility;

namespace Thycotic.DistributedEngine.Service
{
    /// <summary>
    /// Startup message writer. Mostly to ensure Autofac is working properly.
    /// </summary>
    public class StartupMessageWriter : IStartable, IDisposable
    {
        private readonly IEngineIdentificationProvider _engineIdentificationProvider;

        /// <summary>
        /// The startup message delay
        /// </summary>
        public static readonly TimeSpan StartupMessageDelay = TimeSpan.FromMilliseconds(500);

        private readonly ILogWriter _log = Log.Get(typeof(StartupMessageWriter));

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupMessageWriter"/> class.
        /// </summary>
        /// <param name="engineIdentificationProvider">The engine identification provider.</param>
        public StartupMessageWriter(IEngineIdentificationProvider engineIdentificationProvider)
        {
            _engineIdentificationProvider = engineIdentificationProvider;
        }

        /// <summary>
        /// Perform once-off startup processing.
        /// </summary>
        public void Start()
        {
            _log.Debug("Application is starting...");
            _log.Info(string.Format("Running as {0}", GetUserName()));
            _log.Info(string.Format("Running as {0} bit process", _engineIdentificationProvider.Is64Bit ?  "64" : "32"));

            Task.Delay(StartupMessageDelay).ContinueWith(task => 
            {
                //from http://patorjk.com/software/taag/#p=display&f=Graffiti&t=Dobri is awesome
                var logoStream =
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("Thycotic.DistributedEngine.Service.logo.txt");

                if (logoStream != null)
                {
                    var sr = new StreamReader(logoStream);
                    var logoAscii = sr.ReadToEnd();

                    logoAscii = logoAscii.Replace("{version}", ReleaseInformationHelper.Version.ToString());
                    logoAscii = logoAscii.Replace("{architecture}", ReleaseInformationHelper.Architecture);

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
