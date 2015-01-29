using System;

namespace Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands
{
    internal interface IConsoleCommand
    {
        string Name { get; }
        string[] Aliases { get; set; }
        string Description { get; }
        Action<ConsoleCommandParameters> Action { get; set; }
    }
}