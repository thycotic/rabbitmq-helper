using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Thycotic.ActiveDirectorySynchronization;
using Thycotic.ActiveDirectorySynchronization.Core;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.ActiveDirectory;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.ActiveDirectory.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.Messages.Areas.ActiveDirectorySynchronization;
using Thycotic.Messages.Common;
using Thycotic.SharedTypes.General;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectorySynchronization
{
    /// <summary>
    /// ADSync Consumer
    /// </summary>
    public class ActiveDirectorySynchronizationConsumer : IBasicConsumer<ActiveDirectorySynchronizationMessage>
    {
        private readonly IResponseBus _responseBus;
        private readonly ILogWriter _log = Log.Get(typeof(ActiveDirectorySynchronizationConsumer));

        /// <summary>
        /// ADSync Consumer
        /// </summary>
        /// <param name="responseBus"></param>
        public ActiveDirectorySynchronizationConsumer(IResponseBus responseBus)
        {
            Contract.Requires<ArgumentNullException>(responseBus != null);
            _responseBus = responseBus;
        }

        /// <summary>
        /// Scan AD
        /// </summary>
        /// <param name="request"></param>
        public void Consume(ActiveDirectorySynchronizationMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Syncing Groups", string.Join(", ", request.ActiveDirectoryDomainInfos.Select(d => d.DomainName))));

                var input = new ActiveDirectorySynchronizationInput()
                {
                    BatchSize = request.BatchSize
                    ,
                    ActiveDirectoryDomainInfos = MapMessageToADSyncLibrary(request.ActiveDirectoryDomainInfos)
                };

                var result = new ActiveDirectorySynchronizer().QueryGroupsAndMembers(input);
                var mappedResponse = MapADSyncResultToEngineToServerType(result);

                var paging = new Paging
                {
                    Total = mappedResponse.Keys.Count,
                    Take = request.BatchSize
                };

                Enumerable.Range(0, paging.BatchCount).ToList().ForEach(batchNumer =>
                {
                    var pagedUsers = mappedResponse.Skip(paging.Skip).Take(paging.Take).ToList();
                    var groups = pagedUsers.SelectMany(pu => pu.Value).ToList();
                    foreach (var group in groups)
                    {
                        var group2 = group;
                        group.MemberUsers = mappedResponse.Keys.Where(mr => mappedResponse[mr].Any(g => g.ADGuid == group2.ADGuid)).ToList();
                    }
                    var mappedLogData = result.Logs.Select(l => new QueryLogEntry
                    {
                        DomainName = l.DomainName,
                        GroupName = l.GroupName,
                        UserName = l.UserName,
                        Errors = l.Errors
                    }).ToList();

                    var response = new ADSyncResponse(groups, mappedLogData)
                    {
                        BatchNumber = batchNumer,
                        BatchCount = paging.BatchCount
                    };
                    _log.Info(string.Format("{0} : Send Domain Scan Results Batch {1} of {2}", string.Join(", ", request.ActiveDirectoryDomainInfos.Select(d => d.DomainName)), batchNumer + 1, paging.BatchCount));
                    _responseBus.Execute(response);
                    paging.Skip = paging.NextSkip;
                });
            }
            catch (Exception e)
            {
                _log.Error("Scan Domains Failed: ", e);
            }
        }

        private Dictionary<UserQueryInfo, List<GroupQueryInfo>> MapADSyncResultToEngineToServerType(ActiveDirectorySynchronizationResponse result)
        {
            var mappedGroups = new List<GroupQueryInfo>();
            foreach (var group in result.SyncedGroups)
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

            //Flip it into groups-by-user
            var allUsers = mappedGroups.SelectMany(x => x.MemberUsers);
            var groupsPerUserDictionary = new Dictionary<UserQueryInfo, List<GroupQueryInfo>>();
            foreach (var user in allUsers)
            {
                var groupsUserIsIn = mappedGroups.Where(g => g.MemberUsers.Any(u => u == user)).ToList();
                foreach (var group in groupsUserIsIn)
                {
                    group.MemberUsers = new List<UserQueryInfo>();
                }
                if (groupsPerUserDictionary.Keys.Any(key => key == user))
                {
                    groupsPerUserDictionary[user].AddRange(groupsUserIsIn);
                }
                else
                {
                    groupsPerUserDictionary.Add(user, groupsUserIsIn);
                }
            }

            return groupsPerUserDictionary;
        }

        private List<ActiveDirectorySynchronizationDomainInfo> MapMessageToADSyncLibrary(List<ActiveDirectorySynchronizationDomain> infos)
        {
            var mappedInfos = new List<ActiveDirectorySynchronizationDomainInfo>();
            foreach (var requestInfo in infos)
            {
                var mappedInfo = new ActiveDirectorySynchronizationDomainInfo()
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

                mappedInfo.GroupInfos = requestInfo.GroupInfos.Select(g => new ActiveDirectorySynchronizationGroupInfo()
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
