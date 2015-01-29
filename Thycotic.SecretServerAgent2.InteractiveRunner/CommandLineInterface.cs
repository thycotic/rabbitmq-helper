using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Thycotic.Logging;
using Thycotic.SecretServerAgent2.InteractiveRunner.ConsoleCommands;

namespace Thycotic.SecretServerAgent2.InteractiveRunner
{
    internal class CommandLineInterface
    {
        private const string HelpCommandName = "help";
        private const string QuitCommandName = "quit";

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly HashSet<IConsoleCommand> _commandMappings = new HashSet<IConsoleCommand>();

        private readonly ILogWriter _log = Log.Get(typeof(CommandLineInterface));

        public CommandLineInterface()
        {
            var helpCommand = new SystemConsoleCommand
            {
                Name = HelpCommandName,
                Aliases = new[] { "man", "h"},
                Description = "This screen",
                Action = parameters =>
                {
                    Console.WriteLine();
                    Console.WriteLine("Available commands: ".ToUpper());

                    var mappings = _commandMappings.OrderBy(c => c.Name).Select(c => c.ToString());

                    mappings.ToList().ForEach(m => Console.WriteLine(" - {0}", m));
                }
            };

            _commandMappings.Add(helpCommand);

            var quitCommand = new SystemConsoleCommand
            {
                Name = QuitCommandName,
                Aliases = new [] {"exit", "q" },
                Description = "Quits/exists the application",
                Action = parameters => _cts.Cancel()
            };

            _commandMappings.Add(quitCommand);
        }

        public void AddCommand(IConsoleCommand command)
        {
            _commandMappings.Add(command);
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

                var commandName = ParseInput(input, out parameters);
                HandleCommand(commandName, parameters);

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
            var input = Console.ReadLine();

            return !string.IsNullOrWhiteSpace(input) ? input.Trim() : string.Empty;
        }

        private static string ParseInput(string input, out ConsoleCommandParameters parameters)
        {
            var commandName = string.Empty;
            parameters = new ConsoleCommandParameters();

            //no command
            if (string.IsNullOrWhiteSpace(input)) return commandName;

            var regexCommand = new Regex(@"^([\w]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var commandMatches = regexCommand.Matches(input);
            if (commandMatches.Count == 1)
            {
                commandName = commandMatches[0].Groups[0].Value;
            }

            var regexParameters = new Regex(@"-([\w]+)=\""?([\d\w\s.]+)\""?", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var parameterMatches = regexParameters.Matches(input);

            foreach (Match parameterMatch in parameterMatches)
            {
                // -foo="bar baz" => [1] = foo, [2] = bar baz
                parameters.Add(parameterMatch.Groups[1].Value, parameterMatch.Groups[2].Value);
            }

            return commandName;

        }

        private void HandleCommand(string commandName, ConsoleCommandParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(commandName)) return;

            using (LogContext.Create(commandName))
            {
                try
                {
                    var command =
                        _commandMappings.SingleOrDefault(
                            cm =>
                                (cm.Name == commandName) ||
                                ((cm.Aliases != null) && cm.Aliases.Any(ca => ca == commandName)));

                    if (command == null)
                    {
                        _log.Error(string.Format("Command {0} not found", commandName));
                        command = _commandMappings.Single(
                            cm => cm.Name == HelpCommandName);
                    }

                    command.Action.Invoke(parameters);
                }
                catch (Exception ex)
                {
                    _log.Error(string.Format("Command failed because {0}", ex.Message));
                }

            }
        }


        public void Wait()
        {
            _cts.Token.WaitHandle.WaitOne();
        }
    }
}