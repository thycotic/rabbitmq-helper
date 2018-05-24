using System;
using System.Collections.Concurrent;
using System.Management.Automation;
using System.Threading;

namespace Thycotic.RabbitMq.Helper.Logic.Workflow
{
    /// <summary>
    /// Simple workflow
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class CmdletWorkflow : IDisposable
    {
        private static int _globalActivityId = 1;

        private readonly Cmdlet _parent;
        private readonly string _activityName;
        private readonly int _activityId;

        private readonly ConcurrentQueue<Func<bool>> _steps = new ConcurrentQueue<Func<bool>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdletWorkflow"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="activityName">Name of the activity.</param>
        public CmdletWorkflow(Cmdlet parent, string activityName)
        {
            _activityId = Interlocked.Increment(ref _globalActivityId);

            _parent = parent;
            _activityName = activityName;
        }

        /// <summary>
        /// Thens the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public CmdletWorkflow Then(Action action)
        {
            _steps.Enqueue(() =>
            {
                action();
                return true;
            });
            return this;
        }

        /// <summary>
        /// Thens the specified cmdlet.
        /// </summary>
        /// <param name="cmdletFactory">The cmdlet factory.</param>
        /// <returns></returns>
        public CmdletWorkflow Then(Func<Cmdlet> cmdletFactory)
        {
            _steps.Enqueue(() =>
            {
                cmdletFactory().AsChildOf(_parent).InvokeImmediate();
                return true;
            });
            return this;
        }


        /// <summary>
        /// Forks the flow.
        /// </summary>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <param name="actionWhenTrue">The action when true.</param>
        /// <param name="actionWhenFalse">The action when false.</param>
        /// <returns></returns>
        public CmdletWorkflow ThenFork(bool condition, Action<CmdletWorkflow> actionWhenTrue, Action<CmdletWorkflow> actionWhenFalse)
        {
            if (condition)
            {
                actionWhenTrue(this);
            }
            else
            {
                actionWhenFalse(this);
            }
            return this;
        }


        /// <summary>
        /// Reports the progress.
        /// </summary>
        /// <param name="statusDescription">The status description.</param>
        /// <param name="percent">The percent.</param>
        /// <returns></returns>
        public CmdletWorkflow ReportProgress(string statusDescription, int percent)
        {
            _steps.Enqueue(() =>
            {
                _parent.WriteProgress(new ProgressRecord(_activityId, _activityName, statusDescription)
                {
                    PercentComplete = percent
                });
                return true;
            });
            return this;
        }

        /// <summary>
        /// Ifs the specified function.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        public CmdletWorkflow If(Func<bool> func)
        {
            _steps.Enqueue(func);
            return this;
        }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        public void Invoke()
        {
            while (_steps.TryDequeue(out var func))
            {
                var success = func();
                if (!success)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Dispose()
        {
            _parent.WriteProgress(new ProgressRecord(_activityId, _activityName, "Complete")
            {
                PercentComplete = 100,
                RecordType = ProgressRecordType.Completed
            });
        }

    }
}
