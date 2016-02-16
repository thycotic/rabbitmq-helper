using System;
using System.Threading;
using NSubstitute;
using NUnit.Framework;
using Thycotic.Discovery.Core.Inputs;
using Thycotic.Discovery.Core.Results;
using Thycotic.Discovery.Sources.Scanners;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Discovery.Response;
using Thycotic.DistributedEngine.Logic.Areas.Discovery;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Messages.DE.Areas.Discovery.Request;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.Discovery
{
    [TestFixture]
    public class HostRangeConsumerTests
    {
        [Test]
        public void ShouldHandleErrorReturnWhenScanningHostRanges()
        {

            var responseBus = Substitute.For<IResponseBus>();
            var scannerFactory = Substitute.For<IScannerFactory>();
            var discoveryScanner = Substitute.For<IDiscoveryScanner>();
            var consumer = new HostRangeConsumer(responseBus, scannerFactory);

            scannerFactory.GetDiscoveryScanner(1).Returns(discoveryScanner);
            var scanInput = new ScanHostRangeInput {PageSize = 10};
            discoveryScanner.ScanForHostRanges(scanInput).Returns(
                new ScanHostRangeResult {ErrorCode = 1, ErrorMessage = "Error"});

            var message = new ScanHostRangeMessage();
            message.DiscoveryScannerId = 1;
            message.Input = scanInput;
            consumer.Consume(CancellationToken.None, message);

            responseBus.Received().Execute(Arg.Any<ScanHostRangeResponse>());
        }

        [Test]
        public void ShouldNotPostMessageToServerWhenExceptionThrownByScanner()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var scannerFactory = Substitute.For<IScannerFactory>();
            var discoveryScanner = Substitute.For<IDiscoveryScanner>();
            var consumer = new HostRangeConsumer(responseBus, scannerFactory);

            scannerFactory.GetDiscoveryScanner(1).Returns(discoveryScanner);
            var scanInput = new ScanHostRangeInput { PageSize = 10 };
            discoveryScanner.ScanForHostRanges(scanInput).Returns(
                x => { throw new Exception(); });

            var message = new ScanHostRangeMessage();
            message.DiscoveryScannerId = 1;
            message.Input = scanInput;
            consumer.Consume(CancellationToken.None, message);

            responseBus.DidNotReceive().Execute(Arg.Any<ScanHostRangeResponse>());
        }
    }
}
