using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.ActiveDirectory;
using Thycotic.DistributedEngine.Logic.Areas.ActiveDirectory;
using Assert = NUnit.Framework.Assert;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.ActiveDirectory
{
    [TestFixture]
    public class GroupQueryInfoPagerTests
    {
        [Test]
        public void ShouldReturnNothingIfGroupsIsNull()
        {
            var pager = new GroupQueryInfoPager(null, 1).GetEnumerator();
            Assert.That(pager.MoveNext(), Is.False);
            Assert.That(pager.Current, Is.Null);
        } 

        [Test]
        public void ShouldReturnNothingIfGroupsAreEmpty()
        {
            var pager = new GroupQueryInfoPager(new List<GroupQueryInfo>(), 1).GetEnumerator();
            Assert.That(pager.MoveNext(), Is.False);
            Assert.That(pager.Current, Is.Null);
        }

        [Test]
        public void ShouldReturnEmptyGroups()
        {
            var groupQueryInfos = new List<GroupQueryInfo>
            {
                new GroupQueryInfo
                {
                    ADGuid = "Group1",
                    MemberUsers = new List<UserQueryInfo>()
                },
                new GroupQueryInfo
                {
                    ADGuid = "Group2",
                    MemberUsers = new List<UserQueryInfo>
                    {
                        new UserQueryInfo
                        {
                            ADGuid = "User1"
                        }
                    }
                },
            };

            var pager = new GroupQueryInfoPager(groupQueryInfos, 2).GetEnumerator();
            
            Assert.That(pager.MoveNext(), Is.True);
            Assert.That(pager.Current.Count(), Is.EqualTo(2));
            Assert.That(pager.MoveNext(), Is.False);
        }

        [Test]
        public void ShouldReturnGroupInSeparatePagesIfUsersExceedPageSize()
        {
            var groupQueryInfos = new List<GroupQueryInfo>
            {
                new GroupQueryInfo
                {
                    ADGuid = "Group1",
                    MemberUsers = new List<UserQueryInfo>
                    {
                        new UserQueryInfo { ADGuid = "User1" },
                        new UserQueryInfo { ADGuid = "User2" },
                        new UserQueryInfo { ADGuid = "User3" },
                        new UserQueryInfo { ADGuid = "User4" }
                    }
                }
            };

            var pager = new GroupQueryInfoPager(groupQueryInfos, 3).GetEnumerator();

            Assert.That(pager.MoveNext(), Is.True);
            Assert.That(pager.Current.Count(), Is.EqualTo(1));
            Assert.That(pager.Current, Has.All.Matches<GroupQueryInfo>(g => g.ADGuid == "Group1"));
            Assert.That(pager.Current.First().MemberUsers, Has.Some.Matches<UserQueryInfo>(u => u.ADGuid == "User1"));
            Assert.That(pager.Current.First().MemberUsers, Has.Some.Matches<UserQueryInfo>(u => u.ADGuid == "User2"));
            Assert.That(pager.MoveNext(), Is.True);
            Assert.That(pager.Current.Count(), Is.EqualTo(1));
            Assert.That(pager.Current, Has.All.Matches<GroupQueryInfo>(g => g.ADGuid == "Group1"));
            Assert.That(pager.Current.First().MemberUsers, Has.Some.Matches<UserQueryInfo>(u => u.ADGuid == "User3"));
            Assert.That(pager.Current.First().MemberUsers, Has.Some.Matches<UserQueryInfo>(u => u.ADGuid == "User4"));
            Assert.That(pager.MoveNext(), Is.False);
        }

        [Test]
        public void ShouldReturnGroupsInSeparatePagesIfGroupsExceedPageSize()
        {
            var groupQueryInfos = new List<GroupQueryInfo>
            {
                new GroupQueryInfo { ADGuid = "Group1" },
                new GroupQueryInfo { ADGuid = "Group2" },
                new GroupQueryInfo { ADGuid = "Group3" },
                new GroupQueryInfo { ADGuid = "Group4" },
                new GroupQueryInfo { ADGuid = "Group5" }
            };

            var pager = new GroupQueryInfoPager(groupQueryInfos, 2).GetEnumerator();

            Assert.That(pager.MoveNext(), Is.True);
            Assert.That(pager.Current.Count(), Is.EqualTo(2));
            Assert.That(pager.Current, Has.Some.Matches<GroupQueryInfo>(g => g.ADGuid == "Group1"));
            Assert.That(pager.Current, Has.Some.Matches<GroupQueryInfo>(g => g.ADGuid == "Group2"));
            
            Assert.That(pager.MoveNext(), Is.True);
            Assert.That(pager.Current.Count(), Is.EqualTo(2));
            Assert.That(pager.Current, Has.Some.Matches<GroupQueryInfo>(g => g.ADGuid == "Group3"));
            Assert.That(pager.Current, Has.Some.Matches<GroupQueryInfo>(g => g.ADGuid == "Group4"));

            
            Assert.That(pager.MoveNext(), Is.True);
            Assert.That(pager.Current.Count(), Is.EqualTo(1));
            Assert.That(pager.Current, Has.Some.Matches<GroupQueryInfo>(g => g.ADGuid == "Group5"));

            Assert.That(pager.MoveNext(), Is.False);
        }
    }
}