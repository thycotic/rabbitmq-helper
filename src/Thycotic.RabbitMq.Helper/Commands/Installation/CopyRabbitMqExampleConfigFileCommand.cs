using System.IO;
using System.Reflection;
using Thycotic.CLI;
using Thycotic.CLI.Commands;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Certificate;
using Thycotic.RabbitMq.Helper.Commands;

namespace Thycotic.RabbitMq.Helper.Installation
{
    internal class CopyRabbitMqExampleConfigFileCommand : CommandBase
    {

        private static class TokenNames
        {
            public const string PathToCaCert = "%THYCOTIC_PATHTOCACERT%";
            public const string PathToCert = "%THYCOTIC_PATHTOCERT%";
            public const string PathToKey = "%THYCOTIC_PATHTOKEY%";
        }

        private readonly ILogWriter _log = Log.Get(typeof(CopyRabbitMqExampleConfigFileCommand));

        public override string Area
        {
            get { return CommandAreas.Installation; }
        }

        public override string Description
        {
            get { return "Copies RabbitMq example configuration file"; }
        }

        public CopyRabbitMqExampleConfigFileCommand()
        {

            Action = parameters =>
            {

                if (!File.Exists(ConvertCaCerToPemCommand.CertificatePath))
                {
                    throw new FileNotFoundException("CA certificate not found");
                }

                if (!File.Exists(ConvertPfxToPemCommand.CertificatePath))
                {
                    throw new FileNotFoundException("Certificate not found");
                }

                if (!File.Exists(ConvertPfxToPemCommand.KeyPath))
                {
                    throw new FileNotFoundException("Key not found");
                }

                _log.Info("Creating RabbitMq configuration file.");
                
                var contentAssembly = Assembly.GetAssembly(typeof(Program));

                var resourceName = string.Format("{0}.Content.RabbitMq._3._5._3.Ssl.rabbitmq.config.erlang",
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

                ReplaceToken(ref contents, TokenNames.PathToCaCert, ConvertCaCerToPemCommand.CertificatePath);
                ReplaceToken(ref contents, TokenNames.PathToCert, ConvertPfxToPemCommand.CertificatePath);
                ReplaceToken(ref contents, TokenNames.PathToKey, ConvertPfxToPemCommand.KeyPath);

                var configFilePath = Path.Combine(InstallationConstants.RabbitMq.ConfigurationPath,
                    "rabbitmq.config");

                File.WriteAllText(configFilePath, contents);

                return 0;
            };
        }

        private static void ReplaceToken(ref string contents, string tokenName, string value)
        {
            contents = contents.Replace(tokenName, value);
        }
    }
}