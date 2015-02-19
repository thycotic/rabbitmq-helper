using System;

namespace Thycotic.DistributedEngine.Logic.Areas.POC
{
    static class ConsumerConsole
    {
        private static readonly object SyncRoot = new object();

        private static void WriteHelper(Action writeAction)
        {
            //lock so that coloring doesn't get ruined
            lock (SyncRoot)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                writeAction.Invoke();
                Console.ForegroundColor = oldColor;
            }
        }

        public static void Write(string value)
        {
            WriteHelper(() => Console.Write(value));
        }

        public static void WriteLine(string value)
        {
            WriteHelper(() => Console.WriteLine(value));
        }

        public static void Write(char value)
        {
            WriteHelper(() => Console.Write(value));
        }
    }
}
