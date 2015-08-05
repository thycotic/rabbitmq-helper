using System;
using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            if (!Environment.Is64BitOperatingSystem || !Environment.Is64BitProcess)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("WARNING: You are running the helper in a 32-bit process/operating system.");
                Console.WriteLine("We recommend installing site connectors on a 64-bit operating systems.");
                Console.WriteLine();
                Console.WriteLine("Close this window now to abort or press any key to proceed anyway...");
                Console.ReadKey();
                Console.Clear();
                Console.ResetColor();
            }

            Log.Configure();

            var initialCommand = string.Join(" ", args);

            var cli = new CommandLineInterface("Thycotic RabbitMq Helper");

            cli.DiscoverCommands();

            cli.BeginInputLoop(initialCommand);

            return 0;
        }

       


    }
}
