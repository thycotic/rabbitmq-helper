using System;

namespace Thycotic.DistributedEngine.Logic.Areas.POC.Matrix
{
    /// <summary>
    /// Null console writer
    /// </summary>
    public class NullConsoleWriter : BasicConsoleWriter
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
            base.Write(' ', top, left, foregroundColor);
        }
    }
}