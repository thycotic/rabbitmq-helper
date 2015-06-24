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

            var cli = new CommandLineInterface("Thycotic RabbitMq Helper");

            cli.DiscoverCommands();

            cli.BeginInputLoop(initialCommand);

            return 0;
        }

       


    }
}
