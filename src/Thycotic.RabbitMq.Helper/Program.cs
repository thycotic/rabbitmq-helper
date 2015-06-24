using System.Linq;
using System.Reflection;
using System.Threading;
using Autofac;
using Thycotic.CLI;
using Thycotic.CLI.Commands;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Installation;

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
