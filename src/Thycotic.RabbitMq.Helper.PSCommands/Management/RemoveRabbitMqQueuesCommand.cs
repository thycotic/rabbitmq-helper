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
    ///     Removes all non-autodelete and non-exclusive queues in the RabbitMq cluster.
    /// </summary>
    /// <para type="synopsis">Removes all non-autodelete and non-exclusive queues in the RabbitMq cluster.</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Remove-RabbitMqQueues</code>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "RabbitMqQueues")]
    public class RemoveRabbitMqQueuesCommand : RestManagementConsoleCmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqRestClient(BaseUrl, AdminCredential.UserName,
                AdminCredential.GetNetworkCredential().Password);
            var queues = client.GetAllQueues().ToArray();

            if (!queues.Any())
            {
                WriteVerbose("There are no queues to remove");
                return;
            }

            const int activityid = 7;
            const string activity = "Removing";
            WriteProgress(new ProgressRecord(activityid, activity, "Checking Erlang pre-requisites")
            {
                PercentComplete = 5
            });

            var orderedQueues = queues.OrderBy(q => q.vhost).ThenBy(q => q.name).ToList();
            var c = 0;
            var total = orderedQueues.Count;

            var statuses = new List<KeyValuePair<string, string>>();

            orderedQueues.ForEach(q =>
            {
                if (q.auto_delete || q.exclusive)
                {
                    WriteVerbose($"Skipping {q.name} on {q.vhost}");
                    c++;
                    return;
                }

                WriteProgress(new ProgressRecord(activityid, activity, $"Removing {q.name} on {q.vhost}")
                {
                    PercentComplete = Convert.ToInt32(Convert.ToDouble(c) / total * 100)
                });
                WriteVerbose($"Removing {q.name} on {q.vhost}");

                try
                {
                    client.DeleteQueue(q.vhost, q.name);
                    statuses.Add(new KeyValuePair<string, string>(q.name, HttpStatusCode.OK.ToString()));

                }
                catch (Exception ex)
                {
                    statuses.Add(new KeyValuePair<string, string>(q.name, ex.Message));
                }

                c++;
            });

            WriteObject(statuses);

            WriteProgress(new ProgressRecord(activityid, activity, "Removed all queues")
            {
                PercentComplete = 100,
                RecordType = ProgressRecordType.Completed
            });
        }

    }
}