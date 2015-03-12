using System;
using System.Collections.Concurrent;
using System.Linq;
using Thycotic.DistributedEngine.Logic.Areas.POC.Matrix;

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
        /// Writes the line.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        public static void WriteLine(object value, ConsoleColor foregroundColor = ConsoleColor.DarkGreen)
        {
            WriteHelper(() => Console.WriteLine(value), foregroundColor);
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        public static void Write(object value, ConsoleColor foregroundColor = ConsoleColor.DarkGreen)
        {
            WriteHelper(() => Console.Write(value), foregroundColor);
        }

        #region Matrix
        private static readonly ConcurrentBag<IConsoleWriter> MainConsoleWriters = new ConcurrentBag<IConsoleWriter>();
        private static readonly ConcurrentBag<IConsoleWriter> NullConsoleWriters = new ConcurrentBag<IConsoleWriter>();
       
        /// <summary>
        /// Writes the matrix.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        public static void WriteMatrix(char value, ConsoleColor foregroundColor = ConsoleColor.DarkGreen)
        {

            lock (SyncRoot)
            {
                if (MainConsoleWriters.IsEmpty || NullConsoleWriters.IsEmpty)
                {
                    Enumerable.Range(0, 100).ToList().ForEach(i =>
                    {
                        MainConsoleWriters.Add(new TrailingConsoleWriter());
                        NullConsoleWriters.Add(new NullConsoleWriter());
                    });
                }
            }

            IConsoleWriter writer;
            MainConsoleWriters.TryPeek(out writer);
            writer.WriteMatrix(value);

            NullConsoleWriters.TryPeek(out writer);
            writer.WriteMatrix(value);

        }
        #endregion
    }
}
