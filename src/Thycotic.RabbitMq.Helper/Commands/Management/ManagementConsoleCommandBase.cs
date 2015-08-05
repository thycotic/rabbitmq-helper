using System.IO;
using Thycotic.CLI.Commands;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Commands.Installation;

namespace Thycotic.RabbitMq.Helper.Commands.Management
{
    internal abstract class ManagementConsoleCommandBase : CommandBase, IImmediateCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(ManagementConsoleCommandBase));


        protected string Executable { get { return "rabbitmqctl.bat"; } }

        protected string WorkingPath
        {
            get { return InstallationConstants.RabbitMq.BinPath; }
        }

        protected string ExecutablePath 
        {
            get { return Path.Combine(WorkingPath, Executable); }
        }


        protected ManagementConsoleCommandBase()
        {
            if (File.Exists(ExecutablePath)) return;

            _log.Debug("No executable found");
            
        }


    }
}