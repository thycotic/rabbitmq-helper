using System.Linq;
using Thycotic.DistributedEngine.InteractiveRunner;
using Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands;

namespace Thycotic.DistributedEngine.CertificateHelper
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            var initialCommand = string.Join(" ", args);

            var cli = new CommandLineInterface();

            cli.AddCustomCommand(new ConvertPfxToPemCommand());

            cli.BeginInputLoop(initialCommand);

            return 0;
        }

    }
}
