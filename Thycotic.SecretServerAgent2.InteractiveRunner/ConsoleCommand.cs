namespace Thycotic.SecretServerAgent2.InteractiveRunner
{
    internal class ConsoleCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }

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