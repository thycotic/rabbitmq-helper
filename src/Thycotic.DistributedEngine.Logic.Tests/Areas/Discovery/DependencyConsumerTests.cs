using System;
using NSubstitute;
using NUnit.Framework;
using Thycotic.Discovery.Core.Inputs;
using Thycotic.Discovery.Core.Results;
using Thycotic.Discovery.Sources.Scanners;
using Thycotic.DistributedEngine.EngineToServerCommunication.Areas.Discovery.Response;
using Thycotic.DistributedEngine.Logic.Areas.Discovery;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Messages.Areas.Discovery.Request;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.Discovery
{
    [TestFixture]
    public class DependencyConsumerTests
    {
        [Test]
        public void ShouldHandleErrorReturnWhenScanningForDependencies()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var scannerFactory = Substitute.For<IScannerFactory>();
            var discoveryScanner = Substitute.For<IDiscoveryScanner>();
            var consumer = new DependencyConsumer(responseBus, scannerFactory);

            scannerFactory.GetDiscoveryScanner(1).Returns(discoveryScanner);
            var scanInput = new ScanComputerInput {PageSize = 10};
            discoveryScanner.ScanComputerForDependencies(scanInput).Returns(
                new DependencyScanResult {ErrorCode = 1, ErrorMessage = "Error"});

            var message = new ScanDependencyMessage();
            message.DiscoveryScannerId = 1;
            message.Input = scanInput;
            consumer.Consume(message);

            responseBus.Received().Execute(Arg.Any<ScanDependencyResponse>());
        }

        [Test]
        public void ShouldNotPostMessageToServerWhenExceptionThrownByScanner()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var scannerFactory = Substitute.For<IScannerFactory>();
            var discoveryScanner = Substitute.For<IDiscoveryScanner>();
            var consumer = new DependencyConsumer(responseBus, scannerFactory);

            scannerFactory.GetDiscoveryScanner(1).Returns(discoveryScanner);
            var scanInput = new ScanComputerInput { PageSize = 10 };
            discoveryScanner.ScanComputerForDependencies(scanInput).Returns(
                x => { throw new Exception(); });

            var message = new ScanDependencyMessage();
            message.DiscoveryScannerId = 1;
            message.Input = scanInput;
            consumer.Consume(message);

            responseBus.DidNotReceive().Execute(Arg.Any<ScanDependencyResponse>());
        }
    }
}
