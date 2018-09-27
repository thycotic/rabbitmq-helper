using System.Linq;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Gets all queues in the cluster.
    /// </summary>
    /// <para type="synopsis">Gets all queues in the cluster.</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Get-RabbitMqQueues</code>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "RabbitMqQueues")]
    public class GetRabbitMqQueuesCommand : RestManagementConsoleCmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqRestClient(BaseUrl, AdminCredential.UserName,
                AdminCredential.GetNetworkCredential().Password);
            var queues = client.GetAllQueues().ToArray();

           WriteObject(queues);
        }

    }
}