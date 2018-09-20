using System;
using System.Threading;
using Thycotic.RabbitMq.Helper.Logic.OS;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli
{
    /// <summary>
    /// CTL RabbitMqProcess interactor 
    /// </summary>
    /// <seealso cref="IProcessInteractor" />
    public class RabbitMqBatCtlClient : RabbitMqBatClient, IProcessInteractor
    {
       
        /// <summary>
        ///     Gets the executable.
        /// </summary>
        /// <value>
        ///     The executable.
        /// </value>
        protected override string Executable => "rabbitmqctl.bat";

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
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
                var output = string.Empty;

                while (!output.Contains("uptime") && !cts.IsCancellationRequested)
                {

                    var parameters2 = "status";

                    try
                    {
                        output = Invoke(parameters2, TimeSpan.FromSeconds(30));
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                if (!output.Contains("uptime"))
                {
                    throw new ApplicationException("Failed to get RabbitMq uptime information. RabbitMq is probably not running");
                }

                return true;
            }
        }
        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Start()
        {
            try
            {
                var output = Invoke("start_app", TimeSpan.FromSeconds(15));
                
                ValidateOutput($"Starting node rabbit@{Environment.MachineName} ...", output);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to start RabbitMq", ex);
            }
            
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Stop()
        {
            try
            {
                var output = Invoke("stop", TimeSpan.FromSeconds(15));

                ValidateOutput($"Stopping and halting node rabbit@{Environment.MachineName} ...", output);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to stop RabbitMq", ex);
            }
        }

        /// <summary>
        /// Creates the limited access user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="ApplicationException">
        /// Failed to create user. Manual creation might be necessary
        /// </exception>
        public void CreateLimitedAccessUser(string userName, string password)
        {
            var parameters2 = $"add_user {userName} {password}";

            try
            {
                var output = Invoke(parameters2, TimeSpan.FromSeconds(30));

                ValidateOutput($"Adding user \"{userName}\" ...", output);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create user. Manual creation might be necessary", ex);
            }
        }

        /// <summary>
        /// Grants the permissions to user.
        /// </summary>
        /// <param name="virtualHost">The virtual host.</param>
        /// <param name="userName">Name of the user.</param>
        /// <exception cref="ApplicationException">
        /// Failed to grant permissions to user. Manual grant might be necessary
        /// </exception>
        public void GrantPermissionsToUser(string virtualHost, string userName)
        {
            var parameters2 = $"set_permissions -p {virtualHost} {userName} \".*\" \".*\" \".*\"";
            
            try
            {
                var output = Invoke(parameters2, TimeSpan.FromSeconds(30));

                ValidateOutput($"Setting permissions for user \"{userName}\" in vhost \"{virtualHost}\" ...", output);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to grant permissions to user. Manual grant might be necessary",
                    ex);
            }
        }
    }
}
