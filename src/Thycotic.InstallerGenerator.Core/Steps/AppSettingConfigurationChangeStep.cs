using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.Core.Steps
{
    public class AppSettingConfigurationChangeStep : IInstallerGeneratorStep
    {
        public string Name { get; set; }

        public string ConfigurationFilePath { get; set; }
        public IDictionary<string, string> Settings { get; set; }

        private readonly ILogWriter _log = Log.Get(typeof(AppSettingConfigurationChangeStep));

        public void Execute()
        {
            var configFileMap = new ExeConfigurationFileMap {ExeConfigFilename = ConfigurationFilePath};
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
  
            Settings.ToList().ForEach(setting =>
            {
                _log.Info(string.Format("Setting value for {0}", setting.Key));
                config.AppSettings.Settings[setting.Key].Value = setting.Value;
            });

            //only write modified fields
            config.Save(ConfigurationSaveMode.Minimal);  
        }

        
    }
}