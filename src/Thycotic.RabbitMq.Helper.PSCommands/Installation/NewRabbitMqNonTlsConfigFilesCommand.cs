﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.Logic.IO;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Copies RabbitMq non-TLS example configuration file. The configuration file will be located in the Thycotic RabbitMq Site Connector folder.
    /// </summary>
    /// <para type="synopsis">Copies RabbitMq non-TLS example configuration file. </para>
    /// <para type="description">The Copy-RabbitMqExampleNonTlsConfigFile cmdlet copies RabbitMq non-TLS example configuration file.</para>
    /// <para type="description">The configuration file will be located in the Thycotic RabbitMq Site Connector folder.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">New-RabbitMqTlsConfigFiles</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>New-RabbitMqNonTlsConfigFiles</code>
    /// </example>
    [Cmdlet(VerbsCommon.New, "RabbitMqNonTlsConfigFiles")]
    public class NewRabbitMqNonTlsConfigFilesCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Could not locate sample configuration file</exception>
        protected override void ProcessRecord()
        {
            CopyExampleConfigurationFile();
            CopyAdvancedConfigurationFile();
            GenerateConfigurationFile();
        }

        /// <summary>
        /// Copies the example configuration file.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Could not locate sample configuration file</exception>
        protected void CopyExampleConfigurationFile()
        {
            WriteVerbose("Creating RabbitMq example configuration file.");

            var contentAssembly = GetType().Assembly;

            var resourceName = string.Format("{0}.Content.RabbitMq._3._7._5.rabbitmq.conf.example",
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
                "rabbitmq.conf.example");

            File.WriteAllText(configFilePath, contents);
        }

        /// <summary>
        /// Copies the advanced configuration file.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        protected void CopyAdvancedConfigurationFile()
        {
            WriteVerbose("Creating RabbitMq advanced configuration file.");

            var contentAssembly = GetType().Assembly;

            var resourceName = string.Format("{0}.Content.RabbitMq._3._7._5.advanced.config.example",
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
                "advanced.config");

            File.WriteAllText(configFilePath, contents);
        }

        private void GenerateConfigurationFile()
        {
            WriteVerbose("Creating RabbitMq configuration file.");

            var contents = string.Join(Environment.NewLine,
                GetDefaultConfigurationSettings().Select(kvp =>
                {
                    //comment
                    if (kvp.Key.StartsWith("#") && string.IsNullOrWhiteSpace(kvp.Value))
                    {
                        return kvp.Key;
                    }

                    //configuration value
                    return $"{kvp.Key} = {kvp.Value}";
                }));
           
            var configFilePath = Path.Combine(InstallationConstants.RabbitMq.ConfigurationPath,
                "rabbitmq.conf");

            File.WriteAllText(configFilePath, contents);

        }

        /// <summary>
        /// Gets the default configuration settings.
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string,string> GetDefaultConfigurationSettings()
        {
            return new Dictionary<string, string>
            {
                {"listeners.tcp.default","5672" },

                {"# logging to file and/or to an exchange", ""},
                {"# log.dir", RabbitMqConfigurationPathSanitizer.Sanitize(@"C:\temp")},
                {"log.file", "rabbit.log"},
                {"# log.file", "false"},
                {"log.file.level", "error"},

                {"# log.exchange", "true"},
                {"# log.exchange.level", "error"},
            };
        }
    }
}