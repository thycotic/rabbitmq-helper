using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using Thycotic.ActiveDirectory;
using Thycotic.ActiveDirectory.Core;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.ActiveDirectory.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Areas.ActiveDirectory;
using Thycotic.Messages.Common;
using DomainInfo = Thycotic.ActiveDirectory.Core.DomainInfo;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectory
{
    /// <summary>
    /// 
    /// </summary>
    public class AllUsersByDomainQueryConsumer : IBasicConsumer<AllUsersByDomainQueryMessage>
    {
        private const int WCF_MAX_RESULT_SIZE = 250;

        private readonly IResponseBus _responseBus;
        private readonly IActiveDirectorySearcher _activeDirectorySearcher;
        private readonly ILogWriter _log = Log.Get(typeof(AllUsersByDomainQueryConsumer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseBus"></param>
        /// <param name="activeDirectorySearcher"></param>
        public AllUsersByDomainQueryConsumer(IResponseBus responseBus, IActiveDirectorySearcher activeDirectorySearcher)
        {
            Contract.Requires<ArgumentNullException>(responseBus != null);
            Contract.Requires<ArgumentNullException>(activeDirectorySearcher != null);

            _responseBus = responseBus;
            _activeDirectorySearcher = activeDirectorySearcher;
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        public void Consume(CancellationToken token, AllUsersByDomainQueryMessage request)
        {
            ADObjectsQueryResult results = null;
            try
            {
                _log.Info(string.Format("{0} : Searching for all users.", string.Join(", ", request.DomainInfo.DomainName)));

                results = _activeDirectorySearcher.GetAllUserADObjectsInActiveDirectory(request.DomainInfo.ToCoreDomainInfo());
            }
            catch (Exception e)
            {
                RespondWithSearchFailed(request, e);
                return;
            }

            var totalUsers = results.ADObjects.Count;
            _log.Info(String.Format("{0} users found.", totalUsers));

            int pageCount = (totalUsers + WCF_MAX_RESULT_SIZE - 1) / WCF_MAX_RESULT_SIZE;

            for (int i = 1; i <= pageCount; i++)
            {
                var currentPage = results.ADObjects
                    .Skip((i-1)* WCF_MAX_RESULT_SIZE)
                    .Take(WCF_MAX_RESULT_SIZE)
                    .ToList();

                _log.Info(String.Format("{2} Sending batch {0} of {1}.", i, pageCount, request.BatchId.ToString("B")));

                _responseBus.Execute(new AllUsersByDomainQueryResponse
                {
                    BatchCount = pageCount,
                    BatchGuid = request.BatchId,
                    IsBatchComplete = true,
                    Results = new ADObjectsQueryResult(currentPage, results.Error)
                });
            }
        }

        private void RespondWithSearchFailed(AllUsersByDomainQueryMessage request, Exception e)
        {
            const string error = "Search for all users failed";
            _log.Error(error + ": ", e);

            _responseBus.Execute(new AllUsersByDomainQueryResponse
            {
                BatchCount = 1,
                BatchGuid = request.BatchId,
                IsBatchComplete = true,
                Results = new ADObjectsQueryResult(Enumerable.Empty<ADObject>(), error)
            });
        }
    }
}