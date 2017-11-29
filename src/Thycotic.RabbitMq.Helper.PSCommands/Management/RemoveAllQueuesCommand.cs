using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
            if (getResponse.StatusCode != HttpStatusCode.OK)
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

            var byteArray = new UTF8Encoding().GetBytes(string.Format("{0}:{1}", AdminUserName, AdminPassword));
            using (var httpClient = new HttpClient(new HttpClientHandler(), true))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
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

                    var host = string.IsNullOrEmpty(q.VHost) || q.VHost.Equals("/") ? "%2f" : q.VHost;
                    var requestUri = new Uri(string.Format(BaseUrl + "/api/queues/{0}/{1}", host, q.Name));
                    ForceCanonicalPathAndQuery(requestUri); //ZL - Needed becuase virtual host isn't used and "/" was not being encoded properly, need to literally pass %2f

                    var response = Task.FromResult(httpClient.DeleteAsync(requestUri)).Result.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        WriteObject(new KeyValuePair<string, HttpStatusCode>(q.Name, response.StatusCode));
                    }
                    else
                    {
                        WriteWarning(string.Format("{0}{0}StatusCode: {1}{0}ReasonPhrase: {2}{0}RequestUri: {3}{0}"
                            , Environment.NewLine
                            , response.StatusCode
                            , response.ReasonPhrase
                            , response.RequestMessage.RequestUri
                        ));
                    }
                    c++;
                });
            }

            WriteProgress(new ProgressRecord(activityid, activity, "Removed all queues")
            {
                PercentComplete = 100,
                RecordType = ProgressRecordType.Completed
            });
        }

        //https://stackoverflow.com/questions/781205/getting-a-url-with-an-url-encoded-slash
        private void ForceCanonicalPathAndQuery(Uri uri)
        {
            var paq = uri.PathAndQuery; // need to access PathAndQuery
            var flagsFieldInfo = typeof(Uri).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
            var flags = (ulong)flagsFieldInfo.GetValue(uri);
            flags &= ~(ulong)0x30; // Flags.PathNotCanonical|Flags.QueryNotCanonical
            flagsFieldInfo.SetValue(uri, flags);
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