using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Thycotic.ActiveDirectorySynchronization;
using Thycotic.ActiveDirectorySynchronization.Core;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Areas.ActiveDirectorySynchronization;
using Thycotic.Messages.Common;
using Thycotic.SharedTypes.General;
using ActiveDirectorySynchronizationDomainInfo = Thycotic.ActiveDirectorySynchronization.Core.ActiveDirectorySynchronizationDomainInfo;
using SearchForGroupInActiveDirectoryResponse = Thycotic.ActiveDirectorySynchronization.Core.SearchForGroupInActiveDirectoryResponse;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectorySynchronization
{
    /// <summary>
    /// Machine Consumer
    /// </summary>
    public class SearchForGroupInActiveDirectoryConsumer : IBlockingConsumer<SearchForGroupInActiveDirectoryMessage, SearchForGroupInActiveDirectoryResponse>
    {
        private readonly IResponseBus _responseBus;
        private readonly ILogWriter _log = Log.Get(typeof(SearchForGroupInActiveDirectoryConsumer));

        /// <summary>
        /// Machine Consumer
        /// </summary>
        /// <param name="responseBus"></param>
        public SearchForGroupInActiveDirectoryConsumer(IResponseBus responseBus)
        {
            Contract.Requires<ArgumentNullException>(responseBus != null);
            _responseBus = responseBus;
        }

        /// <summary>
        /// Scan Machines
        /// </summary>
        /// <param name="request"></param>
        public SearchForGroupInActiveDirectoryResponse Consume(SearchForGroupInActiveDirectoryMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Searching For Groups", string.Join(", ", request.DomainInfo.DomainName)));

                var input = new SearchForGroupInActiveDirectoryInput
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

                return new ActiveDirectorySynchronizer().SearchForGroupInActiveDirectory(input);

            }
            catch (Exception e)
            {
                const string error = "Scan Domains Failed";
                _log.Error(error + ": ", e);
                return
                    new SearchForGroupInActiveDirectoryResponse(
                        new List<ADObject>(), error);
            }
        }
    }
}
