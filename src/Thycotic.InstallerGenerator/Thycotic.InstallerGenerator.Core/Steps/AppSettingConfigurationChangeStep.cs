using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.Core.Steps
{
    /// <summary>
    /// Application setting configuration change step
    /// </summary>
    public class AppSettingConfigurationChangeStep : IInstallerGeneratorStep
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the configuration file path.
        /// </summary>
        /// <value>
        /// The configuration file path.
        /// </value>
        public string ConfigurationFilePath { get; set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public IDictionary<string, string> Settings { get; set; }

        private readonly ILogWriter _log = Log.Get(typeof(AppSettingConfigurationChangeStep));

        /// <summary>
        /// Executes the step.
        /// </summary>
        public void Execute()
        {
            if (!File.Exists(ConfigurationFilePath))
            {
                throw new FileNotFoundException(string.Format("Configuration file not found at {0}", ConfigurationFilePath));
            }

            _log.Debug(string.Format("Opening {0} for modifications", ConfigurationFilePath));

            var configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = ConfigurationFilePath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            Settings.ToList().ForEach(setting =>
            {
                try
                {
                    if (config.AppSettings.Settings.AllKeys.Contains(setting.Key))
                    {
                        _log.Info(string.Format("Replacing value for {0}", setting.Key));

                        config.AppSettings.Settings[setting.Key].Value = setting.Value;
                    }
                    else
                    {
                        _log.Info(string.Format("Adding value for {0}", setting.Key));

                        config.AppSettings.Settings.Add(setting.Key, setting.Value);

                    }
                }
                catch (Exception)
                {
                    throw new ApplicationException(string.Format("Could not find and replace app setting with key {0}", setting.Key));
                }
            });

            //only write modified fields
            config.Save(ConfigurationSaveMode.Minimal);
        }
    }
}