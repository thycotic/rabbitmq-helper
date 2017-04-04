﻿using System.IO;
using System.Management.Automation;

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