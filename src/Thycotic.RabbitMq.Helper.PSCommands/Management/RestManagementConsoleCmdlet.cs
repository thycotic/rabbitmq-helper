using System.Management.Automation;
using System.Net;
using System.Security;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Base class for a management console command
    /// </summary>
    public abstract class RestManagementConsoleCmdlet : Cmdlet
    {
        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string BaseUrl { get; set; }

        /// <summary>
        ///     Gets or sets the credential of the rabbit mq administrator user.
        /// </summary>
        /// <value>
        ///     The credential of the rabbit mq user.
        /// </value>
        /// <para type="description">Gets or sets the credential of the rabbit mq user.</para>
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public PSCredential AdminCredential { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RestManagementConsoleCmdlet" /> class.
        /// </summary>
        protected RestManagementConsoleCmdlet()
        {
            BaseUrl = "http://localhost:15672";
            AdminCredential = new PSCredential("guest", new NetworkCredential("", "guest").SecurePassword);
        }

    }
}