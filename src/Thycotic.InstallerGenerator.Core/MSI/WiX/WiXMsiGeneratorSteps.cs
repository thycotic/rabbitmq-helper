namespace Thycotic.InstallerGenerator.Core.MSI.WiX
{
    /// <summary>
    /// Sets to generate a Wix MSI. 
    /// </summary>
    public class WiXMsiGeneratorSteps : InstallerGeneratorSteps
    {
        public WiXMsiGeneratorSubsteps Substeps { get; set; }
        
    }
}