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
            _log.Info(string.Format("Application path is {0}", runbook.ApplicationPath));
            _log.Info(string.Format("Working path is {0}", runbook.WorkingPath));
            _log.Info(string.Format("Source path is {0}", runbook.SourcePath));
            if (!string.IsNullOrWhiteSpace(runbook.RecipePath))
            {
                _log.Info(string.Format("Recipe path is {0}", runbook.RecipePath));
            }


            if (string.IsNullOrWhiteSpace(runbook.ArtifactName))
            {
                runbook.ArtifactName = runbook.GetArtifactFileName(runbook.DefaultArtifactName, runbook.ArtifactNameSuffix, runbook.Is64Bit, runbook.Version);
            }

            var path = Path.GetFullPath(Path.Combine(runbook.WorkingPath, runbook.ArtifactName));

            _log.Info(string.Format("Full temporary artifact path will be {0}", path));
            
            _log.Info("Baking steps");
            runbook.BakeSteps();

            runbook.Steps.ToList().ForEach(s =>
            {
                _log.Info(string.Format("Executing {0}", s.Name ?? "Unnamed step"));

                s.Execute();
            });
            
            return path;
        }
    }
}
