namespace Thycotic.InstallerGenerator.Core.Steps
{
    public interface IInstallerGeneratorStep
    {
        string Name { get; }

        void Execute();
    }
}