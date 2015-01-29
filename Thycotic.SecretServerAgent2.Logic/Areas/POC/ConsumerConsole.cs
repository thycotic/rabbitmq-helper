using System;

namespace Thycotic.SecretServerAgent2.Logic.Areas.POC
{
    static class ConsumerConsole
    {
        private static void WriteHelper(Action writeAction)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            writeAction.Invoke();
            Console.ForegroundColor = oldColor;
        }

        public static void Write(string value)
        {
            WriteHelper(() => Console.Write(value));
        }

        public static void WriteLine(string value)
        {
            WriteHelper(() => Console.WriteLine(value));
        }
    }
}
