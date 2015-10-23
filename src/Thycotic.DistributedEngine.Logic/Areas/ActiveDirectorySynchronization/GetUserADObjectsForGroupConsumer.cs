using System;
using System.Collections.Generic;
using Thycotic.ActiveDirectorySynchronization;
using Thycotic.ActiveDirectorySynchronization.Core;
using Thycotic.Logging;
using Thycotic.Messages.Areas.ActiveDirectorySynchronization;
using Thycotic.Messages.Common;
using ActiveDirectorySynchronizationDomainInfo = Thycotic.ActiveDirectorySynchronization.Core.ActiveDirectorySynchronizationDomainInfo;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectorySynchronization
{
    /// <summary>
    /// Machine Consumer
    /// </summary>
    public class GetUserADObjectsForGroupConsumer : IBlockingConsumer<SearchActiveDirectoryMessage, SearchActiveDirectoryResponse>
    {
        private readonly ILogWriter _log = Log.Get(typeof(GetUserADObjectsForGroupConsumer));

        /// <summary>
        /// Get User AD Objects for Group
        /// </summary>
        /// <param name="request"></param>
        public SearchActiveDirectoryResponse Consume(SearchActiveDirectoryMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Searching For Users", string.Join(", ", request.DomainInfo.DomainName)));

                var input = new SearchActiveDirectoryInput
                {
                    BatchSize = request.BatchSize,
                    ActiveDirectoryDomainInfo = new ActiveDirectorySynchronizationDomainInfo
                    {
                        DistinguishedName = request.DomainInfo.DistinguishedName,
                        DomainName = request.DomainInfo.DomainName,
                        Password = request.DomainInfo.Password,
                        ProtocolVersion = request.DomainInfo.ProtocolVersion,
                        UserName = request.DomainInfo.UserName,
                        LdapTimeoutInSeconds = request.DomainInfo.LdapTimeoutInSeconds,
                        UseSecureLdap = request.DomainInfo.UseSecureLdap
                    },
                    Filter = request.Filter,
                    NamesToExclude = request.NamesToExclude
                };

                return new ActiveDirectorySynchronizer().GetUserADObjectsForGroupInActiveDirectory(input);
            }
            catch (Exception e)
            {
                const string error = "Scan Domains Failed";
                _log.Error(error + ": ", e);
                return
                    new SearchActiveDirectoryResponse(
                        new List<ADObject>(), error);
            }
        }
    }
}
