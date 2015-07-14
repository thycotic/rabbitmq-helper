using System;

namespace Thycotic.Utility.Testing.BDD
{
    public static class NegativeTestExtensions
    {
        /// <summary>
        /// Tests for an expected failing condition. The lambda expression should throw an except with the specified type and exception
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="testFixture">The test fixture.</param>
        /// <param name="message">The message.</param>
        /// <param name="func">The function.</param>
        /// <exception cref="Exception">Exception was expected
        /// or
        /// or</exception>
        public static void ShouldFail<TException>(this ICustomTestFixture testFixture, string message, Func<object> func)
        {
            try
            {
                func.Invoke();

                throw new Exception("Exception was expected");
            }
            catch (Exception ex)
            {
                if (ex is TException)
                {
                    if (ex.Message != message)
                    {
                        throw new Exception(string.Format("Exception was cause but had the wrong message. Expected \"{0}\". Got \"{1}\"", message, ex.Message), ex);
                    }

                }
                else
                {
                    throw new Exception(string.Format("Exception is not of the correct type. Expected {0}. Got {1}", typeof(TException).FullName, ex.GetType().FullName), ex);
                }

            }
        }
    }
}