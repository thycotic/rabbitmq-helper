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
using Thycotic.Messages.Areas.Discovery.Request;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.Discovery
{
    [TestFixture]
    public class LocalAccountConsumerTests
    {
        [Test]
        public void ShouldHandleErrorReturnWhenScanningForLocalAccounts()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var scannerFactory = Substitute.For<IScannerFactory>();
            var discoveryScanner = Substitute.For<IDiscoveryScanner>();
            var consumer = new LocalAccountConsumer(responseBus, scannerFactory);

            scannerFactory.GetDiscoveryScanner(1).Returns(discoveryScanner);
            var scanInput = new ScanComputerInput {PageSize = 10};
            discoveryScanner.ScanComputerForLocalAccounts(scanInput).Returns(
                new LocalAccountScanResult {ErrorCode = 1, ErrorMessage = "Error"});

            var message = new ScanLocalAccountMessage();
            message.DiscoveryScannerId = 1;
            message.Input = scanInput;
            consumer.Consume(CancellationToken.None, message);

            responseBus.Received().Execute(Arg.Any<ScanLocalAccountResponse>());
        }

        [Test]
        public void ShouldNotPostMessageToServerWhenExceptionThrownByScanner()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var scannerFactory = Substitute.For<IScannerFactory>();
            var discoveryScanner = Substitute.For<IDiscoveryScanner>();
            var consumer = new LocalAccountConsumer(responseBus, scannerFactory);

            scannerFactory.GetDiscoveryScanner(1).Returns(discoveryScanner);
            var scanInput = new ScanComputerInput { PageSize = 10 };
            discoveryScanner.ScanComputerForLocalAccounts(scanInput).Returns(
                x => { throw new Exception(); });

            var message = new ScanLocalAccountMessage();
            message.DiscoveryScannerId = 1;
            message.Input = scanInput;
            consumer.Consume(CancellationToken.None, message);

            responseBus.DidNotReceive().Execute(Arg.Any<ScanMachineResponse>());
        }
    }
}
