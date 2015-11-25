using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Thycotic.ActiveDirectory;
using Thycotic.ActiveDirectory.Core;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.ActiveDirectory;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.ActiveDirectory.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Areas.ActiveDirectory;
using Thycotic.Messages.Common;
using DomainInfo = Thycotic.ActiveDirectory.Core.DomainInfo;
using GroupInfo = Thycotic.ActiveDirectory.Core.GroupInfo;
using QueryLogEntry = Thycotic.DistributedEngine.EngineToServerCommunication.Areas.ActiveDirectory.QueryLogEntry;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectory
{
    /// <summary>
    /// ADSync Consumer
    /// </summary>
    public class ADSyncRequestConsumer : IBasicConsumer<ADSyncMessage>
    {
        private const int DEFAULT_PAGE_SIZE = 100;
        private readonly IResponseBus _responseBus;
        private readonly IActiveDirectorySearcher _activeDirectorySearcher;
        private readonly ILogWriter _log = Log.Get(typeof(ADSyncRequestConsumer));

        /// <summary>
        /// ADSync Consumer
        /// </summary>
        /// <param name="responseBus"></param>
        /// <param name="activeDirectorySearcher"></param>
        public ADSyncRequestConsumer(IResponseBus responseBus, IActiveDirectorySearcher activeDirectorySearcher)
        {
            Contract.Requires<ArgumentNullException>(responseBus != null);
            Contract.Requires<ArgumentNullException>(activeDirectorySearcher != null);

            _responseBus = responseBus;
            _activeDirectorySearcher = activeDirectorySearcher;
        }

        /// <summary>
        /// Scan AD
        /// </summary>
        /// <param name="message"></param>
        public void Consume(ADSyncMessage message)
        {
            try
            {
                Guid batchGuid = Guid.NewGuid();
                _log.Info(string.Format("{0} : Starting AD groups/users scan for {1}.", batchGuid, string.Join(", ", message.Domains.Select(d => d.DomainName))));

                var adScanQueryInput = new QueryInput
                {
                    BatchSize = message.BatchSize,
                    Domains = Convert(message.Domains)
                };
                GroupsAndMembersQueryResult adScanResult = _activeDirectorySearcher.QueryGroupsAndMembers(adScanQueryInput);

                var synchronizationGroups = message.Domains.SelectMany(d => d.GroupInfos).ToList();

                var mappedResponse = MapADScanResultToEngineToServerType(adScanResult, synchronizationGroups);

                var pager = new GroupQueryInfoPager(mappedResponse, message.BatchSize).GetEnumerator();

                /*
                 * Send batches with Group/User info.
                 */
                int batchNumber = 0;
                while (pager.MoveNext())
                {
                    IEnumerable<GroupQueryInfo> currentBatchGroups = pager.Current.ToList();
                    if (!currentBatchGroups.Any())
                    {
                        continue;
                    }

                    batchNumber++;
                    _log.Info(string.Format("{0} : Sending AD groups/users scan results. Batch {1}", batchGuid, batchNumber));
                    var response = new ADSyncBatchResponse
                    {
                        BatchGuid = batchGuid,
                        BatchCount = null,
                        IsBatchComplete = false,
                        Groups = currentBatchGroups.ToList(),
                        Logs = new List<QueryLogEntry>()

                    };
                    _responseBus.Execute(response);
                }

                /*
                 * Send batches with logging info.
                 */
                var mappedLogData = adScanResult.Logs.Select(log => new QueryLogEntry
                {
                    DomainName = log.DomainName,
                    GroupName = log.GroupName,
                    UserName = log.UserName,
                    Errors = log.Errors
                }).ToList();

                foreach (var logBatch in mappedLogData.Batch(DEFAULT_PAGE_SIZE))
                {
                    batchNumber++;
                    _log.Info(string.Format("{0} : Sending AD groups/users scan logs. Batch {1}", batchGuid, batchNumber));

                    var response = new ADSyncBatchResponse
                    {
                        BatchGuid = batchGuid,
                        BatchCount = null,
                        IsBatchComplete = false,
                        Groups = new List<GroupQueryInfo>(),
                        Logs = logBatch.ToList()

                    };
                    _responseBus.Execute(response);
                }

                /*
                 * Send final batch.
                 */
                batchNumber++;
                _log.Info(string.Format("{0} : Sending AD groups/users scan final batch. Batch {1}", batchGuid, batchNumber));

                var lastResponse = new ADSyncBatchResponse
                {
                    BatchGuid = batchGuid,
                    BatchCount = batchNumber,
                    IsBatchComplete = true,
                    Groups = new List<GroupQueryInfo>(),
                    Logs = new List<QueryLogEntry>()

                };
                _responseBus.Execute(lastResponse);

                _log.Info(string.Format("{0} : Sending AD groups/users scan complete.", batchGuid));
            }
            catch (Exception e)
            {
                _log.Error("AD groups/users scan failed: ", e);
            }
        }

        private IList<GroupQueryInfo> MapADScanResultToEngineToServerType(GroupsAndMembersQueryResult result, List<Messages.Areas.ActiveDirectory.GroupInfo> synchronizationGroups)
        {
            var mappedGroups = new List<GroupQueryInfo>();
            
            List<GroupInfo> syncGroupsFound = result.GroupsFound;
            foreach (var group in syncGroupsFound)
            {
                var mappedGroup = new GroupQueryInfo
                {
                    ADGuid = group.ADGuid,
                    DistinguishedName = group.DistinguishedName,
                    DomainDistinguishedName = group.DomainDistinguishedName,
                    Name = group.Name,
                    DomainName = group.DomainName
                };
                mappedGroup.MemberUsers = group.MemberUsers.Select(u => new UserQueryInfo
                {
                    DistinguishedName = u.DistinguishedName,
                    ADGuid = u.ADGuid,
                    Email = u.Email,
                    Name = u.Name,
                    DomainName = u.DomainName,
                    OUPath = u.OUPath,
                    DisplayName = u.DisplayName,
                    Enabled = u.Active

                }).ToList();
                mappedGroups.Add(mappedGroup);
            }
            return mappedGroups;
        }

        private List<DomainInfo> Convert(List<Messages.Areas.ActiveDirectory.DomainInfo> infos)
        {
            var mappedInfos = new List<DomainInfo>();
            foreach (var requestInfo in infos)
            {
                var mappedInfo = new DomainInfo()
                {
                    DistinguishedName = requestInfo.DistinguishedName,
                    DomainName = requestInfo.DomainName,
                    LdapTimeoutInSeconds = requestInfo.LdapTimeoutInSeconds,
                    UseSecureLdap = requestInfo.UseSecureLdap,
                    Port = requestInfo.Port,
                    UserName = requestInfo.UserName,
                    Password = requestInfo.Password,
                    ProtocolVersion = requestInfo.ProtocolVersion
                };

                mappedInfo.GroupInfos = requestInfo.GroupInfos.Select(g => new GroupInfo()
                {
                    ADGuid = g.ADGuid,
                    DistinguishedName = g.DistinguishedName,
                    Name = g.Name,
                }).ToList();
                mappedInfos.Add(mappedInfo);

            }
            return mappedInfos;
        }
    }
}
