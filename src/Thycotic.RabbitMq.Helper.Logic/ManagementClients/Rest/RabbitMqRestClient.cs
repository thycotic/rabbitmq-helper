using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest
{
    /// <summary>
    /// REST management client
    /// </summary>
    public class RabbitMqRestClient : IRabbitMqRestClient
    {
        private readonly RestClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqRestClient"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public RabbitMqRestClient(string baseUrl, string userName, string password)
        {
            _client = new RestClient(baseUrl) { Authenticator = new HttpBasicAuthenticator(userName, password) };

            _client.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
            _client.AddHandler("text/json", NewtonsoftJsonSerializer.Default);
            _client.AddHandler("text/x-json", NewtonsoftJsonSerializer.Default);
            _client.AddHandler("text/javascript", NewtonsoftJsonSerializer.Default);
            _client.AddHandler("*+json", NewtonsoftJsonSerializer.Default);

        }

        /// <summary>
        /// Executes the specified resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="method">The method.</param>
        /// <param name="body">The body.</param>
        /// <exception cref="Exception"></exception>
        public void Execute(string resource, Method method = Method.POST, object body = null)
        {
            Execute<object>(resource, method, body);
        }

        /// <summary>
        /// Executes the specified resource.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <param name="method">The method.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T Execute<T>(string resource, Method method = Method.POST, object body = null) where T : new()
        {
            var request = new RestRequest(resource, method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };

            if (body != null)
            {
                request.AddJsonBody(body);
            }
            var response = _client.Execute<T>(request);
            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            if (method == Method.PUT &&
                (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent))
            {
            }
            else if (method == Method.DELETE &&
                response.StatusCode == HttpStatusCode.NoContent)
            {
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
            }
            else
            {
                throw new Exception(response.StatusDescription);
            }

            return response.Data;
        }

        //source: https://bytefish.de/blog/restsharp_custom_json_serializer/
        private class NewtonsoftJsonSerializer : ISerializer, IDeserializer
        {
            private readonly Newtonsoft.Json.JsonSerializer _serializer;

            private NewtonsoftJsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
            {
                _serializer = serializer;
            }

            public string ContentType
            {
                get { return "application/json"; } // Probably used for Serialization?
                set { }
            }

            public string DateFormat { get; set; }

            public string Namespace { get; set; }

            public string RootElement { get; set; }

            public string Serialize(object obj)
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                    {
                        _serializer.Serialize(jsonTextWriter, obj);

                        return stringWriter.ToString();
                    }
                }
            }

            public T Deserialize<T>(IRestResponse response)
            {
                var content = response.Content;

                using (var stringReader = new StringReader(content))
                {
                    using (var jsonTextReader = new JsonTextReader(stringReader))
                    {
                        return _serializer.Deserialize<T>(jsonTextReader);
                    }
                }
            }

            public static NewtonsoftJsonSerializer Default => new NewtonsoftJsonSerializer(new Newtonsoft.Json.JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        /// <inheritdoc />
        public IEnumerable<Queue> GetAllQueues()
        {
            return Execute<List<Queue>>("api/queues", Method.GET);
        }

        /// <inheritdoc />
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


        /// <inheritdoc />
        public string GetClusterName()
        {
            var clusterName = Execute<ClusterName>("api/cluster-name", Method.GET);
            return clusterName.name;
        }

        /// <inheritdoc />
        public IEnumerable<Node> GetClusterNodes()
        {
            return Execute<List<Node>>("api/nodes", Method.GET);
        }

        /// <inheritdoc />
        public NodeHealthCheck GetHealthCheck()
        {

            var healthCheck = Execute<NodeHealthCheck>("api/healthchecks/node", Method.GET);
            return healthCheck;
        }

        /// <inheritdoc />
        public void CreatePolicy(string vhost, string name, Policy policy)
        {
            var host = string.IsNullOrEmpty(vhost) || vhost.Equals("/") ? "%2f" : vhost;
            var resource = $"api/policies/{host}/{name}";

            Execute(resource, Method.PUT, policy);
        }

        /// <inheritdoc />
        public void CreateFederationUpstream(string vhost, string name, FederationUpstream upstream)
        {
            var host = string.IsNullOrEmpty(vhost) || vhost.Equals("/") ? "%2f" : vhost;
            var resource = $"api/parameters/federation-upstream/{host}/{name}";

            //HACK: The federation api wants the payload to be in a value element
            var upstreamValue = new {value = upstream};

            Execute(resource, Method.PUT, upstreamValue);
        }
    }
}
