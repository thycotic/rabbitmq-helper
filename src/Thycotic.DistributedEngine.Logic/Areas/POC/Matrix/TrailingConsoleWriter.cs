using System;
using System.Threading.Tasks;

namespace Thycotic.DistributedEngine.Logic.Areas.POC.Matrix
{
    /// <summary>
    /// Trailing console writer
    /// </summary>
    public class TrailingConsoleWriter : BasicConsoleWriter
    {
        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="top">The top.</param>
        /// <param name="left">The left.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        protected override void Write(char value, int top, int left, ConsoleColor foregroundColor = ConsoleColor.DarkGreen)
        {
            base.Write(value, top, left, ConsoleColor.Green);

            Task.Delay(500).ContinueWith(task => base.Write(value, top, left, foregroundColor));
        }
    }
}