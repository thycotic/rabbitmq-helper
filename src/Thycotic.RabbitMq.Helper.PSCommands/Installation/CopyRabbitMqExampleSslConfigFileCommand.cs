using System.IO;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.PSCommands.Certificate;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Copies RabbitMq example configuration file
    /// </summary>
    /// <para type="synopsis">TODO: This is the cmdlet synopsis.</para>
    /// <para type="description">TODO: This is part of the longer cmdlet description.</para>
    /// <para type="description">TODO: Also part of the longer cmdlet description.</para>
    /// <para type="link" uri="http://tempuri.org">TODO: Thycotic</para>
    /// <para type="link">TODO: Get-Help</para>
    /// <example>
    ///     <para>TODO: This is part of the first example's introduction.</para>
    ///     <para>TODO: This is also part of the first example's introduction.</para>
    ///     <code>TODO: New-Thingy | Write-Host</code>
    ///     <para>TODO: This is part of the first example's remarks.</para>
    ///     <para>TODO: This is also part of the first example's remarks.</para>
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

            var resourceName = string.Format("{0}.Content.RabbitMq._3._5._3.Ssl.rabbitmq.config.erlang",
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