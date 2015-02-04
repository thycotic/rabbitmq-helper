using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using Autofac;
using Thycotic.MemoryMq;

namespace Thycotic.SecretServerAgent2.MemoryMq
{
    public class MemoryMqServer : IStartable
    {
        private readonly string _connectionString;
        private readonly string _thumbprint;
        private Task _serverTask;
        private ServiceHost _host;

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
                _host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new AgentVerifier();
                _host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;

                _host.Credentials.ServiceCertificate.SetCertificate(
                    StoreLocation.LocalMachine,
                    StoreName.My,
                    X509FindType.FindByThumbprint,
                    _thumbprint);

                _host.Open();
            });
        }

        public void Stop()
        {
            _host.Close();
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