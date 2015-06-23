using System;
using System.Collections.Generic;
using System.Linq;
using Thycotic.CLI.Fragments;
using Thycotic.Logging;

namespace Thycotic.CLI.Commands
{
    public class WorkflowCommand : CommandBase
    {
        public IEnumerable<ICommandFragment> Steps { get; set; }

        private readonly ILogWriter _log = Log.Get(typeof(WorkflowCommand));

        public WorkflowCommand()
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