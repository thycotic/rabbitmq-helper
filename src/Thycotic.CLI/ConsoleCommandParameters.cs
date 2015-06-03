using System.Collections.Generic;
using Thycotic.Logging;

namespace Thycotic.CLI
{
    public class ConsoleCommandParameters : Dictionary<string, object>
    {
        private readonly ILogWriter _log = Log.Get(typeof(CommandLineInterface));

        public bool TryGet<T>(string name, out T value)
        {
            if (!ContainsKey(name))
            {
                _log.Error(string.Format("Parameter {0} was not found", name));
                value = default (T);
                return false;
            }

            value = (T)this[name];
            return true;
        }
    }
}