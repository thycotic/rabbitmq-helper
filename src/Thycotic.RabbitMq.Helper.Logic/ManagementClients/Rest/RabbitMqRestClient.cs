using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
            _client = new RestClient(baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(userName, password),
                HttpFactory = new SimpleFactory<HttpWithCanonicalNameSupport>()
            };

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
        /// <param name="urlSegments">The URL segments.</param>
        public void Execute(string resource, Method method = Method.POST, object body = null, IDictionary<string, string> urlSegments = null)
        {
            Execute<object>(resource, method, body, urlSegments);
        }

        /// <summary>
        /// Executes the specified resource.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <param name="method">The method.</param>
        /// <param name="body">The body.</param>
        /// <param name="urlSegments">The URL segments.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T Execute<T>(string resource, Method method = Method.POST, object body = null, IDictionary<string, string> urlSegments = null) where T : new()
        {
            var request = new RestRequest(resource, method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = NewtonsoftJsonSerializer.Default
            };

            urlSegments?.ToList().ForEach(kvp => request.AddUrlSegment(kvp.Key, kvp.Value));

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

        /// <inheritdoc />
        public IEnumerable<Queue> GetAllQueues()
        {
            return Execute<List<Queue>>("api/queues", Method.GET);
        }

        /// <inheritdoc />
        public void DeleteQueue(string vhost, string name)
        {
            var resource = "api/queues/{vhost}/{name}";

            Execute(resource, Method.DELETE,
                urlSegments: new Dictionary<string, string> { { "vhost", vhost }, { "name", name } });
        }

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
            var resource = "api/policies/{vhost}/{name}";

            Execute(resource, Method.PUT, policy, new Dictionary<string, string> { { "vhost", vhost }, { "name", name } });
        }

        /// <inheritdoc />
        public void CreateFederationUpstream(string vhost, string name, FederationUpstream upstream)
        {
            var resource = "api/parameters/federation-upstream/{vhost}/{name}";

            //HACK: The federation api wants the payload to be in a value element
            var upstreamValue = new { value = upstream };

            Execute(resource, Method.PUT, upstreamValue, new Dictionary<string, string> { { "vhost", vhost }, { "name", name } });
        }

        #region Helper classes


        /// <summary>
        /// Using Json.net under the covers to address some limitations on the default serialization
        /// </summary>
        /// <remarks>
        /// Source: https://bytefish.de/blog/restsharp_custom_json_serializer/
        /// </remarks>
        /// <seealso cref="RestSharp.Serializers.ISerializer" />
        /// <seealso cref="RestSharp.Deserializers.IDeserializer" />
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
        /// <summary>
        /// Forces the canonical path and query on the http.
        /// </summary>
        /// <remarks>
        /// ZL - Needed because virtual host isn't used and "/" was not being encoded properly, need to literally pass %2f
        /// Source: https://stackoverflow.com/questions/781205/getting-a-url-with-an-url-encoded-slash
        /// </remarks>
        private class HttpWithCanonicalNameSupport : IHttp
        {
            private readonly IHttp _rawHttp = new Http();

            public HttpWebRequest DeleteAsync(Action<HttpResponse> action)
            {
                return _rawHttp.DeleteAsync(action);
            }

            public HttpWebRequest GetAsync(Action<HttpResponse> action)
            {
                return _rawHttp.GetAsync(action);
            }

            public HttpWebRequest HeadAsync(Action<HttpResponse> action)
            {
                return _rawHttp.HeadAsync(action);
            }

            public HttpWebRequest OptionsAsync(Action<HttpResponse> action)
            {
                return _rawHttp.OptionsAsync(action);
            }

            public HttpWebRequest PostAsync(Action<HttpResponse> action)
            {
                return _rawHttp.PostAsync(action);
            }

            public HttpWebRequest PutAsync(Action<HttpResponse> action)
            {
                return _rawHttp.PutAsync(action);
            }

            public HttpWebRequest PatchAsync(Action<HttpResponse> action)
            {
                return _rawHttp.PatchAsync(action);
            }

            public HttpWebRequest MergeAsync(Action<HttpResponse> action)
            {
                return _rawHttp.MergeAsync(action);
            }

            public HttpWebRequest AsPostAsync(Action<HttpResponse> action, string httpMethod)
            {
                return _rawHttp.AsPostAsync(action, httpMethod);
            }

            public HttpWebRequest AsGetAsync(Action<HttpResponse> action, string httpMethod)
            {
                return _rawHttp.AsGetAsync(action, httpMethod);
            }

            public HttpResponse Delete()
            {
                return _rawHttp.Delete();
            }

            public HttpResponse Get()
            {
                return _rawHttp.Get();
            }

            public HttpResponse Head()
            {
                return _rawHttp.Head();
            }

            public HttpResponse Options()
            {
                return _rawHttp.Options();
            }

            public HttpResponse Post()
            {
                return _rawHttp.Post();
            }

            public HttpResponse Put()
            {
                return _rawHttp.Put();
            }

            public HttpResponse Patch()
            {
                return _rawHttp.Patch();
            }

            public HttpResponse Merge()
            {
                return _rawHttp.Merge();
            }

            public HttpResponse AsPost(string httpMethod)
            {
                return _rawHttp.AsPost(httpMethod);
            }

            public HttpResponse AsGet(string httpMethod)
            {
                return _rawHttp.AsGet(httpMethod);
            }

            public Action<Stream> ResponseWriter
            {
                get => _rawHttp.ResponseWriter;
                set => _rawHttp.ResponseWriter = value;
            }

            public CookieContainer CookieContainer
            {
                get => _rawHttp.CookieContainer;
                set => _rawHttp.CookieContainer = value;
            }
            public ICredentials Credentials
            {
                get => _rawHttp.Credentials;
                set => _rawHttp.Credentials = value;
            }
            public bool AlwaysMultipartFormData
            {
                get => _rawHttp.AlwaysMultipartFormData;
                set => _rawHttp.AlwaysMultipartFormData = value;
            }
            public string UserAgent
            {
                get => _rawHttp.UserAgent;
                set => _rawHttp.UserAgent = value;
            }
            public int Timeout
            {
                get => _rawHttp.Timeout;
                set => _rawHttp.Timeout = value;
            }
            public int ReadWriteTimeout
            {
                get => _rawHttp.ReadWriteTimeout;
                set => _rawHttp.ReadWriteTimeout = value;
            }
            public bool FollowRedirects
            {
                get => _rawHttp.FollowRedirects;
                set => _rawHttp.FollowRedirects = value;
            }
            public X509CertificateCollection ClientCertificates
            {
                get => _rawHttp.ClientCertificates;
                set => _rawHttp.ClientCertificates = value;
            }
            public int? MaxRedirects
            {
                get => _rawHttp.MaxRedirects;
                set => _rawHttp.MaxRedirects = value;
            }
            public bool UseDefaultCredentials
            {
                get => _rawHttp.UseDefaultCredentials;
                set => _rawHttp.UseDefaultCredentials = value;
            }
            public Encoding Encoding
            {
                get => _rawHttp.Encoding;
                set => _rawHttp.Encoding = value;
            }
            public IList<HttpHeader> Headers => _rawHttp.Headers;

            public IList<HttpParameter> Parameters => _rawHttp.Parameters;

            public IList<HttpFile> Files => _rawHttp.Files;

            public IList<HttpCookie> Cookies => _rawHttp.Cookies;

            public string RequestBody
            {
                get => _rawHttp.RequestBody;
                set => _rawHttp.RequestBody = value;
            }
            public string RequestContentType
            {
                get => _rawHttp.RequestContentType;
                set => _rawHttp.RequestContentType = value;
            }
            public bool PreAuthenticate
            {
                get => _rawHttp.PreAuthenticate;
                set => _rawHttp.PreAuthenticate = value;
            }
            public RequestCachePolicy CachePolicy
            {
                get => _rawHttp.CachePolicy;
                set => _rawHttp.CachePolicy = value;
            }
            public byte[] RequestBodyBytes
            {
                get => _rawHttp.RequestBodyBytes;
                set => _rawHttp.RequestBodyBytes = value;
            }
            public Uri Url
            {
                get => _rawHttp.Url;
                set => _rawHttp.Url = value.WithCanonicalPathAndQuery();
            }
            public IWebProxy Proxy
            {
                get => _rawHttp.Proxy;
                set => _rawHttp.Proxy = value;
            }
        }

        #endregion
    }
}
