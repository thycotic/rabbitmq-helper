using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.Service.EngineToServer;

namespace Thycotic.DistributedEngine.Service.Tests
{
    [TestFixture]
    public class EngineToServerConnectionManagerTests
    {
        [Test]
        public void ShouldOpenValidSingleWebConnection()
        {
            var manager = new EngineToServerConnectionManager("http://localhost/ihawu", false);
            var callback = Substitute.For<IEngineToServerCommunicationCallback>();
            var service = manager.OpenLiveChannel(callback);
            service.PreAuthenticate();
        }

        [Test]
        public void ShouldFailToOpenInvalidSingleWebConnection()
        {
            try
            {
                var conn = new EngineToServerConnectionManager("http://invalid", false);
                var callback = Substitute.For<IEngineToServerCommunicationCallback>();
                var service = conn.OpenLiveChannel(callback);
                service.Dispose();
                Assert.Fail("Where is my exception?");
            }
            catch (EndpointNotFoundException)
            {                
            }
        }
    }
}
