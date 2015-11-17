using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.ActiveDirectory;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectory
{
    /// <summary>
    /// Applies paging to a collection of GroupQueryInfo by users. GroupQueryInfo can appear in multiple pages, but the MemberUsers for the group will not.
    /// </summary>
    public class GroupQueryInfoPager : IEnumerable<IEnumerable<GroupQueryInfo>>
    {
        private readonly IEnumerable<GroupQueryInfo> _groups;
        private readonly int _pageSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="pageSize"></param>
        public GroupQueryInfoPager(IEnumerable<GroupQueryInfo> groups, int pageSize)
        {
            _groups = groups;
            _pageSize = pageSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IEnumerable<GroupQueryInfo>> GetEnumerator()
        {
            if (_groups == null || !_groups.Any())
            {
                yield break;
            }

            var currentPage = new List<GroupQueryInfo>();
            GroupQueryInfo currentGroup = null;
            int usersProcessed = 0;
            foreach (var group in _groups)
            {
                currentGroup = CopyWithoutMembers(group);
                currentPage.Add(currentGroup);

                foreach (var user in group.MemberUsers)
                {
                    if (currentGroup == null)
                    {
                        currentGroup = CopyWithoutMembers(group);
                        currentPage.Add(currentGroup);
                    }
                    currentGroup.MemberUsers.Add(user);
                    usersProcessed++;

                    if (usersProcessed >= _pageSize)
                    {
                        yield return currentPage;

                        currentPage = new List<GroupQueryInfo>();
                        currentGroup = null;
                        usersProcessed = 0;
                    }
                }
            }

            if (usersProcessed > 0)
            {
                yield return currentPage;
            }
        }

        private static GroupQueryInfo CopyWithoutMembers(GroupQueryInfo @group)
        {
            return new GroupQueryInfo
            {
                ADGuid = @group.ADGuid,
                DisplayName = @group.DisplayName,
                DistinguishedName = @group.DistinguishedName,
                DomainDistinguishedName = @group.DomainDistinguishedName,
                DomainName = @group.DomainName,
                Name = @group.Name,
                MemberUsers = new List<UserQueryInfo>(),
                MemberGroups = new List<GroupQueryInfo>()
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}