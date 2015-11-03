using System;
using System.DirectoryServices;
using Thycotic.Logging;
using Thycotic.Messages.Authenticate.Request;
using Thycotic.Messages.Authenticate.Response;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.Authentication
{
    /// <summary>
    /// Authenticate By AD Consumer
    /// </summary>
    [ConsumerPriority(Priority.Highest)]
    public class AuthenticateByAdConsumer : IBlockingConsumer<AuthenticateByAdMessage, AuthenticateByAdResponse>
    {
        private readonly ILogWriter _log = Log.Get(typeof(AuthenticateByAdConsumer));

        /// <summary>
        /// Consumes the request
        /// </summary>
        /// <param name="request">AuthenticateByAdMessage</param>
        /// <returns>AuthenticateByAdResponse</returns>
        public AuthenticateByAdResponse Consume(AuthenticateByAdMessage request)
        {
            _log.Info(string.Format("Got an Authenticate-By-AD request for {0}@{1}: ", request.Username, request.UserDomain));

            string domainandusername = request.Ldaps ? String.Concat(request.Username, "@", request.UserDomain) : String.Concat(request.UserDomain, "\\", request.Username);
            string domainConnectionString = request.Port != 389 ? request.DomainToAuthenticateTo + ":" + request.Port : request.DomainToAuthenticateTo;
            var authenticationType = request.Ldaps ? AuthenticationTypes.SecureSocketsLayer : AuthenticationTypes.Secure;
            DirectoryEntry user = new DirectoryEntry("LDAP://" + domainConnectionString, domainandusername, request.Password, authenticationType);
            try
            {
                user.RefreshCache(new[] { "objectClass" });
            }
            catch (Exception ex)
            {
                _log.Info(string.Format("Authenticate-By-AD for {0}@{1}: Failure", request.Username, request.UserDomain));
                return new AuthenticateByAdResponse {Success = false, ErrorMessage = ex.ToString()};
            }
            finally
            {
                user.Dispose();
            }
            _log.Info(string.Format("Authenticate-By-AD for {0}@{1}: Success", request.Username, request.UserDomain));
            return new AuthenticateByAdResponse {Success = true};
        }
    }
}
