using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using PostSharp.Aspects;
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
            var manager = Substitute.ForPartsOf<EngineToServerConnectionManager>("http://good;",
                            false);
            var conn = Substitute.For<IEngineToServerConnection>();
            var channel = Substitute.For<IEngineToServerCommunicationWcfService>();
            var callback = Substitute.For<IEngineToServerCommunicationCallback>();

            conn.OpenChannel(Arg.Any<IEngineToServerCommunicationCallback>()).Returns(channel);
            manager.GetConnection(Arg.Is(0)).Returns(conn);

            channel.When(c => c.Ping()).Do(x => { });

            manager.OpenLiveChannel(callback);
            Assert.AreEqual("http://good", manager.CurrentConnectionString);            
        }

        [Test]
        public void ShouldFailToOpenInvalidSingleWebConnection()
        {
            var manager = Substitute.ForPartsOf<EngineToServerConnectionManager>("http://bad;",
                            false);
            var badConn = Substitute.For<IEngineToServerConnection>();
            var badChannel = Substitute.For<IEngineToServerCommunicationWcfService>();
            var callback = Substitute.For<IEngineToServerCommunicationCallback>();

            badConn.OpenChannel(Arg.Any<IEngineToServerCommunicationCallback>()).Returns(badChannel);
            manager.GetConnection(Arg.Is(0)).Returns(badConn);

            badChannel.When(c => c.Ping()).Do(x => { throw new Exception(); });

            Assert.AreEqual("http://bad", manager.CurrentConnectionString);
            Assert.Throws<EndpointNotFoundException>(() => manager.OpenLiveChannel(callback));
        }

        [Test]
        public void ShouldFailToOpenInvalidNetTcpConnection()
        {
            var manager = Substitute.ForPartsOf<EngineToServerConnectionManager>("net.tcp://bad;",
                false);
            var badConn = Substitute.For<IEngineToServerConnection>();
            var badChannel = Substitute.For<IEngineToServerCommunicationWcfService>();
            var callback = Substitute.For<IEngineToServerCommunicationCallback>();

            badConn.OpenChannel(Arg.Any<IEngineToServerCommunicationCallback>()).Returns(badChannel);
            manager.GetConnection(Arg.Is(0)).Returns(badConn);

            badChannel.When(c => c.Ping()).Do(x => { throw new Exception(); });

            Assert.AreEqual("net.tcp://bad", manager.CurrentConnectionString);
            Assert.Throws<EndpointNotFoundException>(() => manager.OpenLiveChannel(callback));
        }

        [Test]
        public void ShouldOpenCorrectConnection()
        {
            var manager = Substitute.ForPartsOf<EngineToServerConnectionManager>("http://bad;http://good",false);
            var badConn = Substitute.For<IEngineToServerConnection>();
            var goodConn = Substitute.For<IEngineToServerConnection>();
            
            var badChannel = Substitute.For<IEngineToServerCommunicationWcfService>();
            var goodChannel = Substitute.For<IEngineToServerCommunicationWcfService>();

            badConn.OpenChannel(Arg.Any<IEngineToServerCommunicationCallback>()).Returns(badChannel);
            goodConn.OpenChannel(Arg.Any<IEngineToServerCommunicationCallback>()).Returns(goodChannel);

            var callback = Substitute.For<IEngineToServerCommunicationCallback>();

            manager.GetConnection(Arg.Is(0)).Returns(badConn);
            manager.GetConnection(Arg.Is(1)).Returns(goodConn);

            int counter = 0;
            badChannel.When(c => c.Ping()).Do(x => { throw new Exception(); });
            goodChannel.When(c => c.Ping()).Do(x => counter++);

            Assert.AreEqual("http://bad", manager.CurrentConnectionString);
                        
            var service = manager.OpenLiveChannel(callback);                
            service.Dispose();
            Assert.AreEqual("http://good", manager.CurrentConnectionString);
        }

        [Test]
        public void ShouldOpenConnection_WebClient()
        {
         // TODO
        }
    }
}
