using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Thycotic.DistributedEngine.Logic.Areas.POC
{
    /// <summary>
    /// Matrix console writer
    /// </summary>
    public class MatrixConsoleWriter
    {
        private static readonly object SyncRoot = new object();
        private int _matrixLeft = -1;
        private int _matrixTop = -1;
        private Task _lastTask;

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        public void WriteMatrix(char value, ConsoleColor foregroundColor = ConsoleColor.DarkGreen)
        {
            int left, top;


            lock (SyncRoot)
            {
                const int margin = 10;

                var random = new Random(Guid.NewGuid().GetHashCode());

                if (_matrixLeft == -1)
                {
                    _matrixLeft = random.Next(Console.LargestWindowWidth - margin);
                    _matrixTop = 0;
                }

                if (_matrixTop > Console.LargestWindowHeight - margin - random.Next(Console.LargestWindowHeight / 10))
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

            if (_lastTask == null)
            {
                _lastTask = Task.Delay(500);
            }

            _lastTask.ContinueWith(tast => Task.Delay(500)).ContinueWith(task =>
            {
                lock(SyncRoot)
                lock (Console.Out)
                {

                    var currentLeft = Console.CursorLeft;
                    var currentTop = Console.CursorTop;
                    Console.CursorVisible = false; //Hide cursor
                    Console.SetCursorPosition(left, top);
                    ConsumerConsole.Write(value);
                    Console.SetCursorPosition(currentLeft, currentTop);
                    Console.CursorVisible = true; //Show cursor back
                }
            });

        }
    }
}