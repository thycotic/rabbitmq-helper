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
        /// <inheritdoc />
        protected override string Executable => "rabbitmqctl.bat";


        /// <inheritdoc />
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
                    throw new Exception("Failed to get RabbitMq uptime information. RabbitMq is probably not running");
                }

                return true;
            }
        }


        /// <inheritdoc />
        public void SoftStart()
        {
            try
            {
                var output = Invoke("start_app", TimeSpan.FromSeconds(15));
                
                ValidateOutput($"Starting node rabbit@{Environment.MachineName} ...", output);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to start RabbitMq", ex);
            }
            
        }

        /// <inheritdoc />
        public void SoftStop()
        {
            try
            {
                var output = Invoke("stop_app", TimeSpan.FromSeconds(15));

                ValidateOutput($"Stopping rabbit application on node rabbit@{Environment.MachineName} ...", output);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to start RabbitMq", ex);
            }
        }


        /// <inheritdoc />
        public void HardStop()
        {
            try
            {
                var output = Invoke("stop", TimeSpan.FromSeconds(15));

                ValidateOutput($"Stopping and halting node rabbit@{Environment.MachineName} ...", output);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to stop RabbitMq", ex);
            }
        }

        /// <summary>
        /// Creates the limited access user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="Exception">
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
                throw new Exception("Failed to create user. Manual creation might be necessary", ex);
            }
        }

        /// <summary>
        /// Grants the permissions to user.
        /// </summary>
        /// <param name="virtualHost">The virtual host.</param>
        /// <param name="userName">Name of the user.</param>
        /// <exception cref="Exception">
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
                throw new Exception("Failed to grant permissions to user. Manual grant might be necessary",
                    ex);
            }
        }
    }
}
