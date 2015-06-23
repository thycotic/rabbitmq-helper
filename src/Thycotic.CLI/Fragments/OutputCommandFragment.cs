using System;

namespace Thycotic.CLI.Fragments
{
    public class OutputCommandFragment : ICommandFragment
    {
        public Func<ConsoleCommandParameters, int> Action { get; set; }

        public string Name { get; set; }
        public string Output { get; set; }
        public ICommandFragment WhenTrue { get; set; }
        public ICommandFragment WhenFalse { get; set; }

        public OutputCommandFragment()
        {
            Action = parameters =>
            {
                Console.WriteLine(Output);

                return 0;
            };
        }
    }
}
