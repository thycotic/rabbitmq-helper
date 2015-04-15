using System.Configuration;

namespace Thycotic.InstallerGenerator.Core.Steps
{
    public class AppSettingConfigurationChangeStep : IInstallerGeneratorStep
    {
        public string Name { get; set; }

        public string ConfigurationFilePath { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        
        public void Execute()
        {
            var configFileMap = new ExeConfigurationFileMap {ExeConfigFilename = ConfigurationFilePath};
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
  
            config.AppSettings.Settings[SettingKey].Value = SettingValue;

            //only write modified fields
            config.Save(ConfigurationSaveMode.Minimal);  
        }

        
    }
}