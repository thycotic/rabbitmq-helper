using System;
using System.IdentityModel.Selectors;
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
        private bool _useSsl;
        private readonly string _thumbprint;
        private Task _serverTask;
        private ServiceHost _host;

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqServer));
        private readonly UserNamePasswordValidator _engineClientVerifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqServer"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="engineClientVerifier">The engine client verifier.</param>
        public MemoryMqServer(string connectionString, UserNamePasswordValidator engineClientVerifier)
        {
            _connectionString = connectionString;
            _useSsl = false;
            _engineClientVerifier = engineClientVerifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqServer" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <param name="engineClientVerifier">The engine client verifier.</param>
        public MemoryMqServer(string connectionString, string thumbprint, UserNamePasswordValidator engineClientVerifier)
        {
            _connectionString = connectionString;
            _useSsl = true;
            _thumbprint = thumbprint;
            _engineClientVerifier = engineClientVerifier;
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
                    serviceBinding = new NetTcpBinding(SecurityMode.TransportWithMessageCredential);
                    serviceBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
                }
                else
                {
                    serviceBinding = new NetTcpBinding(SecurityMode.Message);
                }
                serviceBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

                _host = new ServiceHost(typeof(Thycotic.MemoryMq.MemoryMqServer));
                _host.AddServiceEndpoint(typeof(IMemoryMqServer), serviceBinding, _connectionString);
                _host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = _engineClientVerifier;
                _host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;

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