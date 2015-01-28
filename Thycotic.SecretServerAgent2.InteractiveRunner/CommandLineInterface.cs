using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Thycotic.Logging;

namespace Thycotic.SecretServerAgent2.InteractiveRunner
{
    internal class CommandLineInterface
    {
        private const string HelpCommandName = "help";
        private const string DefaultCommandName = HelpCommandName;
        private const string QuitCommandName = "quit";

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Dictionary<ConsoleCommand, Action<ConsoleCommandParameters>> _commandMappings = new Dictionary<ConsoleCommand, Action<ConsoleCommandParameters>>();

        private readonly ILogWriter _log = Log.Get(typeof(CommandLineInterface));

        public CommandLineInterface()
        {
            _commandMappings.Add(new ConsoleCommand { Name = HelpCommandName, Description = "This screen" }, parameters =>
            {
                Console.WriteLine("Available commands: ");
                _commandMappings.Keys.OrderBy(cm => cm.Name).ToList().ForEach(cm => Console.WriteLine(" - {0}", cm));
            });

            _commandMappings.Add(new ConsoleCommand { Name = QuitCommandName, Description = "Quits/exists the application" }, parameters => _cts.Cancel());
        }

        public void AddCommand(ConsoleCommand command, Action<ConsoleCommandParameters> action)
        {
            _commandMappings.Add(command, action);
        }

        public void BeginInputLoop(string initialCommand)
        {
            do
            {
                ConsoleCommandParameters parameters;

                string input;

                if (!string.IsNullOrWhiteSpace(initialCommand))
                {
                    //escape quotes
                    input = initialCommand.Replace("&quot;", "\"");

                    Console.WriteLine();
                    Console.Write("{0}@{1} # ", Environment.UserName, Environment.MachineName);
                    Console.WriteLine(input);
                }
                else
                {
                    input = ConsumeInput();
                }
            
                var command = ParseInput(input, out parameters);
                HandleCommand(command, parameters);

                //reset so there is no initial input
                if (!string.IsNullOrWhiteSpace(initialCommand))
                {
                    initialCommand = string.Empty;
                }

            } while (!_cts.IsCancellationRequested);
        }

        private static string ConsumeInput()
        {
            Console.WriteLine();
            Console.Write("{0}@{1} # ", Environment.UserName, Environment.MachineName);
            return Console.ReadLine();
        }

        private static ConsoleCommand ParseInput(string input, out ConsoleCommandParameters parameters)
        {
            var command = new ConsoleCommand { Name = DefaultCommandName };
            parameters = new ConsoleCommandParameters();

            //no command
            if (string.IsNullOrWhiteSpace(input)) return command;

            var regexCommand = new Regex(@"^([\w]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var commandMatches = regexCommand.Matches(input);
            if (commandMatches.Count == 1)
            {
                command.Name = commandMatches[0].Groups[0].Value;
            }

            var regexParameters = new Regex(@"-([\w]+)=\""?([\w\s.]{2,})\""?", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var parameterMatches = regexParameters.Matches(input);

            foreach (Match parameterMatch in parameterMatches)
            {
                // -foo="bar baz" => [1] = foo, [2] = bar baz
                parameters.Add(parameterMatch.Groups[1].Value, parameterMatch.Groups[2].Value);
            }

            return command;

        }

        private void HandleCommand(ConsoleCommand command, ConsoleCommandParameters parameters)
        {
            if (!_commandMappings.ContainsKey(command))
            {
                _log.Error(string.Format("Command {0} not found", command.Name));
                command = new ConsoleCommand { Name = DefaultCommandName };
            }

            _commandMappings[command].Invoke(parameters);

            Console.WriteLine();
        }


        public void Wait()
        {
            _cts.Token.WaitHandle.WaitOne();
        }
    }
}