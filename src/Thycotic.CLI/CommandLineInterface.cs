using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Thycotic.CLI.Commands;
using Thycotic.Logging;
using Thycotic.Utility;

namespace Thycotic.CLI
{
    public class CommandLineInterface
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly HashSet<ICommand> _commandBuiltInMappings = new HashSet<ICommand>();
        private readonly HashSet<ICommand> _commandCustomMappings = new HashSet<ICommand>();

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
            _commandBuiltInMappings.Add(new SystemCommand
            {
                CustomName = "clear",
                Area = coreAreaName,
                Aliases = new[] { "cls" },
                Description = "Clears the terminal screen",
                Action = parameters => { Console.Clear(); return 0; }
            });

            _commandBuiltInMappings.Add(new SystemCommand
            {
                CustomName = "help",
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
                        Console.WriteLine("{0}:", a.ToUpper());

                        if (a == uncategorizedAreaName) a = null;

                        var mappings =
                            currentMappings.Where(c => c.Area == a).OrderBy(c => c.Name).Select(c => c.ToString());

                        mappings.ToList().ForEach(m => Console.WriteLine("\t{0}", m));

                        Console.WriteLine();
                    });

                    return 0;
                }
            });

            _commandBuiltInMappings.Add(new SystemCommand
            {
                CustomName = "quit",
                Area = coreAreaName,
                Aliases = new[] { "exit", "q" },
                Description = "Quits/exists the application",
                Action = parameters => { _cts.Cancel(); return 0; }
            });
            #endregion

            ConfigureConsoleWindow();
        }

        /// <summary>
        /// Discovers the commands in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly. Defaults to the entry assembly if null</param>
        /// <returns></returns>
        public IContainer DiscoverCommands(Assembly assembly = null)
        {
            using (LogContext.Create("CLI configuration"))
            {
                ClearCommands();


                // Create the builder with which components/services are registered.
                var builder = new ContainerBuilder();

                builder.Register(context => CancellationToken).As<CancellationToken>().SingleInstance();

                builder.RegisterAssemblyTypes(assembly ?? Assembly.GetEntryAssembly())
                    .Where(t => !t.IsAbstract)
                    .Where(t => typeof(ICommand).IsAssignableFrom(t))
                    .Where(t => t != typeof(SystemCommand));

                var container = builder.Build();

                var commands =
                    container.ComponentRegistry.Registrations.Where(
                        r => typeof(ICommand).IsAssignableFrom(r.Activator.LimitType));

                commands.ToList()
                    .ForEach(c => AddCustomCommand((ICommand)container.Resolve(c.Activator.LimitType)));

                return container;
            }
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

        public void AddCustomCommand(ICommand command)
        {
            _commandCustomMappings.Add(command);
        }

        public void ClearCommands()
        {
            _commandCustomMappings.Clear();
        }

        private IEnumerable<ICommand> GetCurrentCommandMappings()
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

            var regexParameters = new Regex(@"-([\w.]+)=\""?([\d\w\s.:/\\\[\]@]+)\""?", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var parameterMatches = regexParameters.Matches(input);

            foreach (Match parameterMatch in parameterMatches)
            {
                // -foo="bar baz" => [1] = foo, [2] = bar baz
                parameters[parameterMatch.Groups[1].Value] = parameterMatch.Groups[2].Value.Trim();
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
                                (cm.Name.ToLower() == commandName.ToLower()) ||
                                ((cm.Aliases != null) && cm.Aliases.Any(ca => ca.ToLower() == commandName.ToLower())));

                    if (command == null)
                    {
                        _log.Error(string.Format("Command {0} not found", commandName));
                        command = _commandCustomMappings.Single(
                            cm => cm.Name == "Help");
                    }

                    if (command is IImmediateCommand)
                    {
                        var exitCode = command.Action.Invoke(parameters);
                        if (exitCode != 0)
                        {
                            Environment.Exit(exitCode);
                        }
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