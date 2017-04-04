using System.IO;
using System.Management.Automation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Copies RabbitMq non-SSL example configuration file. The configuration file will be located in the Thycotic RabbitMq Site Connector folder.
    /// </summary>
    /// <para type="synopsis">Copies RabbitMq non-SSL example configuration file. </para>
    /// <para type="description">The Copy-RabbitMqExampleNonSslConfigFile cmdlet copies RabbitMq non-SSL example configuration file.</para>
    /// <para type="description">The configuration file will be located in the Thycotic RabbitMq Site Connector folder.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Copy-RabbitMqExampleSslConfigFile</para>
    /// <example>
    ///     <code>Copy-RabbitMqExampleNonSslConfigFile</code>
    /// </example>
    [Cmdlet(VerbsCommon.Copy, "RabbitMqExampleNonSslConfigFile")]
    public class CopyRabbitMqExampleNonSslConfigFileCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Could not locate sample configuration file</exception>
        protected override void ProcessRecord()
        {
            WriteVerbose("Creating RabbitMq configuration file.");

            var contentAssembly = GetType().Assembly;

            var resourceName = string.Format("{0}.Content.RabbitMq._3._5._3.NonSsl.rabbitmq.config.erlang",
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

            var configFilePath = Path.Combine(InstallationConstants.RabbitMq.ConfigurationPath,
                "rabbitmq.config");

            File.WriteAllText(configFilePath, contents);
        }
    }
}