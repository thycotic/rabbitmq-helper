using System;
using System.Collections.Generic;
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
using SearchForGroupInActiveDirectoryResponse = Thycotic.DistributedEngine.EngineToServerCommunication.Areas.ActiveDirectorySynchronization.Response.SearchForGroupInActiveDirectoryResponse;

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

                var result = new ActiveDirectorySynchronizer().SearchForGroupInActiveDirectory(input);

                var batchId = Guid.NewGuid();
                var paging = new Paging
                {
                    Total = result.ADObjects.Count,
                    Take = request.BatchSize
                };

                Enumerable.Range(0, paging.BatchCount).ToList().ForEach(x =>
                {
                    var response = new SearchForGroupInActiveDirectoryResponse()
                    {
                        ADObjects = ConvertADobjectsToADObjects(result.ADObjects).ToList(),
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
                const string error = "Scan Domains Failed";
                _log.Error(error + ": ", e);
                return new SearchForGroupInActiveDirectoryResponse(new List<EngineToServerCommunication.Areas.ActiveDirectorySynchronization.ADObject>(), error);
            }
            return null;
        }

        private IEnumerable<EngineToServerCommunication.Areas.ActiveDirectorySynchronization.ADObject> ConvertADobjectsToADObjects(IEnumerable<ADObject> adObjects)
        {
            return adObjects.Select(adObject => new EngineToServerCommunication.Areas.ActiveDirectorySynchronization.ADObject
            {
                ADGuid = adObject.ADGuid, Name = adObject.Name
            }).ToList();
        }
    }
}
