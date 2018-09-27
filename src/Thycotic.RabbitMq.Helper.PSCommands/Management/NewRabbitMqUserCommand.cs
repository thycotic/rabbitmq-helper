using System;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;
using Thycotic.RabbitMq.Helper.Logic.OS;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Adds a basic user to RabbitMq. This user has no permissions
    /// </summary>
    /// <para type="synopsis">Adds a basic user to RabbitMq. This user has no permissions</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>New-RabbitMqUser</code>
    /// </example>
    [Cmdlet(VerbsCommon.New, "RabbitMqUser")]
    public class NewRabbitMqUserCommand : Cmdlet
    {

        /// <summary>
        ///     Gets or sets the credential of the rabbit mq user.
        /// </summary>
        /// <value>
        ///     The credential of the rabbit mq user.
        /// </value>
        /// <para type="description">Gets or sets the credential of the rabbit mq user.</para>
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public PSCredential Credential { get; set; }

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

            WriteVerbose($"Adding limited-access user {Credential.UserName}");

            client.CreateLimitedAccessUser(Credential.UserName, Credential.GetNetworkCredential().Password);
        }
    }
}