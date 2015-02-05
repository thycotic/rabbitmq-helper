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
    public class MemoryMqServer : IStartable, IDisposable
    {
        private readonly string _connectionString;
        private readonly string _thumbprint;
        private Task _serverTask;
        private ServiceHost _host;

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqServer));

        public MemoryMqServer(string connectionString, string thumbprint)
        {
            _connectionString = connectionString;
            _thumbprint = thumbprint;
        }

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
                _host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new EngineVerifier();
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

        public void Dispose()
        {
            Stop();
        }
    }

}