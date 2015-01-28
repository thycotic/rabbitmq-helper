using System;
using Autofac;
using Thycotic.Logging;

namespace Thycotic.SecretServerAgent2
{
    public class StartupMessageWriter : IStartable
    {
        private readonly ILogWriter _log = Log.Get(typeof(StartupMessageWriter));

        public void Start()
        {
            _log.Info("Application is starting...");
        }
    }
}
