using System.IO;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.PSCommands.Certificate;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Copies RabbitMq SSL example configuration file. The configuration file will be located in the Thycotic RabbitMq Site Connector folder.
    /// </summary>
    /// <para type="synopsis">Copies RabbitMq SSL example configuration file.</para>
    /// <para type="description">The Copy-RabbitMqExampleSslConfigFile cmdlet copies RabbitMq SSL example configuration file.</para>
    /// <para type="description">The configuration file will be located in the Thycotic RabbitMq Site Connector folder.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Copy-RabbitMqExampleNonSslConfigFile</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Copy-RabbitMqExampleSslConfigFile</code>
    /// </example>
    [Cmdlet(VerbsCommon.Copy, "RabbitMqExampleSslConfigFile")]
    public class CopyRabbitMqExampleSslConfigFileCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">
        ///     CA certificate not found
        ///     or
        ///     Certificate not found
        ///     or
        ///     Key not found
        ///     or
        ///     Could not locate sample configuration file
        /// </exception>
        protected override void ProcessRecord()
        {
            if (!File.Exists(ConvertCaCerToPemCommand.CertificatePath))
                throw new FileNotFoundException("CA certificate not found");

            if (!File.Exists(ConvertPfxToPemCommand.CertificatePath))
                throw new FileNotFoundException("Certificate not found");

            if (!File.Exists(ConvertPfxToPemCommand.KeyPath))
                throw new FileNotFoundException("Key not found");

            WriteVerbose("Creating RabbitMq configuration file.");

            var contentAssembly = GetType().Assembly;

            var resourceName = string.Format("{0}.Content.RabbitMq._3._7._5.Ssl.rabbitmq.config.erlang",
                contentAssembly.GetName().Name);

            string contents;

            using (var stream = contentAssembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new FileNotFoundException("Could not locate sample configuration file");

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
        }

        private static void ReplaceToken(ref string contents, string tokenName, string value)
        {
            //rabbit doesn't like single slashed in paths
            value = value.Replace(@"\", @"\\");

            contents = contents.Replace(tokenName, value);
        }

        private static class TokenNames
        {
            public const string PathToCaCert = "%THYCOTIC_PATHTOCACERT%";
            public const string PathToCert = "%THYCOTIC_PATHTOCERT%";
            public const string PathToKey = "%THYCOTIC_PATHTOKEY%";
        }
    }
}