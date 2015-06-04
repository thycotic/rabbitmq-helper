using System;

namespace Thycotic.CLI
{
    public class BinaryConsoleCommandFragment : IConsoleCommandFragment
    {
        public Func<ConsoleCommandParameters, int> Action { get; set; }

        public string Name { get; set; }
        public string Prompt { get; set; }
        public IConsoleCommandFragment WhenTrue { get; set; }
        public IConsoleCommandFragment WhenFalse { get; set; }

        public BinaryConsoleCommandFragment()
        {
            WhenFalse = new NoopConsoleCommandFragement();
            WhenTrue = new NoopConsoleCommandFragement();

            Action = parameters =>
            {
                string silentString;
                bool silent;
                if (parameters.TryGet("silent", out silentString) &&
                    bool.TryParse(silentString, out silent))
                {
                    return WhenTrue.Action.Invoke(parameters);
                }


                Console.Write("{0} [Y/N] ", Prompt);

                var input = Console.ReadLine() ?? string.Empty;

                var choice = input.ToLower().Trim();
                switch (choice)
                {
                    case "t":
                    case "true":
                    case "y":
                    case "yes":
                    case "yeh":
                    case "yeah":
                        return WhenTrue.Action.Invoke(parameters);
                    default:
                        return WhenFalse.Action.Invoke(parameters);
                }

            };
        }


    }
}
