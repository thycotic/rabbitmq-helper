using System;
using System.IO;
using System.Linq;

namespace Thycotic.InstallerGenerator.MSI.WiX
{
    public class WiXMsiGenerator : IInstallerGenerator<WiXMsiGeneratorSteps>
    {
        public string Generate(WiXMsiGeneratorSteps steps)
        {
            this.ExecuteExternalProcess(steps.WorkingPath, ToolPaths.Heat,
                this.SanitizeExternalProcessArguments(steps.Substeps.Heat), "Heat process");
            this.ExecuteExternalProcess(steps.WorkingPath, ToolPaths.Candle,
                this.SanitizeExternalProcessArguments(steps.Substeps.Candle), "Candle process");
            this.ExecuteExternalProcess(steps.WorkingPath, ToolPaths.Light,
                this.SanitizeExternalProcessArguments(steps.Substeps.Light), "Light process");

            return Path.Combine(steps.WorkingPath, steps.ArtifactName);
        }
    }
}
