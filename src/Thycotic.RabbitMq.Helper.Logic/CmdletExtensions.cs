using System.Linq;
using System.Management.Automation;

namespace Thycotic.RabbitMq.Helper.Logic
{
    /// <summary>
    /// Commandlet extensions.
    /// </summary>
    public static class CmdletExtensions
    {
        /// <summary>
        /// Returns the current command as a child of the specified command.
        /// Use this when establishing chains of commandlets.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="child">The child.</param>
        /// <returns></returns>
        public static Cmdlet AsChildOf(this Cmdlet child, Cmdlet parent)
        {
          
            child.CommandRuntime = parent.CommandRuntime;

            return child;
        }

        /// <summary>
        /// Invokes the command immediately.
        /// </summary>
        /// <param name="cmd">The command.</param>
        public static void InvokeImmediate(this Cmdlet cmd)
        {

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            cmd.Invoke<object>().ToArray();
        }

        /// <summary>
        /// Continues the command with the child one.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="child">The child.</param>
        /// <returns></returns>
        public static Cmdlet ContinueWith(this Cmdlet parent, Cmdlet child)
        {
            child.AsChildOf(parent).InvokeImmediate();
            return child;
        }
    }
}