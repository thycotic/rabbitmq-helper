using System;
using System.Collections.Generic;
using System.Configuration;
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
            var configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = ConfigurationFilePath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            Settings.ToList().ForEach(setting =>
            {
                try
                {
                    _log.Info(string.Format("Setting value for {0}", setting.Key));
                    config.AppSettings.Settings[setting.Key].Value = setting.Value;
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