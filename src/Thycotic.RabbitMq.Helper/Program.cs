using System;
using Thycotic.CLI;
using Thycotic.CLI.Legacy;
using Thycotic.RabbitMq.Helper.PSCommands.Installation;

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
            
            var initialCommand = string.Join(" ", args);

            if (string.IsNullOrWhiteSpace(initialCommand.Trim()))
            {
                initialCommand = null;
            }

            var isLegacyCli = !string.IsNullOrWhiteSpace(initialCommand) && initialCommand.StartsWith("installConnector");

#pragma warning disable 618
            //we are basically forever married to the old cli format due to possibility of legacy documentation lingering around -dkk
            var cli = isLegacyCli ? new CommandLineWithLegacyParameterParsing() : new CommandLineInterface();
#pragma warning restore 618

            cli.Modules = new[] {typeof (InstallConnectorCommand).Assembly.Location};

            if (isLegacyCli)
            {
                cli.ConsumeInput(initialCommand + @" -verbose=""true""");
            }
            else
            {
                cli.BeginInputLoop(initialCommand);
            }

            return 0;
        }
    }
}
