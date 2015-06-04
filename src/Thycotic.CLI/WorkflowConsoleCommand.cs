using System;
using System.Collections.Generic;
using System.Linq;
using Thycotic.Logging;

namespace Thycotic.CLI
{
    public class WorkflowConsoleCommand : ConsoleCommandBase
    {
        public IEnumerable<IConsoleCommandFragment> Steps { get; set; }

        private readonly ILogWriter _log = Log.Get(typeof(WorkflowConsoleCommand));

        public WorkflowConsoleCommand()
        {
            Action = parameters =>
            {

                Steps.ToList().ForEach(s =>
                {
                    _log.Debug(string.Format("Executing {0}", s.Name ?? "Unnamed step"));

                    if (s.Action.Invoke(parameters) != 0)
                    {
                        throw new ApplicationException("Step failed");
                    }
                });

                return 0;

            };
        }
    }
}