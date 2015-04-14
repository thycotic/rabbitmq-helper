using System;
using System.IO;

namespace Thycotic.InstallerGenerator.Core
{
    public abstract class InstallerGeneratorSteps : IInstallerGeneratorSteps
    {
        public string WorkingPath { get; set; }
        public string RecipePath { get; set; }
        public string SourcePath { get; set; }
        public string ArtifactPath { get; set; }
        public string ArtifactName { get; set; }

        protected InstallerGeneratorSteps()
        {
            WorkingPath = Path.Combine("temp", Guid.NewGuid().ToString());
            ArtifactPath = @"artifact";

        }
    }
}