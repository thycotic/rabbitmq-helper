using System.IO;
using System.Linq;
using Thycotic.Logging;

namespace Thycotic.InstallerGenerator.Core
{
    /// <summary>
    /// Basic installer generator
    /// </summary>
    public class BasicInstallerGenerator : IInstallerGenerator<IInstallerGeneratorRunbook>
    {
        private readonly ILogWriter _log = Log.Get(typeof(BasicInstallerGenerator));

        /// <summary>
        /// Generates the specified runbook.
        /// </summary>
        /// <param name="runbook">The runbook.</param>
        /// <returns></returns>
        public string Generate(IInstallerGeneratorRunbook runbook)
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
