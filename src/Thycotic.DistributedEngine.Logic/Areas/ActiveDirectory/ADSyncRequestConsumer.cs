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
using Thycotic.SharedTypes.General;
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
        private readonly IResponseBus _responseBus;
        private readonly ILogWriter _log = Log.Get(typeof(ADSyncRequestConsumer));

        /// <summary>
        /// ADSync Consumer
        /// </summary>
        /// <param name="responseBus"></param>
        public ADSyncRequestConsumer(IResponseBus responseBus)
        {
            Contract.Requires<ArgumentNullException>(responseBus != null);
            _responseBus = responseBus;
        }

        /// <summary>
        /// Scan AD
        /// </summary>
        /// <param name="message"></param>
        public void Consume(ADSyncMessage message)
        {
            try
            {
                _log.Info(string.Format("{0} : Syncing Groups", string.Join(", ", message.Domains.Select(d=>d.DomainName))));

                var input = new QueryInput()
                {
                    BatchSize = message.BatchSize
                    ,Domains = Convert(message.Domains)
                };

                var result = new ActiveDirectorySearcher().QueryGroupsAndMembers(input);

                var paging = new Paging
                {
                    Total = result.GroupsFound.Count,
                    Take = message.BatchSize
                };

                var mappedResponse = Convert(result);

                Enumerable.Range(0, paging.BatchCount).ToList().ForEach(x =>
                {
                    var response = new ADSyncResponse
                    {
                        Groups = mappedResponse.Groups.Skip(paging.Skip).Take(paging.Take).ToList(),
                        Logs = mappedResponse.Logs.Skip(paging.Skip).Take(paging.Take).ToList()
                    };
                    _log.Info(string.Format("{0} : Send Domain Scan Results Batch {1} of {2}", string.Join(", ", message.Domains.Select(d => d.DomainName)), x + 1, paging.BatchCount));
                    _responseBus.Execute(response);
                    paging.Skip = paging.NextSkip;                        
                });
            }
            catch (Exception e)
            {
                _log.Error("Scan Domains Failed: ",  e);
            }
        }

        private ADSyncResponse Convert(GroupsAndMembersQueryResult result)
        {
            var mappedGroups = new List<GroupQueryInfo>();
            foreach (var group in result.GroupsFound)
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
            var mappedLogData = result.Logs.Select(l => new QueryLogEntry
            {
                DomainName = l.DomainName,
                GroupName = l.GroupName,
                UserName = l.UserName,
                Errors = l.Errors
            }).ToList();

            return new ADSyncResponse(mappedGroups, mappedLogData);
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
