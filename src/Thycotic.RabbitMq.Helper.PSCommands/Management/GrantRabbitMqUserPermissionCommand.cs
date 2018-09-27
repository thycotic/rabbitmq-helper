using System;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;
using Thycotic.RabbitMq.Helper.Logic.OS;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Grants a RabbitMq permissions
    /// </summary>
    /// <para type="synopsis">Grants a RabbitMq permissions</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>New-BasicRabbitMqUser</code>
    /// </example>
    [Cmdlet(VerbsSecurity.Grant, "RabbitMqUserPermission")]
    public class GrantRabbitMqUserPermissionCommand : Cmdlet
    {
        /// <summary>
        /// Gets or sets the virtual host.
        /// </summary>
        /// <value>
        /// The virtual host.
        /// </value>
        /// <para type="description">Gets or sets the virtual host.</para>
        [Parameter(
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [Alias("VHost")]
        public string VirtualHost { get; set; }

        /// <summary>
        ///     Gets or sets the name of the rabbit mq user.
        /// </summary>
        /// <value>
        ///     The name of the rabbit mq user.
        /// </value>
        /// <para type="description">Gets or sets the name of the rabbit mq user.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        [Alias("RabbitMqUserName")]
        public string UserName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrantRabbitMqUserPermissionCommand"/> class.
        /// </summary>
        public GrantRabbitMqUserPermissionCommand()
        {
            VirtualHost = "/";
        }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        /// <exception cref="System.Exception">
        ///     Failed to create user. Manual creation might be necessary
        ///     or
        ///     Failed to grant permissions to user. Manual grant might be necessary
        /// </exception>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqBatCtlClient();

            WriteVerbose($"Granting permissions to user {UserName}");
            
            client.GrantPermissionsToUser(VirtualHost, UserName);
          
        }
    }
}