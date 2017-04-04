using System;
using System.Management.Automation;
using Thycotic.Utility.OS;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Adds a basic user to RabbitMq
    /// </summary>
    /// <para type="synopsis">TODO: This is the cmdlet synopsis.</para>
    /// <para type="description">TODO: This is part of the longer cmdlet description.</para>
    /// <para type="description">TODO: Also part of the longer cmdlet description.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">TODO: Get-Help</para>
    /// <example>
    ///     <para>TODO: This is part of the first example's introduction.</para>
    ///     <para>TODO: This is also part of the first example's introduction.</para>
    ///     <code>TODO: New-Thingy | Write-Host</code>
    ///     <para>TODO: This is part of the first example's remarks.</para>
    ///     <para>TODO: This is also part of the first example's remarks.</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "BasicRabbitMqUser")]
    [Alias("addRabbitMqUser")]
    public class NewBasicRabbitMqUserCommand : ManagementConsoleCmdlet
    {
        /// <summary>
        ///     Gets or sets the name of the rabbit mq user.
        /// </summary>
        /// <value>
        ///     The name of the rabbit mq user.
        /// </value>
        /// <para type="description">TODO: Property description.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        [Alias("RabbitMqUserName")]
        public string UserName { get; set; }

        /// <summary>
        ///     Gets or sets the rabbit mq password.
        /// </summary>
        /// <value>
        ///     The rabbit mq password.
        /// </value>
        /// <para type="description">TODO: Property description.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        [Alias("RabbitMqPw", "RabbitMqPassword")]
        public string Password { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        /// <exception cref="System.ApplicationException">
        ///     Failed to create user. Manual creation might be necessary
        ///     or
        ///     Failed to grant permissions to user. Manual grant might be necessary
        /// </exception>
        protected override void ProcessRecord()
        {
            var externalProcessRunner = new ExternalProcessRunner
            {
                EstimatedProcessDuration = TimeSpan.FromSeconds(15)
            };

            WriteVerbose(string.Format("Adding limited-access user {0}", UserName));

            var parameters2 = string.Format("add_user {0} {1}", UserName, Password);

            try
            {
                externalProcessRunner.Run(ExecutablePath, WorkingPath, parameters2);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create user. Manual creation might be necessary", ex);
            }

            WriteVerbose(string.Format("Granting permissions to user {0}", UserName));

            parameters2 = string.Format("set_permissions -p / {0} \".*\" \".*\" \".*\"", UserName);

            try
            {
                externalProcessRunner.Run(ExecutablePath, WorkingPath, parameters2);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to grant permissions to user. Manual grant might be necessary",
                    ex);
            }
        }
    }
}