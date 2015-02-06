using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using Autofac;
using Thycotic.Logging;
using Thycotic.MemoryMq;

namespace Thycotic.SecretServerEngine2.MemoryMq
{
    /// <summary>
    /// Memory mq WCF server wrapper.
    /// </summary>
    public class MemoryMqServer : IStartable, IDisposable
    {
        private readonly string _connectionString;
        private readonly string _thumbprint;
        private Task _serverTask;
        private ServiceHost _host;

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqServer));

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqServer"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        public MemoryMqServer(string connectionString, string thumbprint)
        {
            _connectionString = connectionString;
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
                var serviceBinding = new NetTcpBinding(SecurityMode.TransportWithMessageCredential);
                serviceBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
                serviceBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

                _host = new ServiceHost(typeof(Thycotic.MemoryMq.MemoryMqServer));
                _host.AddServiceEndpoint(typeof(IMemoryMqServer), serviceBinding, _connectionString);
                _host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new EngineClientVerifier();
                _host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;

                _host.Credentials.ServiceCertificate.SetCertificate(
                    StoreLocation.LocalMachine,
                    StoreName.My,
                    X509FindType.FindByThumbprint,
                    _thumbprint);

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
            _log.Info("Server stopping...");

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