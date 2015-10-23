using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Thycotic.ActiveDirectorySynchronization;
using Thycotic.ActiveDirectorySynchronization.Core;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Areas.ActiveDirectorySynchronization;
using Thycotic.Messages.Common;
using ActiveDirectorySynchronizationDomainInfo = Thycotic.ActiveDirectorySynchronization.Core.ActiveDirectorySynchronizationDomainInfo;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectorySynchronization
{
    /// <summary>
    /// Machine Consumer
    /// </summary>
    public class SearchForGroupADObjectsInActiveDirectoryConsumer : IBlockingConsumer<SearchActiveDirectoryMessage, SearchActiveDirectoryResponse>
    {
        private readonly ILogWriter _log = Log.Get(typeof(SearchForGroupADObjectsInActiveDirectoryConsumer));

        /// <summary>
        /// Search for Group AD Objects
        /// </summary>
        /// <param name="request"></param>
        public SearchActiveDirectoryResponse Consume(SearchActiveDirectoryMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Searching For Groups", string.Join(", ", request.DomainInfo.DomainName)));

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

                return new ActiveDirectorySynchronizer().SearchForGroupADObjectsInActiveDirectory(input);

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
