using System;
using System.IO;
using Thycotic.InstallerGenerator.Core.Steps;

namespace Thycotic.InstallerGenerator.Core
{
    public abstract class InstallerGeneratorRunbook : IInstallerGeneratorRunbook
    {
        public string WorkingPath { get; set; }
        public string RecipePath { get; set; }
        public string SourcePath { get; set; }
        public string ArtifactPath { get; set; }
        public string ArtifactName { get; set; }
        public string Version { get; set; }
    
        public IInstallerGeneratorStep[] Steps { get; protected set; }

        protected InstallerGeneratorRunbook()
        {
            WorkingPath = Path.GetFullPath(Path.Combine("temp", Guid.NewGuid().ToString()));
            ArtifactPath = Path.GetFullPath(@"artifact");
        }

        public abstract void BakeSteps();
    }
}