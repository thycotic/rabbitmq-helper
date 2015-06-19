using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;
using Thycotic.Utility;

namespace Thycotic.CLI
{
    public class CommandLineInterface
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly HashSet<IConsoleCommand> _commandBuiltInMappings = new HashSet<IConsoleCommand>();
        private readonly HashSet<IConsoleCommand> _commandCustomMappings = new HashSet<IConsoleCommand>();

        private readonly ILogWriter _log = Log.Get(typeof(CommandLineInterface));

        public CancellationToken CancellationToken
        {
            get { return _cts.Token; }
        }

        private readonly string _applicationName;

        public CommandLineInterface(string applicationName, string coreAreaName = "Core", string uncategorizedAreaName = "Uncategorized")
        {
            _applicationName = applicationName;

            #region BuildAll-in system commands
            _commandBuiltInMappings.Add(new SystemConsoleCommand
            {
                Name = "clear",
                Area = coreAreaName,
                Aliases = new[] { "cls" },
                Description = "Clears the terminal screen",
                Action = parameters => { Console.Clear(); return 0; }
            });

            _commandBuiltInMappings.Add(new SystemConsoleCommand
            {
                Name = "help",
                Area = coreAreaName,
                Aliases = new[] { "man", "h" },
                Description = "This screen",
                Action = parameters =>
                {
                    Console.WriteLine();
                    Console.WriteLine("Available commands: ".ToUpper());

                    var currentMappings = GetCurrentCommandMappings().ToArray();

                    currentMappings.Select(c => !string.IsNullOrWhiteSpace(c.Area) ? c.Area : uncategorizedAreaName).Distinct().OrderBy(a => a).ToList().ForEach(a =>
                    {
                        Console.WriteLine("{0} command area", a);

                        if (a == uncategorizedAreaName) a = null;

                        var mappings =
                            currentMappings.Where(c => c.Area == a).OrderBy(c => c.Name).Select(c => c.ToString());

                        mappings.ToList().ForEach(m => Console.WriteLine(" - {0}", m));

                        Console.WriteLine();
                    });

                    return 0;
                }
            });

            _commandBuiltInMappings.Add(new SystemConsoleCommand
            {
                Name = "quit",
                Area = coreAreaName,
                Aliases = new[] { "exit", "q" },
                Description = "Quits/exists the application",
                Action = parameters => { _cts.Cancel(); return 0; }
            });
            #endregion

            ConfigureConsoleWindow();
        }

        private void ConfigureConsoleWindow()
        {
            try
            {
                Console.Title = string.Format("{0} in interactive mode v.{1} ({2})", _applicationName, ReleaseInformationHelper.Version, ReleaseInformationHelper.Architecture);

                InteropHelper.DisableCloseMenuItem();
                InteropHelper.Maximize();
            }
            catch
            {
                //console not available
            }
        }

        public void AddCustomCommand(IConsoleCommand command)
        {
            _commandCustomMappings.Add(command);
        }

        public void ClearCommands()
        {
            _commandCustomMappings.Clear();
        }

        private IEnumerable<IConsoleCommand> GetCurrentCommandMappings()
        {
            return _commandCustomMappings.Union(_commandBuiltInMappings);
        }

        public void ConsumeInput(string initialCommand)
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
                throw new ArgumentException("No input provided");
            }

            var commandName = ParseInput(input, out parameters);
            HandleCommand(commandName, parameters);

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
                    input = PromptForInput();
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

        private static string PromptForInput()
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

            var regexParameters = new Regex(@"-([\w]+)=\""?([\d\w\s.:\\\[\]@]+)\""?", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var parameterMatches = regexParameters.Matches(input);

            foreach (Match parameterMatch in parameterMatches)
            {
                // -foo="bar baz" => [1] = foo, [2] = bar baz
                parameters.Add(parameterMatch.Groups[1].Value.ToLower(), parameterMatch.Groups[2].Value.Trim());
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
                        GetCurrentCommandMappings().SingleOrDefault(
                            cm =>
                                (cm.Name == commandName) ||
                                ((cm.Aliases != null) && cm.Aliases.Any(ca => ca == commandName)));

                    if (command == null)
                    {
                        _log.Error(string.Format("Command {0} not found", commandName));
                        command = _commandCustomMappings.Single(
                            cm => cm.Name == "help");
                    }

                    if (command is IImmediateConsoleCommand)
                    {
                        command.Action.Invoke(parameters);
                    }
                    else
                    {
                        Task.Factory.StartNew(() => command.Action.Invoke(parameters), CancellationToken).ContinueWith(
                            task =>
                            {
                                if (task.Exception != null)
                                {
                                    _log.Error(string.Format("Command failed because {0}", task.Exception.Message), task.Exception);
                                }
                            }, CancellationToken);


                    }


                }
                catch (Exception ex)
                {
                    _log.Error(string.Format("Command failed because {0}", ex.Message), ex);
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