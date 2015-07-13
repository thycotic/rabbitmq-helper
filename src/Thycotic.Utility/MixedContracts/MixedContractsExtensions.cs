using System;
using System.Diagnostics.Contracts;

namespace Thycotic.Utility.MixedContracts
{

    /// <summary>
    /// Mixed contracts exception
    /// </summary>
    public class MixedContractsException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MixedContractsException"/> class.
        /// </summary>
        public MixedContractsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MixedContractsException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public MixedContractsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MixedContractsException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public MixedContractsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Mixed contract extensions for code that does not have its own contracts
    /// </summary>
    public static class MixedContractsExtensions
    {
        /// <summary>
        /// Ensures the specified target is not null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="target">The target.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static T EnsureNotNull<T>(this object obj, T target, string message = "")
        {
            Contract.Ensures(Contract.Result<T>() != null);

            if (target == null)
            {
                throw new MixedContractsException(message);
            }

            Contract.Assume(target != null);

            return target;
        }


        /// <summary>
        /// Ensures the specified lhs is greater than or equal to the rhs
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException"></exception>
        public static int EnsureGreaterThanOrEqualTo(this object obj, int lhs, int rhs, string message = "")
        {
            Contract.Ensures(Contract.Result<int>() >= rhs);

            if (!(lhs >= rhs))
            {
                throw new MixedContractsException(message);
            }

            Contract.Assume(lhs >= rhs);

            return lhs; 
        }
    }
}
