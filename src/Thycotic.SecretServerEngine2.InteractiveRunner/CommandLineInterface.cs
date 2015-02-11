using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;
using Thycotic.SecretServerEngine2.InteractiveRunner.ConsoleCommands;

namespace Thycotic.SecretServerEngine2.InteractiveRunner
{
    internal class CommandLineInterface
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly HashSet<IConsoleCommand> _commandMappings = new HashSet<IConsoleCommand>();

        private readonly ILogWriter _log = Log.Get(typeof(CommandLineInterface));

        public CommandLineInterface()
        {
            #region Build-in system commands
            _commandMappings.Add(new SystemConsoleCommand
            {
                Name = "clear",
                Area = CommandAreas.Core,
                Aliases = new[] { "cls" },
                Description = "Clears the terminal screen",
                Action = parameters => Console.Clear()
            });

            _commandMappings.Add(new SystemConsoleCommand
            {
                Name = "help",
                Area = CommandAreas.Core,
                Aliases = new[] { "man", "h" },
                Description = "This screen",
                Action = parameters =>
                {
                    Console.WriteLine();
                    Console.WriteLine("Available commands: ".ToUpper());

                    _commandMappings.Select(c => !string.IsNullOrWhiteSpace(c.Area) ? c.Area : CommandAreas.Uncategorized).Distinct().OrderBy(a => a).ToList().ForEach(a =>
                    {
                        Console.WriteLine("{0} command area", a);

                        if (a == CommandAreas.Uncategorized) a = null;

                        var mappings =
                            _commandMappings.Where(c => c.Area == a).OrderBy(c => c.Name).Select(c => c.ToString());

                        mappings.ToList().ForEach(m => Console.WriteLine(" - {0}", m));

                        Console.WriteLine();
                    });
                }
            });

            _commandMappings.Add(new SystemConsoleCommand
            {
                Name = "quit",
                Area = CommandAreas.Core,
                Aliases = new[] { "exit", "q" },
                Description = "Quits/exists the application",
                Action = parameters => _cts.Cancel()
            });
            #endregion

            ConfigureConsoleWindow();
        }

        private static void ConfigureConsoleWindow()
        {
            Console.Title = string.Format("Secret Server Agent in interactive mode v.{0} ({1})", ReleaseInformationHelper.Version, ReleaseInformationHelper.Architecture);

            InteropHelper.DisableCloseMenuItem();

            InteropHelper.Maximize();
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
                            cm => cm.Name == "help");
                    }

                    Task.Factory.StartNew(() => command.Action.Invoke(parameters));
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

        private static class InteropHelper
        {
            #region Close window related imports
            private const int MfBycommand = 0x00000000;

            private const int ScClose = 0xF060;

            [DllImport("user32.dll")]
            private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

            [DllImport("user32.dll")]
            private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

            [DllImport("user32.dll")]
            private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
            #endregion

            #region Maximize related imports
            [DllImport("kernel32.dll", ExactSpelling = true)]
            private static extern IntPtr GetConsoleWindow();
            #endregion

            public static void DisableCloseMenuItem()
            {
                //prevent users from using Ctrl-C
                Console.CancelKeyPress += (sender, args) => { args.Cancel = true; };

                DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), ScClose, MfBycommand);
            }

            public static void Maximize()
            {
                Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

                var p = Process.GetCurrentProcess();
                ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3
            }
        }
    }
}