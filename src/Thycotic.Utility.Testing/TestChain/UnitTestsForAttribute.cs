using System;

namespace Thycotic.Utility.Testing.TestChain
{
    /// <summary>
    ///     Attribute used to mark classes as containing tests for a class.
    ///     The attribute is not inheritable
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class UnitTestsForAttribute : Attribute
    {
        /// <summary>
        ///     The type under test
        /// </summary>
        public readonly Type Type;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnitTestsForAttribute" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public UnitTestsForAttribute(Type type)
        {
            Type = type;
        }
    }
}