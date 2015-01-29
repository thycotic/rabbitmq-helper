using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands
{
    internal abstract class ConsoleCommandBase : IConsoleCommand
    {
        public virtual string Name { get; set; }
        public virtual string[] Aliases { get; set; }
        public virtual string Description { get; set; }
        public Action<ConsoleCommandParameters> Action { get; set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ConsoleCommandBase))
                return false;

            var consoleCommand = obj as ConsoleCommandBase;

            return Name.Equals(consoleCommand.Name);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Name);

            if (Aliases != null && Aliases.Any())
            {
                sb.Append(string.Format(" ({0})", string.Join(" ",Aliases)));
            }

            if (!string.IsNullOrWhiteSpace(Description))
            {
                sb.Append(string.Format(" - {0}", Description));
            }

            return sb.ToString();
        }
    }
}