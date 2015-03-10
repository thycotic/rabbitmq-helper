using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using Autofac;
using Thycotic.Logging;
using Thycotic.MemoryMq;

namespace Thycotic.DistributedEngine.MemoryMq
{
    /// <summary>
    /// Memory mq WCF server wrapper.
    /// </summary>
    public class MemoryMqServerWrapper : IStartable, IDisposable
    {
        private readonly string _connectionString;
        private readonly bool _useSsl;
        private readonly string _thumbprint;
        private Task _serverTask;
        private ServiceHost _host;

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqServerWrapper));

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqServerWrapper"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MemoryMqServerWrapper(string connectionString)
        {
            _connectionString = connectionString;
            _useSsl = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqServerWrapper" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        public MemoryMqServerWrapper(string connectionString, string thumbprint)
        {
            _connectionString = connectionString;
            _useSsl = true;
            _thumbprint = thumbprint;
        }

        /// <summary>
        /// Perform once-off startup processing.
        /// </summary>
        /// <exception cref="System.ApplicationException">Server already running</exception>
        public void Start()
        {
            if (_serverTask != null)
            {
                throw new ApplicationException("Server already running");
            }

            _serverTask = Task.Factory.StartNew(() =>
            {
                NetTcpBinding serviceBinding;

                if (_useSsl)
                {
                    serviceBinding = new NetTcpBinding(SecurityMode.Transport);
                    serviceBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
                }
                else
                {
                    serviceBinding = new NetTcpBinding(SecurityMode.None);
                }
                serviceBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

                _host = new ServiceHost(typeof(MemoryMqWcfServer));
                _host.AddServiceEndpoint(typeof(IMemoryMqWcfServer), serviceBinding, _connectionString);

                if (_useSsl)
                {
                    _host.Credentials.ServiceCertificate.SetCertificate(
                        StoreLocation.LocalMachine,
                        StoreName.My,
                        X509FindType.FindByThumbprint,
                        _thumbprint);
                }
                
                try
                {
                    _host.Open();

                    _log.Info("Server running...");
                }
                catch (Exception ex)
                {
                    _log.Error(string.Format("Server could not start because {0}", ex.Message), ex);    
                }
            });
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            _log.Info("Server stopping. This might take a few seconds...");

            if ((_host == null) || (_serverTask == null)) return;

            if (_host.State == CommunicationState.Opened)
            {
                _host.Close();
            }
            _serverTask.Wait();
                
            _host = null;
            _serverTask = null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
    }

}