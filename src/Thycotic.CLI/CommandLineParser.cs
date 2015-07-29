using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Thycotic.CLI
{
    public static class CommandLineParser
    {
        public static string ParseInput(string input, out ConsoleCommandParameters parameters)
        {
            Contract.Ensures(Contract.Result<string>() != null);

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

            // parses input string into key-value pairs of arguments where each argument is in one of the following formats:
            // -key=value
            // -key=a-value-with-dashes
            // -key="another value"
            // input can contain multiple arguments
            // the argument key should be any string not containing spaces, the argument value can contain spaces if it is surrounded in quotes
            var regexParameters = new Regex(@"-(""(?:\\.|[^""])*""|[^\s]*)=(""(?:\\.|[^""])*""|[^\s]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var parameterMatches = regexParameters.Matches(input);

            foreach (Match parameterMatch in parameterMatches)
            {
                // -foo="bar baz" => [1] = foo, [2] = bar baz
                parameters[parameterMatch.Groups[1].Value] = parameterMatch.Groups[2].Value.Trim().Replace(@"""","");
            }

            return commandName;
        }
    }
}
