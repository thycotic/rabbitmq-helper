using System;

namespace Thycotic.CLI.Fragments
{
    public class NoopCommandFragement : ICommandFragment
    {
        public string Name { get; set; }
        public Func<ConsoleCommandParameters, int> Action { get; set; }

        public NoopCommandFragement()
        {
            Action = parameters => 0;
        }
    }
}