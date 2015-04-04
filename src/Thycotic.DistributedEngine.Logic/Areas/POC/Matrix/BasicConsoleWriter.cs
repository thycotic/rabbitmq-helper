using System;

namespace Thycotic.DistributedEngine.Logic.Areas.POC.Matrix
{
    /// <summary>
    /// Matrix console writer
    /// </summary>
    public abstract class BasicConsoleWriter : IConsoleWriter
    {
        private static readonly object SyncRoot = new object();
        private int _matrixLeft = -1;
        private int _matrixTop = -1;

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="top">The top.</param>
        /// <param name="left">The left.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        protected virtual void Write(char value, int top, int left, ConsoleColor foregroundColor = ConsoleColor.DarkGreen)
        {
            lock (SyncRoot)
            {

                var currentLeft = Console.CursorLeft;
                var currentTop = Console.CursorTop;
                Console.CursorVisible = false; //Hide cursor
                Console.SetCursorPosition(left, top);
                ConsumerConsole.Write(value, foregroundColor);
                Console.SetCursorPosition(currentLeft, currentTop);
                Console.CursorVisible = true; //Show cursor back
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteMatrix(char value)
        {
            int left, top;

            if ((Console.LargestWindowHeight == 0) || (Console.LargestWindowWidth == 0))
            {
                return;
            }

            lock (SyncRoot)
            {
                const int margin = 5;

                var random = new Random(Guid.NewGuid().GetHashCode());

                if (_matrixLeft == -1)
                {
                    _matrixLeft = random.Next(Console.LargestWindowWidth - margin);
                    _matrixTop = random.Next(Console.LargestWindowHeight - margin);
                }

                if (_matrixTop >= Console.LargestWindowHeight - margin)
                {
                    _matrixLeft = random.Next(Console.LargestWindowWidth - margin);
                    _matrixTop = 0;
                }
                else
                {
                    _matrixTop++;
                }

                left = _matrixLeft;
                top = _matrixTop;

            }

            Write(value, top, left);
        }
    }
}