using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Thycotic.Logging;

namespace Thycotic.CLI
{
    public class ConsoleCommandParameters
    {
        private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private readonly ILogWriter _log = Log.Get(typeof(CommandLineInterface));

        public string this[string key]
        {
            get
            {
                Contract.Requires(key != null); 
                return _dictionary[key];
            }
            set
            {
                Contract.Requires(key != null);

                _dictionary[key] = value;
            }
        }

        public IEnumerable<string> AllKeys {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

                return _dictionary.Keys;
            }
        }

        public bool TryGet(string name, out string value)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name), "Parameter name should not be null or empty");

            if (!_dictionary.ContainsKey(name))
            {
                _log.Debug(string.Format("Parameter {0} was not found", name));
                value = string.Empty;
                return false;
            }

            value = _dictionary[name];
            return true;
        }

        public bool TryGetBoolean(string name, out bool value)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(name));

            string booleanString;
            value = false;

            return TryGet(name, out booleanString) && bool.TryParse(booleanString, out value);
        }
    }
}