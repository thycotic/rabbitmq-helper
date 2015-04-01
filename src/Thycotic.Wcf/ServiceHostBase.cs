using System;
using System.ComponentModel;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using Thycotic.Logging;

namespace Thycotic.Wcf
{
    /// <summary>
    /// Base WCF server wrapper
    /// </summary>
    public class ServiceHostBase<TServer, TEndPoint> : IDisposable
    {
        private readonly string _connectionString;
        private readonly UserNamePasswordValidator _userNamePasswordValidator;
        private readonly bool _useSsl;
        private readonly string _thumbprint;
        private ServiceHost _host;

        private readonly ILogWriter _log = Log.Get(typeof(ServiceHostBase));

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostBase{TServer,TEndPoint}"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="userNamePasswordValidator"></param>
        public ServiceHostBase(string connectionString, UserNamePasswordValidator userNamePasswordValidator = null)
        {
            _connectionString = connectionString;
            _userNamePasswordValidator = userNamePasswordValidator;

            _useSsl = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostBase{TServer,TEndPoint}" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <param name="userNamePasswordValidator">The user name password validator.</param>
        public ServiceHostBase(string connectionString, string thumbprint, UserNamePasswordValidator userNamePasswordValidator = null)
        {
            _connectionString = connectionString;
            _thumbprint = thumbprint;
            _userNamePasswordValidator = userNamePasswordValidator;

            _useSsl = true;
        }

        /// <summary>
        /// Applies the additional initialization.
        /// </summary>
        /// <param name="host">The host.</param>
        protected virtual void ApplyAdditionalInitialization(ServiceHost host)
        {
            //none by default
        }

        /// <summary>
        /// Perform once-off startup processing.
        /// </summary>
        /// <exception cref="System.ApplicationException">Service already running</exception>
        public virtual void Start()
        {

            NetTcpBinding serviceBinding;


            if (_useSsl)
            {
                _log.Info("Host will offer encrypted connection");
                serviceBinding =
                    new NetTcpBinding(_userNamePasswordValidator != null ? SecurityMode.TransportWithMessageCredential : SecurityMode.Transport);
            }
            else
            {
                _log.Warn("Host will not offer encrypted connection");
                serviceBinding = new NetTcpBinding(SecurityMode.None);
            }

            if (_userNamePasswordValidator != null)
            {
                serviceBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            }
            else
            {
                serviceBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
            }

            _host = new ServiceHost(typeof (TServer));

            _host.AddServiceEndpoint(typeof (TEndPoint), serviceBinding, _connectionString);

            if (_useSsl)
            {
                _host.Credentials.ServiceCertificate.SetCertificate(
                    StoreLocation.LocalMachine,
                    StoreName.My,
                    X509FindType.FindByThumbprint,
                    _thumbprint);

                if (_userNamePasswordValidator != null)
                {
                    _host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode =
                        UserNamePasswordValidationMode.Custom;
                    _host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator =
                        _userNamePasswordValidator;
                }
            }
            else
            {
                //message user/password doesn't allow non-ssl use                 
                _log.Warn("Host will not be able to validate client credentials. Use SSL for increased security");
            }
            try
            {
                ApplyAdditionalInitialization(_host);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to apply additional configuration", ex);
            }

            try
            {
                _host.Open();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Failed to open service host because {0}", ex));
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public virtual void Stop()
        {
            if (_host == null) return;
            
            switch (_host.State)
            {
                case CommunicationState.Opened:
                    _host.Close();
                    break;
                case CommunicationState.Faulted:
                    _host.Abort();
                    break;
            }
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