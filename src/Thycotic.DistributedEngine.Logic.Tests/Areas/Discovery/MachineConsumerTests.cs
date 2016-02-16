using System;
using System.Collections.Generic;
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
    public class MachineConsumerTests
    {
        [Test]
        public void ShouldHandleErrorReturnWhenScanningForMachines()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var scannerFactory = Substitute.For<IScannerFactory>();
            var discoveryScanner = Substitute.For<IDiscoveryScanner>();
            var consumer = new MachineConsumer(responseBus, scannerFactory);

            scannerFactory.GetDiscoveryScanner(1).Returns(discoveryScanner);
            var scanMachinesInput = new ScanMachinesInput {PageSize = 10};
            discoveryScanner.ScanForMachines(scanMachinesInput).Returns(
                new ScanMachineResult {ErrorCode = 1, ErrorMessage = "Error Message", Logs = new List<DiscoveryLog>()});

            var message = new ScanMachineMessage();
            message.DiscoveryScannerId = 1;
            message.Input = scanMachinesInput;
            consumer.Consume(CancellationToken.None, message);

            responseBus.Received().Execute(Arg.Any<ScanMachineResponse>());
        }

        [Test]
        public void ShouldNotPostMessageToServerWhenExceptionThrownByScanner()
        {
            var responseBus = Substitute.For<IResponseBus>();
            var scannerFactory = Substitute.For<IScannerFactory>();
            var discoveryScanner = Substitute.For<IDiscoveryScanner>();
            var consumer = new MachineConsumer(responseBus, scannerFactory);

            scannerFactory.GetDiscoveryScanner(1).Returns(discoveryScanner);
            var scanMachinesInput = new ScanMachinesInput {PageSize = 10};
            discoveryScanner.ScanForMachines(scanMachinesInput).Returns(x => { throw new Exception(); });

            var message = new ScanMachineMessage();
            message.DiscoveryScannerId = 1;
            message.Input = scanMachinesInput;
            consumer.Consume(CancellationToken.None, message);

            responseBus.DidNotReceive().Execute(Arg.Any<ScanMachineResponse>());
        }
    }
}
