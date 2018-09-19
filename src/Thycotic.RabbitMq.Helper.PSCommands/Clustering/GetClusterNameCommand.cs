using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest;
using Thycotic.RabbitMq.Helper.PSCommands.Management;

namespace Thycotic.RabbitMq.Helper.PSCommands.Clustering
{
    /// <summary>
    ///     Gets the current cluster name
    /// </summary>
    /// <para type="synopsis"> Gets the current cluster name</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Remove-AllQueues</code>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "ClusterName")]
    public class GetClusterNameCommand : RestManagementConsoleCmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqRestClient(BaseUrl, AdminCredential.UserName,
                AdminCredential.GetNetworkCredential().Password);

            var clusterName = client.GetClusterName();

            WriteObject(clusterName);
        }

    }
}