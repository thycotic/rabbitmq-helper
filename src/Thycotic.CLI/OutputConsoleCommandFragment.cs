using System;

namespace Thycotic.CLI
{
    public class OutputConsoleCommandFragment : IConsoleCommandFragment
    {
        public Func<ConsoleCommandParameters, int> Action { get; set; }

        public string Name { get; set; }
        public string Output { get; set; }
        public IConsoleCommandFragment WhenTrue { get; set; }
        public IConsoleCommandFragment WhenFalse { get; set; }

        public OutputConsoleCommandFragment()
        {
            Action = parameters =>
            {
                Console.WriteLine(Output);

                return 0;
            };
        }
    }
}
