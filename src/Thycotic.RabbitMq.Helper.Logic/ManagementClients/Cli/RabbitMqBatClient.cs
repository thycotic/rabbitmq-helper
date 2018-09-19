using System;
using System.IO;
using Thycotic.RabbitMq.Helper.Logic.OS;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli
{
    /// <summary>
    /// CTL RabbitMqProcess interactor 
    /// </summary>
    /// <seealso cref="IProcessInteractor" />
    public abstract class RabbitMqBatClient
    {
        /// <summary>
        /// Standard exception messages
        /// </summary>
        public static class ExceptionMessages
        {
            /// <summary>
            /// The invalid output
            /// </summary>
            public const string InvalidOutput = "Invalid output received";
        }

        /// <summary>
        ///     Gets the executable.
        /// </summary>
        /// <value>
        ///     The executable.
        /// </value>
        protected abstract string Executable { get; }

        /// <summary>
        ///     Gets the working path.
        /// </summary>
        /// <value>
        ///     The working path.
        /// </value>
        protected string WorkingPath => InstallationConstants.RabbitMq.BinPath;

        /// <summary>
        ///     Gets the executable path.
        /// </summary>
        /// <value>
        ///     The executable path.
        /// </value>
        protected string ExecutablePath => Path.Combine(WorkingPath, Executable);

        /// <summary>
        /// Gets a value indicating whether this <see cref="IProcessInteractor" /> is exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        public bool Exists => File.Exists(ExecutablePath);
        

        /// <summary>
        /// Invokes the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="estimatedProcessDuration">Duration of the estimated process.</param>
        /// <returns></returns>
        public string Invoke(string parameters, TimeSpan estimatedProcessDuration)
        {
            var externalProcessRunner = new ExternalProcessRunner
            {
                EstimatedProcessDuration = estimatedProcessDuration
            };

            return externalProcessRunner.Run(ExecutablePath, WorkingPath, parameters);
        }
    }
}
