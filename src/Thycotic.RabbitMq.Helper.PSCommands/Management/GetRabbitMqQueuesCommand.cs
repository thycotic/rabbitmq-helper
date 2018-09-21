using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models;

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