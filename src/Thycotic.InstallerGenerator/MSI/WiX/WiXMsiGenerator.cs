using System.IO;
using System.Linq;
using Thycotic.InstallerGenerator.Core;
using Thycotic.InstallerGenerator.Core.MSI.WiX;

namespace Thycotic.InstallerGenerator.MSI.WiX
{
    public class WiXMsiGenerator : IInstallerGenerator<WiXMsiGeneratorSteps>
    {
        public string Generate(WiXMsiGeneratorSteps steps)
        {
            steps.Steps.ToList().ForEach(s=> s.Execute() );
            
            return Path.Combine(steps.WorkingPath, steps.ArtifactName);
        }
    }
}
