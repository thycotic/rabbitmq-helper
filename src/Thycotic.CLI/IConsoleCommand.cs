using System;

namespace Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands
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