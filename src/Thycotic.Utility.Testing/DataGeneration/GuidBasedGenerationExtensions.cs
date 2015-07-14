using System;

namespace Thycotic.Utility.Testing.DataGeneration
{
    public static class GuidBasedGenerationExtensions
    {
        /// <summary>
        /// Generates a unique dummy name based on a GUID
        /// </summary>
        /// <param name="testFixture">The test fixture.</param>
        /// <returns></returns>
        public static string GenerateUniqueDummyName(this ICustomTestFixture testFixture)
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}