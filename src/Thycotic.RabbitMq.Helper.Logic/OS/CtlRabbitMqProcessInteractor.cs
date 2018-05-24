using System;
using System.IO;
using Thycotic.RabbitMq.Helper.Logic.Reflection;

namespace Thycotic.RabbitMq.Helper.Logic.OS
{
    /// <summary>
    /// CTL RabbitMqProcess interactor 
    /// </summary>
    /// <seealso cref="IProcessInteractor" />
    public class CtlRabbitMqProcessInteractor : IProcessInteractor
    {

        /// <summary>
        ///     Gets the executable.
        /// </summary>
        /// <value>
        ///     The executable.
        /// </value>
        protected string Executable => "rabbitmqctl.bat";

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
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Stop()
        {
            Invoke("stop", TimeSpan.FromSeconds(15));
        }

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
