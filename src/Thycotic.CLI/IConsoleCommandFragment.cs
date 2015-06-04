using System;

namespace Thycotic.CLI
{
    public interface IConsoleCommandFragment
    {
        string Name { get; }

        Func<ConsoleCommandParameters, int> Action { get;}
    }
}