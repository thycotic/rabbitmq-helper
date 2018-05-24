using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.PSCommands.Certificate;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Copies RabbitMq TLS example configuration file. The configuration file will be located in the Thycotic RabbitMq Site Connector folder.
    /// </summary>
    /// <para type="synopsis">Copies RabbitMq TLS example configuration file.</para>
    /// <para type="description">The new-RabbitMqTlsConfigFiles cmdlet copies RabbitMq TLS example configuration file.</para>
    /// <para type="description">The configuration file will be located in the Thycotic RabbitMq Site Connector folder.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Copy-RabbitMqExampleNonSslConfigFile</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>New-RabbitMqExampleSslConfigFile</code>
    /// </example>
    [Cmdlet(VerbsCommon.New, "RabbitMqTlsConfigFiles")]
    public class NewRabbitMqTlsConfigFilesCommand : NewRabbitMqNonTlsConfigFilesCommand
    {
        /// <summary>
        /// Gets the default configuration settings.
        /// </summary>
        /// <returns></returns>
        protected override IDictionary<string, string> GetDefaultConfigurationSettings()
        {
            var defaultSettings = base.GetDefaultConfigurationSettings();

            var tlsSettings = new Dictionary<string, string>
                {
                    {"listeners.ssl.default", "5671"},

                    {"ssl_options.versions.1", "tlsv1.2"},
                    {"ssl_options.versions.2", "tlsv1.1"},
                    //{"ssl_options.versions.3", "tlsv1"},

                    {"ssl_options.verify", "verify_peer"},
                    {"ssl_options.fail_if_no_peer_cert", "false"},
                    {"ssl_options.cacertfile", ConvertCaCerToPemCommand.CertificatePath},
                    {"ssl_options.certfile", ConvertPfxToPemCommand.CertificatePath},
                    {"ssl_options.keyfile", ConvertPfxToPemCommand.KeyPath}
                };

            return new[] {defaultSettings, tlsSettings}.SelectMany(dict => dict)
                .ToLookup(pair => pair.Key, pair => pair.Value)
                .ToDictionary(group => group.Key, group => group.First());

        }
    }
}