using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Org.BouncyCastle.Ocsp;
using Thycotic.ActiveDirectorySynchronization;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Areas.ActiveDirectorySynchronization;
using Thycotic.Messages.Common;
using Thycotic.SharedTypes.General;
using ActiveDirectorySynchronizationDomainInfo = Thycotic.ActiveDirectorySynchronization.ActiveDirectorySynchronizationDomainInfo;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectorySynchronization
{
    /// <summary>
    /// Machine Consumer
    /// </summary>
    public class SearchForGroupInActiveDirectoryConsumer : IBasicConsumer<SearchForGroupInActiveDirectoryMessage>
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
        public void Consume(SearchForGroupInActiveDirectoryMessage request)
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

                var result = new ActiveDirectorySynchronizer().SearchForGroupInActiveDirectory(input);

                var batchId = Guid.NewGuid();
                var paging = new Paging
                {
                    Total = result.ADObjects.Count,
                    Take = request.BatchSize
                };

                Enumerable.Range(0, paging.BatchCount).ToList().ForEach(x =>
                {
                    var response = new EngineToServerCommunication.Areas.ActiveDirectorySynchronization.Response.SearchForGroupInActiveDirectoryResponse()
                    {
                        //This .Cast might not work
                        ADObjects = result.ADObjects.Cast<EngineToServerCommunication.Areas.ActiveDirectorySynchronization.ADObject>().Skip(paging.Skip).Take(paging.Take).ToList(),
                        Error = result.Error
                    };
                    //TODO - Use batch id?
                    _log.Info(string.Format("{0} : Send Domain Search Results Batch {1} of {2}", string.Join(", ", request.DomainInfo.DomainName), x + 1, paging.BatchCount));
                    _responseBus.Execute(response);
                    paging.Skip = paging.NextSkip;                        
                });
            }
            catch (Exception e)
            {
                _log.Error("Scan Domains Failed: ",  e);
            }
        }
    }
}
