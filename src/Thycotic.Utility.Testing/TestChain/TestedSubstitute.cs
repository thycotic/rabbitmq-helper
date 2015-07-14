using System;
using NSubstitute;

namespace Thycotic.Utility.Testing.TestChain
{
    public static class TestedSubstitute
    {

        /// <summary>
        /// Generates substitute for a class or interface which requires tests.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public static T For<T>()
            where T : class
        {
            var type = typeof (T);

            if (!type.IsDefined(typeof(UnitTestsRequiredAttribute), true))
            {
                throw new ApplicationException(
                    string.Format(
                        "The class or interface {0} you are trying to substitute for does not require that tests exist for it.",
                        type.FullName));
            }

            return Substitute.For<T>();
        }
    }
}