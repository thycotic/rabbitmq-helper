using System.IO;
using System.Linq;
using Thycotic.InstallerGenerator.Core;
using Thycotic.InstallerGenerator.Core.MSI.WiX;

namespace Thycotic.InstallerGenerator.MSI.WiX
{
    public class WiXMsiGenerator : IInstallerGenerator<WiXMsiGeneratorRunbook>
    {
        public string Generate(WiXMsiGeneratorRunbook runbook)
        {
            runbook.Steps.ToList().ForEach(s=> s.Execute() );
            
            return Path.Combine(runbook.WorkingPath, runbook.ArtifactName);
        }
    }
}
