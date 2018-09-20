using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest;
using Thycotic.RabbitMq.Helper.PSCommands.Management;

namespace Thycotic.RabbitMq.Helper.PSCommands.Clustering
{
    /// <summary>
    ///     Joins a RabbitMq cluster
    /// </summary>
    /// <para type="synopsis"> Joins a RabbitMq cluster</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Join-RabbitMqCluster</code>
    /// </example>
    [Cmdlet(VerbsCommon.Join, "RabbitMqCluster")]
    public class JoinRabbitMqClusterCommand : RestManagementConsoleCmdlet
    {
        /// <summary>
        /// Gets or sets the content of the cookie.
        /// </summary>
        /// <value>
        /// The content of the cookie.
        /// </value>
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string CookieContent { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            
            
        }

    }
}