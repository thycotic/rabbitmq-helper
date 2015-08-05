using System.IO;
using System.Reflection;
using Thycotic.CLI.Commands;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper.Commands.Installation
{
    internal class CopyRabbitMqExampleNonSslConfigFileCommand : CommandBase, IImmediateCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(CopyRabbitMqExampleNonSslConfigFileCommand));

        public override string Area
        {
            get { return CommandAreas.Installation; }
        }

        public override string Description
        {
            get { return "Copies RabbitMq example configuration file"; }
        }

        public CopyRabbitMqExampleNonSslConfigFileCommand()
        {

            Action = parameters =>
            {

                _log.Info("Creating RabbitMq configuration file.");
                
                var contentAssembly = Assembly.GetAssembly(typeof(Program));

                var resourceName = string.Format("{0}.Content.RabbitMq._3._5._3.NonSsl.rabbitmq.config.erlang",
                    contentAssembly.GetName().Name);

                string contents;

                using (var stream = contentAssembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        throw new FileNotFoundException("Could not locate sample configuration file");
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        contents = reader.ReadToEnd();

                    }
                }

                var configFilePath = Path.Combine(InstallationConstants.RabbitMq.ConfigurationPath,
                    "rabbitmq.config");

                File.WriteAllText(configFilePath, contents);

                return 0;
            };
        }
    }
}