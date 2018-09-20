using System;
using System.IO;
using System.Runtime.Serialization;
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

        /// <summary>
        /// Validates the output.
        /// </summary>
        /// <param name="expectedOutput">The expected output.</param>
        /// <param name="actualOutput">The actual output.</param>
        /// <param name="strict">if set to <c>true</c> [strict].</param>
        /// <exception cref="InvalidRabbitMqBatOutputException"></exception>
        public virtual void ValidateOutput(string expectedOutput, string actualOutput, bool strict = true)
        {
            if (string.IsNullOrWhiteSpace(actualOutput))
            {
                throw new InvalidRabbitMqBatOutputException(expectedOutput, "No output received");
            }

            if (strict && !actualOutput.Trim().Equals(expectedOutput, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidRabbitMqBatOutputException(expectedOutput, actualOutput);
            }

            if (!strict && !actualOutput.ToLower().Contains(expectedOutput.ToLower()))
            {
                throw new InvalidRabbitMqBatOutputException(expectedOutput, actualOutput);
            }
        }
    }
}
