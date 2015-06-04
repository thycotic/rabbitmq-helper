namespace Thycotic.CLI
{
    public interface IConsoleCommand : IConsoleCommandFragment
    {
        string Area { get; }
        string[] Aliases { get; set; }
        string Description { get; }
        
    }
}