using System;
using System.Linq;
using System.Text;

namespace Thycotic.SecretServerEngine2.InteractiveRunner.ConsoleCommands
{
    internal abstract class ConsoleCommandBase : IConsoleCommand
    {

        public virtual string Name { get; set; }
        public virtual string Area { get; set; }
        public virtual string[] Aliases { get; set; }
        public virtual string Description { get; set; }

        protected bool Equals(ConsoleCommandBase other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Area, other.Area) && Equals(Aliases, other.Aliases);
        }

        public static bool operator ==(ConsoleCommandBase left, ConsoleCommandBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ConsoleCommandBase left, ConsoleCommandBase right)
        {
            return !Equals(left, right);
        }

        public Action<ConsoleCommandParameters> Action { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Area != null ? Area.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Aliases != null ? Aliases.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ConsoleCommandBase) obj);
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