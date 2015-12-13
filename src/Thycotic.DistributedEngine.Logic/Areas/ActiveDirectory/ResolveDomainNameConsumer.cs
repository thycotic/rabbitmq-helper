using System;
using System.Threading;
using Thycotic.ActiveDirectory;
using Thycotic.ActiveDirectory.Core;
using Thycotic.Logging;
using Thycotic.Messages.Areas.ActiveDirectory;
using Thycotic.Messages.Areas.ActiveDirectory.Response;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectory
{
    /// <summary>
    /// Consumer for handling requests to resolve various domain name formats.
    /// </summary>
    public class ResolveDomainNameConsumer :
        IBlockingConsumer<ResolveDomainDistinguishedNameMessage, DomainDistinguishedNameResponse>,
        IBlockingConsumer<ResolveFullyQualifiedDomainNameMessage, FullyQualifiedDomainNameResponse>
    {
        private readonly ILogWriter _log = Log.Get(typeof(ResolveDomainNameConsumer));

        /// <summary>
        /// Consumes a ResolveDomainDistinguishedNameMessage.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">A ResolveDomainDistinguishedNameMessage.</param>
        /// <returns>
        /// A DomainDistinguishedNameResponse.
        /// </returns>
        public DomainDistinguishedNameResponse Consume(CancellationToken token, ResolveDomainDistinguishedNameMessage request)
        {
            try
            {
                _log.Info(string.Format("Resolving distinguished name for domain: {0}", request.DomainName));
                var connectionSettings = new ConnectionSettings
                {
                    SecureSocketLayer = request.UseSSL
                };
                
                var distinguishedName = DistinguishedNameHelper.ResolveDomainDistinguishedName(request.DomainName, request.UserPrincipalName, request.UserPassword, connectionSettings);
                _log.Debug("Resolve domain to " + (distinguishedName ?? "(null)") );
                
                return new DomainDistinguishedNameResponse
                {
                    DomainDistinguishedName = distinguishedName
                };
            }
            catch (Exception e)
            {
                _log.Error("Domain DN resolution failed" + ": ", e);
                return new DomainDistinguishedNameResponse
                {
                    ErrorMessage = e.Message
                };
            }       
        }

        /// <summary>
        /// Consumes a ResolveFullyQualifiedDomainNameMessage.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">A ResolveFullyQualifiedDomainNameMessage.</param>
        /// <returns>
        /// A FullyQualifiedDomainNameResponse.
        /// </returns>
        public FullyQualifiedDomainNameResponse Consume(CancellationToken token, ResolveFullyQualifiedDomainNameMessage request)
        {
            try
            {
                _log.Info(string.Format("Resolving fully qualified name for domain: {0}", request.FriendlyDomainName));
                var connectionSettings = new ConnectionSettings
                {
                    SecureSocketLayer = request.UseSSL
                };

                var fqdn = DistinguishedNameHelper.ResolveFullyQualifiedDomainNameFromFriendlyName(request.FriendlyDomainName, request.UserPrincipalName, request.UserPassword, connectionSettings);
                _log.Debug("Resolve domain to " + (fqdn ?? "(null)"));

                return new FullyQualifiedDomainNameResponse
                {
                    FullyQualifiedDomainName = fqdn
                };
            }
            catch (Exception e)
            {
                _log.Error("FQDN resolution failed" + ": ", e);
                return new FullyQualifiedDomainNameResponse
                {
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
