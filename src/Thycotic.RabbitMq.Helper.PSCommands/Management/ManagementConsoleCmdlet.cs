using System.IO;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.PSCommands.Installation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    /// Base class for a management console command
    /// </summary>
    public abstract class ManagementConsoleCmdlet : Cmdlet
    {

        /// <summary>
        /// Gets the executable.
        /// </summary>
        /// <value>
        /// The executable.
        /// </value>
        protected string Executable { get { return "rabbitmqctl.bat"; } }

        /// <summary>
        /// Gets the working path.
        /// </summary>
        /// <value>
        /// The working path.
        /// </value>
        protected string WorkingPath
        {
            get { return InstallationConstants.RabbitMq.BinPath; }
        }

        /// <summary>
        /// Gets the executable path.
        /// </summary>
        /// <value>
        /// The executable path.
        /// </value>
        protected string ExecutablePath
        {
            get { return Path.Combine(WorkingPath, Executable); }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementConsoleCmdlet"/> class.
        /// </summary>
        protected ManagementConsoleCmdlet()
        {
            if (File.Exists(ExecutablePath)) return;

            //some tools make instances of the commandlets to document them.
            //if rabbit mq is not installer when the tool run, this code will blow up.
            if (CommandRuntime != null)
            {
                WriteWarning("No management executable found");
            }


        }
    }
}