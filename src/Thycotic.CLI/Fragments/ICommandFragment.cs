using System;

namespace Thycotic.CLI.Fragments
{
    public interface ICommandFragment
    {
        string Name { get; }

        Func<ConsoleCommandParameters, int> Action { get;}
    }
}