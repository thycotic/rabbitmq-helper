using System;
using System.Collections.Concurrent;
using Thycotic.CLI.Commands;
using Thycotic.Discovery.Core.Elements;
using Thycotic.Discovery.Core.Inputs;
using Thycotic.Discovery.Sources.Scanners;
using Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.DE.Areas.Discovery.Request;
using System.Linq;
using System.Collections.Generic;

namespace Thycotic.DistributedEngine.InteractiveRunner.Commands.Discovery
{
    class LocalAccountsCommand : CommandBase
    {
        public static ConcurrentDictionary<string, int> ScanHistory = new ConcurrentDictionary<string, int>(); 
        private readonly IRequestBus _bus;
        private static readonly ILogWriter _log = Log.Get(typeof(LocalAccountsCommand));

        public override string Area
        {
            get { return CommandAreas.Discovery; }
        }

        public override string Description
        {
            get { return "Local Accounts Performance Scanner"; }
        }

        public LocalAccountsCommand(IRequestBus bus, IExchangeNameProvider exchangeNameProvider)
        {
            _bus = bus;

            Action = parameters =>
            {
                _log.Info("Starting local accounts scan");
                PublishLocalAccountMessages();
                _log.Info("Publishing complete.");
                return 0;
            };
        }

        private ScanMachinesInput GetScanMachinesInput()
        {
            return new ScanMachinesInput
            {
                AuthenticationAccountInfos = new[]
                {
                    new AuthenticationAccount
                    {
                        Domain = "testparent.thycotic.com",
                        Username = "MrMittens",
                        Password = "Password1"
                    }
                },
                SecureSocketLayer = false,
                IgnoreClusterNodeObjects = false,
                Domain = "testparent.thycotic.com"
            };
        }

        public class ComputerScanModel
        {
            public int ScannerHelperResultId { get; set; }
            public ComputerItem[] Computers { get; set; }
            public List<LocalAccountScanModel> AcccountsPerComputer = new List<LocalAccountScanModel>();
        }

        public class LocalAccountScanModel
        {
            public string ComputerName { get; set; }
            public LocalAccount[] LocalAccounts { get; set; }
            public string ErrorMessage { get; set; }
        }

        public void PublishLocalAccountMessages()
        {
            var adScanner = new ActiveDirectoryDiscoveryScanner();
            var authenticationAccountInfos = new[]
            {
                new AuthenticationAccount
                {
                    Domain = "testparent.thycotic.com",
                    Username = "mrmittens",
                    Password = "Password1"
                }
            };

            var computers = adScanner.ScanForMachines(GetScanMachinesInput());
            var messages = (computers.Computers.Where(x => x.ComputerName != "LYNX").Select((x, i) => new ScanLocalAccountMessage
            {
                ComputerId = i,
                DiscoveryScannerId = 6,
                DiscoverySourceId = 1,
                ExpiresOn = DateTime.UtcNow + TimeSpan.FromDays(5),
                Input = new ScanComputerInput
                {
                    AuthenticationAccountInfos = authenticationAccountInfos,
                    SecureSocketLayer = false,
                    Domain = "testparent.thycotic.com",
                    ComputerName = x.ComputerName,
                    LocalAccountDiscoveryMethod = 1,
                    ShowDetailedErrorMessages = false,
                    DiscoveryThreadTimeOut = 60,
                    IncludeDisabled = false,
                    DependencySearchTimeout = 300,
                    MachineOperatingSystemVersion = x.ComputerVersion,
                    PageSize = 50
                }
            })).ToList();

            foreach (var message in messages)
            {
                _bus.BasicPublish("THYCOPAIR24.testparent.thycotic.com", message);
            }
        }

        public static void CheckCompare(string computerName, int count)
        {
            if (ScanHistory.ContainsKey(computerName))
            {
                if (ScanHistory[computerName] != count)
                {
                    _log.Error(string.Format("An account-scan mismatch was found on computer {0}", computerName));
                }
            }
            else
            {
                ScanHistory.TryAdd(computerName, count);
            }
        }
    }
}
