using System;

namespace Thycotic.CLI.Fragments
{
    public class BinaryCommandFragment : ICommandFragment
    {
        public Func<ConsoleCommandParameters, int> Action { get; set; }

        public string Name { get; set; }
        public string Prompt { get; set; }
        public ICommandFragment WhenTrue { get; set; }
        public ICommandFragment WhenFalse { get; set; }

        public BinaryCommandFragment()
        {
            WhenFalse = new NoopCommandFragement();
            WhenTrue = new NoopCommandFragement();

            Action = parameters =>
            {
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
