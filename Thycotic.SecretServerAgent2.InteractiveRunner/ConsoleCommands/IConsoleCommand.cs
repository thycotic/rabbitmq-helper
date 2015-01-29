namespace Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands
{
    internal interface IConsoleCommand
    {
        string Name { get; }
        string Description { get; }
        void Execute(ConsoleCommandParameters parameters);
    }
}