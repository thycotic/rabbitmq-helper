using Thycotic.CLI.Fragments;

namespace Thycotic.CLI.Commands
{
    public interface ICommand : ICommandFragment
    {
        string Area { get; }
        string[] Aliases { get; set; }
        string Description { get; }
        
    }
}