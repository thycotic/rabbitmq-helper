using System;

namespace Thycotic.CLI
{
    public class NoopConsoleCommandFragement : IConsoleCommandFragment
    {
        public string Name { get; set; }
        public Func<ConsoleCommandParameters, int> Action { get; set; }

        public NoopConsoleCommandFragement()
        {
            Action = parameters => 0;
        }
    }
}