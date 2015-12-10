using System;
using System.Collections.Generic;
using System.Linq;
using Thycotic.ActiveDirectory;
using Thycotic.ActiveDirectory.Core;
using Thycotic.Logging;
using Thycotic.Messages.Areas.ActiveDirectory;
using Thycotic.Messages.Common;
using DomainInfo = Thycotic.ActiveDirectory.Core.DomainInfo;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectory
{
    /// <summary>
    /// Consumer for searching AD objects.
    /// </summary>
    public class GenericQueryConsumer : 
        IBlockingConsumer<AllUsersByDomainQueryMessage, ADObjectsQueryResult>,
        IBlockingConsumer<GroupsByDomainQueryMessage, ADObjectsQueryResult>,
        IBlockingConsumer<UsersByGroupsQueryMessage, ADObjectsQueryResult>
    {
        private readonly ILogWriter _log = Log.Get(typeof(GenericQueryConsumer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ADObjectsQueryResult Consume(AllUsersByDomainQueryMessage request)
        {
            const int maxSize = 300;
            try
            {
                _log.Info(string.Format("{0} : Searching for all users.", string.Join(", ", request.DomainInfo.DomainName)));

                var results = new ActiveDirectorySearcher().GetAllUserADObjectsInActiveDirectory(ConvertRequestToActiveDirectoryDomainInfo(request));

                var totalUsers = results.ADObjects.Count;
                _log.Info(String.Format("{0} users found.", totalUsers));

                if (totalUsers > maxSize)
                {
                    _log.Info(String.Format("There more than {0} users. Results will be truncated.", maxSize));
                    return new ADObjectsQueryResult
                    {
                        ADObjects = results.ADObjects.OrderBy(o => o.Name).Take(maxSize).ToList(),
                        Error = results.Error
                    };
                }

                return results;
            }
            catch (Exception e)
            {
                const string error = "Search for all users failed";
                _log.Error(error + ": ", e);
                return new ADObjectsQueryResult(new List<ADObject>(), error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ADObjectsQueryResult Consume(GroupsByDomainQueryMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Searching for groups.", string.Join(", ", request.DomainInfo.DomainName)));

                var input = new SearchActiveDirectoryInput
                {
                    BatchSize = request.BatchSize,
                    Domain = ConvertRequestToActiveDirectoryDomainInfo(request),
                    Filter = request.Filter,
                    NamesToExclude = request.NamesToExclude
                };

                return new ActiveDirectorySearcher().SearchForGroupADObjectsInActiveDirectory(input);
            }
            catch (Exception e)
            {
                const string error = "Search for groups failed";
                _log.Error(error + ": ", e);
                return new ADObjectsQueryResult(new List<ADObject>(), error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ADObjectsQueryResult Consume(UsersByGroupsQueryMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Searching for users in specific groups.", string.Join(", ", request.DomainInfo.DomainName)));

                var input = new SearchActiveDirectoryInput
                {
                    BatchSize = request.BatchSize,
                    Domain = ConvertRequestToActiveDirectoryDomainInfo(request),
                    Filter = request.Filter,
                    NamesToExclude = request.NamesToExclude
                };

                return new ActiveDirectorySearcher().GetUserADObjectsForGroupInActiveDirectory(input);
            }
            catch (Exception e)
            {
                const string error = "Search for users in groups failed";
                _log.Error(error + ": ", e);
                return new ADObjectsQueryResult(new List<ADObject>(), error);
            }
        }

        private static DomainInfo ConvertRequestToActiveDirectoryDomainInfo(QueryMessage request)
        {
            return new DomainInfo
            {
                DistinguishedName = request.DomainInfo.DistinguishedName,
                DomainName = request.DomainInfo.DomainName,
                Password = request.DomainInfo.Password,
                ProtocolVersion = request.DomainInfo.ProtocolVersion,
                UserName = request.DomainInfo.UserName,
                Port = request.DomainInfo.Port,
                LdapTimeoutInSeconds = request.DomainInfo.LdapTimeoutInSeconds,
                UseSecureLdap = request.DomainInfo.UseSecureLdap
            };
        }
    }
}
