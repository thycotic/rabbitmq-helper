using System.IO;
using System.Linq;
using Thycotic.InstallerGenerator.Core;
using Thycotic.InstallerGenerator.Core.MSI.WiX;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.MSI.WiX
{
    /// <summary>
    /// WiX MSI generator
    /// </summary>
    public class WiXMsiGenerator : IInstallerGenerator<WiXMsiGeneratorRunbook>
    {
        private readonly ILogWriter _log = Log.Get(typeof(WiXMsiGenerator));

        /// <summary>
        /// Generates the specified runbook.
        /// </summary>
        /// <param name="runbook">The runbook.</param>
        /// <returns></returns>
        public string Generate(WiXMsiGeneratorRunbook runbook)
        {
            _log.Info("Baking steps");
            runbook.BakeSteps();

            runbook.Steps.ToList().ForEach(s =>
            {
                _log.Info(string.Format("Executing {0}", s.Name ?? "Unnamed step"));

                s.Execute();
            });
            
            return Path.GetFullPath(Path.Combine(runbook.WorkingPath, runbook.ArtifactName));
        }
    }
}
