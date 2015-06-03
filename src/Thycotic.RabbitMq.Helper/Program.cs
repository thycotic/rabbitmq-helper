using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            Log.Configure();

            var initialCommand = string.Join(" ", args);

            var cli = new CommandLineInterface();

            cli.AddCustomCommand(new InstallRabbitMqCommand());
            cli.AddCustomCommand(new ConvertPfxToPemCommand());

            cli.BeginInputLoop(initialCommand);

            return 0;
        }

    }
}
