using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Commands.Installation;
using Thycotic.Utility.OS;

namespace Thycotic.RabbitMq.Helper.Commands.Management
{
    internal class TailRabbitMqLog : ManagementConsoleCommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof(TailRabbitMqLog));

        public override string Area
        {
            get { return CommandAreas.Management; }
        }

        public override string[] Aliases {
            get { return new[] {"taillog"}; }
            set { }
        }

        public override string Description
        {
            get { return "Prints out the tail of the RabbitMq log"; }
        }

        public TailRabbitMqLog()
        {
            string logFile = string.Format("rabbit@{0}.log", Environment.MachineName);
            var logPath = Path.Combine(InstallationConstants.RabbitMq.ConfigurationPath, "log", logFile);
            const int linesToPrint = 50;

            Action = parameters =>
            {
                _log.Info(string.Format("Printing tail for {0}", logPath));

                var lockedFileReader = new LockedFileReader(logPath);

                var lines = lockedFileReader.GetTailLines(linesToPrint);

                lines.ToList().ForEach(Console.WriteLine);

                return 0;
            };
        }
    }
}