using System;

namespace Thycotic.Utility.Testing.TestChain
{
    /// <summary>
    ///     Attribute used to mark classes as requiring unit tests. Any type this attribute is applied to has to have unit
    ///     tests or the build will break.
    ///     The attribute is not inheritable
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class UnitTestsRequiredAttribute : Attribute
    {
    }
}