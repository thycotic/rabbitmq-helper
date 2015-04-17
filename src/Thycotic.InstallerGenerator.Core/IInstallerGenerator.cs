using Thycotic.InstallerGenerator.Core.Steps;

namespace Thycotic.InstallerGenerator.Core
{
    public interface IInstallerGenerator<in TSteps> 
        where TSteps : IInstallerGeneratorRunbook
    {
        string Generate(TSteps steps);
    }
}
