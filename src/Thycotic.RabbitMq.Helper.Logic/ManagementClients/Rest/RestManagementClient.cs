using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest
{
    /// <summary>
    /// REST management client
    /// </summary>
    public class RestManagementClient
    {
        private readonly RestClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestManagementClient"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public RestManagementClient(string baseUrl, string userName, string password)
        {
            _client = new RestClient(baseUrl) { Authenticator = new HttpBasicAuthenticator(userName, password) };
        }

        /// <summary>
        /// Executes the specified resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="method">The method.</param>
        /// <exception cref="ApplicationException"></exception>
        public void Execute(string resource, Method method = Method.POST)
        {
            var getRequest = new RestRequest(resource, method);
            var getResponse = _client.Execute(getRequest);
            if (getResponse.ErrorException != null)
            {
                throw getResponse.ErrorException;
            }
            if (method != Method.DELETE && getResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(getResponse.StatusDescription);
            }
        }

        /// <summary>
        /// Executes the specified resource.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public T Execute<T>(string resource, Method method = Method.POST) where T : new()
        {
            var getRequest = new RestRequest(resource, method);
            var getResponse = _client.Execute<T>(getRequest);
            if (getResponse.ErrorException != null)
            {
                throw getResponse.ErrorException;
            }
            if (getResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException(getResponse.StatusDescription);
            }

            return getResponse.Data;
        }

        /// <summary>
        /// Gets all queues.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Queue> GetAllQueues()
        {
            return Execute<List<Queue>>("api/queues", Method.GET);
        }

        /// <summary>
        /// Removes the queue.
        /// </summary>
        /// <param name="vhost">The vhost.</param>
        /// <param name="name">The name.</param>
        public void DeleteQueue(string vhost, string name)
        {
            var host = string.IsNullOrEmpty(vhost) || vhost.Equals("/") ? "%2f" : vhost;
            var resource = $"api/queues/{host}/{name}";

            //var requestUri = new Uri(new Uri(BaseUrl), resource);
            //ForceCanonicalPathAndQuery(requestUri); //ZL - Needed because virtual host isn't used and "/" was not being encoded properly, need to literally pass %2f

            Execute(resource, Method.DELETE);
        }


        //TODO: Remove this unless deletion starts failing again on some hosts.
        ////https://stackoverflow.com/questions/781205/getting-a-url-with-an-url-encoded-slash
        //private void ForceCanonicalPathAndQuery(Uri uri)
        //{
        //    var paq = uri.PathAndQuery; // need to access PathAndQuery
        //    var flagsFieldInfo = typeof(Uri).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
        //    var flags = (ulong)flagsFieldInfo.GetValue(uri);
        //    flags &= ~(ulong)0x30; // Flags.PathNotCanonical|Flags.QueryNotCanonical
        //    flagsFieldInfo.SetValue(uri, flags);
        //}


        /// <summary>
        /// Gets the name of the cluster.
        /// </summary>
        /// <returns></returns>
        public string GetClusterName()
        {
            var clusterName = Execute<ClusterName>("api/cluster-name", Method.GET);
            return clusterName.name;
        }

        /// <summary>
        /// Gets the health check.
        /// </summary>
        /// <returns></returns>
        public NodeHealthCheck GetHealthCheck()
        {

            var healthCheck = Execute<NodeHealthCheck>("api/healthchecks/node", Method.GET);
            return healthCheck;
        }
    }
}
