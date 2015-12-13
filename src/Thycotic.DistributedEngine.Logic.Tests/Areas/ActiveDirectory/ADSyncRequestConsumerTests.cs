using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using NSubstitute;
using NUnit.Framework;
using Thycotic.ActiveDirectory;
using Thycotic.ActiveDirectory.Core;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.ActiveDirectory.Response;
using Thycotic.DistributedEngine.Logic.Areas.ActiveDirectory;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Messages.Areas.ActiveDirectory;
using GroupInfo = Thycotic.ActiveDirectory.Core.GroupInfo;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.ActiveDirectory
{
    [TestFixture]
    public class ADSyncRequestConsumerTests
    {
        [Test]
        public void ShouldSendAtLeastThreeResponsesForScanAndLogsAndFinalBatch()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var adSearcher = Substitute.For<IActiveDirectorySearcher>();
            var errors = new QueryErrors();
            errors.Errors.Add("error");
            adSearcher.QueryGroupsAndMembers(Arg.Any<QueryInput>()).Returns(new GroupsAndMembersQueryResult
            {
                GroupsFound = new List<GroupInfo>
                {
                    new GroupInfo
                    {
                        ADGuid = "Group 1",
                        MemberUsers = new List<UserInfo>
                        {
                            new UserInfo { ADGuid = "User 1" }
                        }
                    }
                },
                Logs = new List<QueryErrors> { errors }
            });
            var adSyncRequestConsumer = new ADSyncRequestConsumer(responseBus, adSearcher);

            adSyncRequestConsumer.Consume(CancellationToken.None, new ADSyncMessage());

            responseBus.Received().Execute(Arg.Is<ADSyncBatchResponse>(r => r.IsBatchComplete == false && r.BatchCount == null));
            responseBus.Received().Execute(Arg.Is<ADSyncBatchResponse>(r => r.IsBatchComplete == false && r.BatchCount == null));
            responseBus.Received().Execute(Arg.Is<ADSyncBatchResponse>(r => r.IsBatchComplete && r.BatchCount == 3));
        } 
    }
}