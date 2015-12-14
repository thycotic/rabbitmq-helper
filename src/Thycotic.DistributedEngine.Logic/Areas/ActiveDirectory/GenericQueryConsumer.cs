using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Thycotic.ActiveDirectory;
using Thycotic.ActiveDirectory.Core;
using Thycotic.Logging;
using Thycotic.Messages.Areas.ActiveDirectory;
using Thycotic.Messages.Common;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectory
{
    /// <summary>
    /// Consumer for searching AD objects.
    /// </summary>
    public class GenericQueryConsumer :
        IBlockingConsumer<GroupsByDomainQueryMessage, ADObjectsQueryResult>,
        IBlockingConsumer<UsersByGroupsQueryMessage, ADObjectsQueryResult>
    {
        /* 
         * JA - We need to restrict the amount of data returned to prevent issues with WCF message sizes.
         * Ideally, the block calls should ask for specific pages of data. Unfortunately, at this time, the
         * underlying logic for querying AD is very inefficient (queries everything, rather than just a single
         * page), making paged calls to the engine even more inefficient.
         */
        private const int WCF_MAX_RESULT_SIZE = 300;

        private readonly ILogWriter _log = Log.Get(typeof(GenericQueryConsumer));

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public ADObjectsQueryResult Consume(CancellationToken token, GroupsByDomainQueryMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Searching for groups.", string.Join(", ", request.DomainInfo.DomainName)));

                var input = new SearchActiveDirectoryInput
                {
                    BatchSize = request.BatchSize,
                    Domain = request.DomainInfo.ToCoreDomainInfo(),
                    Filter = request.Filter,
                    NamesToExclude = request.NamesToExclude
                };

                var results = new ActiveDirectorySearcher().SearchForGroupADObjectsInActiveDirectory(input);

                var totalGroups = results.ADObjects.Count;
                _log.Info(String.Format("{0} groups found.", totalGroups));

                if (totalGroups > WCF_MAX_RESULT_SIZE)
                {
                    _log.Info(String.Format("There more than {0} groups. Results will be truncated.", WCF_MAX_RESULT_SIZE));
                    return new ADObjectsQueryResult
                    {
                        ADObjects = results.ADObjects.OrderBy(o => o.Name).Take(WCF_MAX_RESULT_SIZE).ToList(),
                        Error = results.Error
                    };
                }

                return results;
            }
            catch (Exception e)
            {
                const string error = "Search for groups failed";
                _log.Error(error + ": ", e);
                return new ADObjectsQueryResult(new List<ADObject>(), error);
            }
        }

        /// <summary>
        /// Consumes the specified request.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public ADObjectsQueryResult Consume(CancellationToken token, UsersByGroupsQueryMessage request)
        {
            try
            {
                _log.Info(string.Format("{0} : Searching for users in specific groups.", string.Join(", ", request.DomainInfo.DomainName)));

                var input = new SearchActiveDirectoryInput
                {
                    BatchSize = request.BatchSize,
                    Domain = request.DomainInfo.ToCoreDomainInfo(),
                    Filter = request.Filter,
                    NamesToExclude = request.NamesToExclude
                };

                var results = new ActiveDirectorySearcher().GetUserADObjectsForGroupInActiveDirectory(input);

                var totalGroups = results.ADObjects.Count;
                _log.Info(String.Format("{0} users found.", totalGroups));

                if (totalGroups > WCF_MAX_RESULT_SIZE)
                {
                    _log.Info(String.Format("There more than {0} users. Results will be truncated.", WCF_MAX_RESULT_SIZE));
                    return new ADObjectsQueryResult
                    {
                        ADObjects = results.ADObjects.OrderBy(o => o.Name).Take(WCF_MAX_RESULT_SIZE).ToList(),
                        Error = results.Error
                    };
                }

                return results;
            }
            catch (Exception e)
            {
                const string error = "Search for users in groups failed";
                _log.Error(error + ": ", e);
                return new ADObjectsQueryResult(new List<ADObject>(), error);
            }
        }
    }
}
