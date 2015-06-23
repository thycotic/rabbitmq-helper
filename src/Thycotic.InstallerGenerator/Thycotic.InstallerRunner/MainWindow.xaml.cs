using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Thycotic.CLI.OS;
using Thycotic.Utility.Reflection;

namespace Thycotic.InstallerRunner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            //TODO: OMG REFACTOR THIS HORRIBLE CODE -dkk

            var appSettings = ConfigurationManager.AppSettings;

            var parameters = new Dictionary<string, string>();

            appSettings.AllKeys.Where(k => k != "Path.MSI").ToList().ForEach(k => parameters.Add(k, appSettings[k]));

            var processRunner = new ExternalProcessRunner();

            var msiPath = appSettings["Path.MSI"];

            var assemblyEntrypointProvider = new AssemblyEntryPointProvider();

            var workingPath = assemblyEntrypointProvider.GetAssemblyDirectory(GetType());

            try
            {
                var coreMsiParameters = string.Format(@"/i ""{0}"" /qn /log install.log", msiPath);
                var serviceParameters = string.Join(" ",
                    parameters.Select(p => string.Format(@"{0}=""{1}""", p.Key, p.Value)));

                var processInfo = new ProcessStartInfo("msiexec")
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WorkingDirectory = workingPath,
                    Arguments = string.Join(" ", coreMsiParameters, serviceParameters)
                };
                
                processRunner.Run(processInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.InnerException.Message);
                throw;
            }
        }
    }
}
