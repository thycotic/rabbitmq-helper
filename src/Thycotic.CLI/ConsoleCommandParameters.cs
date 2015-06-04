using System.Collections.Generic;
using Thycotic.Logging;

namespace Thycotic.CLI
{
    public class ConsoleCommandParameters : Dictionary<string, object>
    {
        private readonly ILogWriter _log = Log.Get(typeof(CommandLineInterface));

        public bool TryGet(string name, out string value)
        {
            if (!ContainsKey(name))
            {
                _log.Debug(string.Format("Parameter {0} was not found", name));
                value = string.Empty;
                return false;
            }

            value = this[name].ToString();
            return true;
        }

        public bool TryGetBoolean(string name, out bool value)
        {
            string booleanString;
            value = false;

            return TryGet(name, out booleanString) && bool.TryParse(booleanString, out value);
        }
    }
}