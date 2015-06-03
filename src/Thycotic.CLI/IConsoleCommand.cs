using System;

namespace Thycotic.CLI
{
    public interface IConsoleCommand
    {
        string Name { get; }
        string Area { get; }
        string[] Aliases { get; set; }
        string Description { get; }
        Action<ConsoleCommandParameters> Action { get; set; }
    }
}