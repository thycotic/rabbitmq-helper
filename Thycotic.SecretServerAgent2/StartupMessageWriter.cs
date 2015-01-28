using System;
using System.IO;
using System.Reflection;
using Autofac;
using Thycotic.Logging;

namespace Thycotic.SecretServerAgent2
{
    public class StartupMessageWriter : IStartable
    {
        private readonly ILogWriter _log = Log.Get(typeof(StartupMessageWriter));

        public void Start()
        {
            var logoStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Thycotic.SecretServerAgent2.logo.txt");

            if (logoStream != null)
            {
                var sr = new StreamReader(logoStream);
                var logoAscii = sr.ReadToEnd();
            
                //don't use the log since it just spams the table
                Console.WriteLine(logoAscii);
            }
            else
            {
                _log.Warn("Could not locate terminal logo");
            }

            _log.Info("Application is starting...");
        }
    }
}
