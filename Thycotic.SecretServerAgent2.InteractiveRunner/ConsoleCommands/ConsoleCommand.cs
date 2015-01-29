namespace Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands
{
    internal class ConsoleCommand : IConsoleCommand
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

        public virtual void Execute(ConsoleCommandParameters parameters)
        {
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ConsoleCommand))
                return false;

            var consoleCommand = obj as ConsoleCommand;

            return Name.Equals(consoleCommand.Name);
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Description) ? Name : string.Format("{0} - {1}", Name, Description);
        }
    }
}