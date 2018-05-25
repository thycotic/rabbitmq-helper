using System;
using System.Management.Automation;
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
        ///     Gets or sets the rabbit mq password.
        /// </summary>
        /// <value>
        ///     The rabbit mq password.
        /// </value>
        /// <para type="description">Gets or sets the rabbit mq password.</para>
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
            var ctlInteractor = new CtlRabbitMqProcessInteractor();

            WriteVerbose($"Adding limited-access user {UserName}");

            var parameters2 = $"add_user {UserName} {Password}";

            try
            {
                var  output = ctlInteractor.Invoke(parameters2, TimeSpan.FromSeconds(30));
                WriteVerbose(output);
                if (output != $"Adding user \"{UserName}\" ...")
                {
                    throw new ApplicationException(CtlRabbitMqProcessInteractor.ExceptionMessages.InvalidOutput);
                }


            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to create user. Manual creation might be necessary", ex);
            }
        }
    }
}