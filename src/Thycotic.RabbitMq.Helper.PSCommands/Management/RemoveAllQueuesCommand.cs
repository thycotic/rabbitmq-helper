using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Deletes all queues in the current instance of RabbitMq
    /// </summary>
    /// <para type="synopsis">Deletes all queues in the current instance of RabbitMq</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Remove-AllQueues</code>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "AllQueues")]
    public class RemoveAllQueuesCommand : RestManagementConsoleCmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RestClient(BaseUrl) { Authenticator = new HttpBasicAuthenticator(AdminUserName, AdminPassword) };

            var getRequest = new RestRequest("api/queues");

            var getResponse = client.Execute<List<QueueSlim>>(getRequest);

            if (getResponse.ErrorException != null)
            {
                throw getResponse.ErrorException;
            }
            else if (getResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(getResponse.StatusDescription);
            }

            if (!getResponse.Data.Any())
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

            var queues = getResponse.Data.OrderBy(q => q.VHost).ThenBy(q => q.Name).ToList();

            var c = 0;
            var total = queues.Count;
            queues.ForEach(q =>
            {
                if (q.AutoDelete || q.Exclusive)
                {
                    WriteVerbose(string.Format("Skipping {0} on {1}", q.Name, q.VHost));
                    c++;
                    return;
                }

                WriteProgress(new ProgressRecord(activityid, activity, string.Format("Removing {0} on {1}", q.Name, q.VHost))
                {
                    PercentComplete = Convert.ToInt32(Convert.ToDouble(c) / total * 100)
                });
                WriteVerbose(string.Format("Removing {0} on {1}", q.Name, q.VHost));

                var deleteRequest = new RestRequest("api/queues/{host}/{name}", Method.DELETE);
                deleteRequest.AddUrlSegment("host", q.VHost);
                deleteRequest.AddUrlSegment("name", q.Name);

                var deleteResponse = client.Execute(deleteRequest);

                if (deleteResponse.ErrorException != null)
                {
                    WriteWarning(deleteResponse.ErrorException.Message);
                }
                else if (getResponse.StatusCode != HttpStatusCode.OK)
                {
                    WriteWarning(getResponse.StatusDescription);
                }
                else
                {
                    WriteObject(new KeyValuePair<string, HttpStatusCode>(q.Name, deleteResponse.StatusCode));
                }
                c++;
            });

            WriteProgress(new ProgressRecord(activityid, activity, "Removed all queues")
            {
                PercentComplete = 100,
                RecordType = ProgressRecordType.Completed
            });
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class QueueSlim
        {
            public string Name { get; set; }
            public string VHost { get; set; }
            public int Messages { get; set; }
            public bool Exclusive { get; set; }
            public bool AutoDelete { get; set; }
        }
    }
}