namespace Thycotic.CLI.Commands
{
    public class SystemCommand : CommandBase, IImmediateCommand {
        
        public override string Name
        {
            get { return CustomName; }
        }

        public string CustomName { get; set; }
    }
}