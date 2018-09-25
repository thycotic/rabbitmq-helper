using System;
using Thycotic.RabbitMq.Helper.Logic.OS;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli
{
    /// <summary>
    /// CTL RabbitMqProcess interactor 
    /// </summary>
    /// <seealso cref="IProcessInteractor" />
    public class RabbitMqBatPluginClient : RabbitMqBatClient
    {

        /// <summary>
        ///     Gets the executable.
        /// </summary>
        /// <value>
        ///     The executable.
        /// </value>
        protected override string Executable => "rabbitmq-plugins.bat";


        /// <summary>
        /// Enables the management UI.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public string EnableManagementUi()
        {
            const string parameters2 = "enable rabbitmq_management";

            var output = Invoke(parameters2, TimeSpan.FromSeconds(60));

            ValidateOutput($"Enabling plugins on node rabbit@{Environment.MachineName}:", output, false);
            try
            {
                ValidateOutput("The following plugins have been enabled:", output, false);
                ValidateOutput("rabbitmq_management", output, false);
                ValidateOutput("rabbitmq_management_agent", output, false);
                ValidateOutput("rabbitmq_web_dispatch", output, false);
            }
            catch (Exception)
            {
                ValidateOutput("Plugin configuration unchanged.", output, false);
            }

            return output;
        }

        /// <summary>
        /// Enables the management UI.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public string DisableManagementUi()
        {
            const string parameters2 = "disable rabbitmq_management";

            var output = Invoke(parameters2, TimeSpan.FromSeconds(60));

            ValidateOutput($"Disabling plugins on node rabbit@{Environment.MachineName}:", output, false);
            try
            {
                ValidateOutput("The following plugins have been disabled:", output, false);
                ValidateOutput("rabbitmq_management", output, false);
                ValidateOutput("rabbitmq_management_agent", output, false);
                ValidateOutput("rabbitmq_web_dispatch", output, false);
            }
            catch (Exception)
            {
                ValidateOutput("Plugin configuration unchanged.", output, false);
            }

            return output;
        }

        /// <summary>
        /// Enables the federation and management UI.
        /// </summary>
        /// <returns></returns>
        public string EnableFederationAndManagementUi()
        {
            const string parameters = "enable rabbitmq_federation";

            var output = Invoke(parameters, TimeSpan.FromSeconds(60));

            ValidateOutput($"Enabling plugins on node rabbit@{Environment.MachineName}:", output, false);
            try
            {
                ValidateOutput("The following plugins have been enabled:", output, false);
                ValidateOutput("rabbitmq_federation", output, false);
            }
            catch (Exception)
            {
                ValidateOutput("Plugin configuration unchanged.", output, false);
            }

            const string parameters2 = "enable rabbitmq_federation_management";

            var output2 = Invoke(parameters2, TimeSpan.FromSeconds(60));

            ValidateOutput($"Enabling plugins on node rabbit@{Environment.MachineName}:", output2, false);
            try
            {
                ValidateOutput("The following plugins have been enabled:", output2, false);
                ValidateOutput("rabbitmq_federation_management", output2, false);
            }
            catch (Exception)
            {
                ValidateOutput("Plugin configuration unchanged.", output, false);
            }
            return output + output2;
        }

        /// <summary>
        /// Disables the federation and management UI.
        /// </summary>
        /// <returns></returns>
        public string DisableFederationAndManagementUi()
        {
            const string parameters = "disable rabbitmq_federation_management";

            var output = Invoke(parameters, TimeSpan.FromSeconds(60));

            ValidateOutput($"Disabling plugins on node rabbit@{Environment.MachineName}:", output, false);
            try
            {

                ValidateOutput("The following plugins have been disabled:", output, false);
                ValidateOutput("rabbitmq_federation_management", output, false);
            }
            catch (Exception)
            {
                ValidateOutput("Plugin configuration unchanged.", output, false);
            }

            const string parameters2 = "disable rabbitmq_federation";

            var output2 = Invoke(parameters2, TimeSpan.FromSeconds(60));

            ValidateOutput($"Disabling plugins on node rabbit@{Environment.MachineName}:", output2, false);
            try
            {
                ValidateOutput("The following plugins have been disabled:", output2, false);
                ValidateOutput("rabbitmq_federation", output2, false);
            }
            catch (Exception)
            {
                ValidateOutput("Plugin configuration unchanged.", output, false);
            }
            return output + output2;
        }
    }
}
