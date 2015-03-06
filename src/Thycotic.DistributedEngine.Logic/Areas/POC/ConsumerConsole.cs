using System;

namespace Thycotic.DistributedEngine.Logic.Areas.POC
{
    /// <summary>
    /// Console used by consumers
    /// </summary>
    public static class ConsumerConsole
    {
        private static readonly object SyncRoot = new object();

        private static void WriteHelper(Action writeAction, ConsoleColor foregroundColor = ConsoleColor.DarkGreen)
        {
            //lock so that coloring doesn't get ruined
            lock (SyncRoot)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = foregroundColor;
                writeAction.Invoke();
                Console.ForegroundColor = oldColor;
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        public static void Write(string value, ConsoleColor foregroundColor = ConsoleColor.DarkGreen)
        {
            WriteHelper(() => Console.Write(value), foregroundColor);
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        public static void WriteLine(string value, ConsoleColor foregroundColor = ConsoleColor.DarkGreen)
        {
            WriteHelper(() => Console.WriteLine(value), foregroundColor);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        public static void Write(char value, ConsoleColor foregroundColor = ConsoleColor.DarkGreen)
        {
            WriteHelper(() => Console.Write(value), foregroundColor);
        }

        /// <summary>
        /// Writes the matrix.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        public static void WriteMatrix(char value, ConsoleColor foregroundColor = ConsoleColor.DarkGreen)
        {
            const int margin = 10;
            var random = new Random(Guid.NewGuid().GetHashCode());
            Console.SetCursorPosition(random.Next(Console.LargestWindowWidth-margin), random.Next(Console.LargestWindowHeight - margin));
            WriteHelper(() => Console.Write(value), foregroundColor);
        }
    }
}
