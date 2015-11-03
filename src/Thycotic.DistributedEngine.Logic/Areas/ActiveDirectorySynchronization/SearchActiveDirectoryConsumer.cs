using System;
using System.Collections.Generic;
using Thycotic.ActiveDirectory;
using Thycotic.ActiveDirectory.Core;
using Thycotic.Logging;
using Thycotic.Messages.Areas.ActiveDirectorySynchronization;
using Thycotic.Messages.Common;
using ActiveDirectorySynchronizationDomainInfo = Thycotic.ActiveDirectory.Core.ActiveDirectorySynchronizationDomainInfo;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectorySynchronization
{
    /// <summary>
    /// Consumer for searching AD objects.
    /// </summary>
    public class SearchActiveDirectoryConsumer : 
        IBlockingConsumer<SearchActiveDirectoryForAllUsersMessage, SearchActiveDirectoryResponse>,
        IBlockingConsumer<SearchActiveDirectoryForGroupsMessage, SearchActiveDirectoryResponse>,
        IBlockingConsumer<SearchActiveDirectoryForUsersByGroupsMessage, SearchActiveDirectoryResponse>
    {
        private readonly ILogWriter _log = Log.Get(typeof(SearchActiveDirectoryConsumer));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SearchActiveDirectoryResponse Consume(SearchActiveDirectoryForAllUsersMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Searching for all users.", string.Join(", ", request.DomainInfo.DomainName)));

                return new ActiveDirectorySynchronizer().GetAllUserADObjectsInActiveDirectory(ConvertRequestToActiveDirectoryDomainInfo(request));
            }
            catch (Exception e)
            {
                const string error = "Search for all users failed";
                _log.Error(error + ": ", e);
                return new SearchActiveDirectoryResponse(new List<ADObject>(), error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SearchActiveDirectoryResponse Consume(SearchActiveDirectoryForGroupsMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Searching for groups.", string.Join(", ", request.DomainInfo.DomainName)));

                var input = new SearchActiveDirectoryInput
                {
                    BatchSize = request.BatchSize,
                    ActiveDirectoryDomainInfo = ConvertRequestToActiveDirectoryDomainInfo(request),
                    Filter = request.Filter
                };

                return new ActiveDirectorySynchronizer().SearchForGroupADObjectsInActiveDirectory(input);
            }
            catch (Exception e)
            {
                const string error = "Search for groups failed";
                _log.Error(error + ": ", e);
                return new SearchActiveDirectoryResponse(new List<ADObject>(), error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SearchActiveDirectoryResponse Consume(SearchActiveDirectoryForUsersByGroupsMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Searching for users in specific groups.", string.Join(", ", request.DomainInfo.DomainName)));

                var input = new SearchActiveDirectoryInput
                {
                    BatchSize = request.BatchSize,
                    ActiveDirectoryDomainInfo = ConvertRequestToActiveDirectoryDomainInfo(request),
                    Filter = request.Filter,
                    NamesToExclude = request.NamesToExclude
                };

                return new ActiveDirectorySynchronizer().GetUserADObjectsForGroupInActiveDirectory(input);
            }
            catch (Exception e)
            {
                const string error = "Search for users in groups failed";
                _log.Error(error + ": ", e);
                return new SearchActiveDirectoryResponse(new List<ADObject>(), error);
            }
        }

        private static ActiveDirectorySynchronizationDomainInfo ConvertRequestToActiveDirectoryDomainInfo(SearchActiveDirectoryMessage request)
        {
            return new ActiveDirectorySynchronizationDomainInfo
            {
                DistinguishedName = request.DomainInfo.DistinguishedName,
                DomainName = request.DomainInfo.DomainName,
                Password = request.DomainInfo.Password,
                ProtocolVersion = request.DomainInfo.ProtocolVersion,
                UserName = request.DomainInfo.UserName,
                LdapTimeoutInSeconds = request.DomainInfo.LdapTimeoutInSeconds,
                UseSecureLdap = request.DomainInfo.UseSecureLdap
            };
        }
    }
}
