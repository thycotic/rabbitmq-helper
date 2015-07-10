using System;
using System.Diagnostics.Contracts;
using System.Management.Automation;

namespace Thycotic.DistributedEngine.Logic
{
    /// <summary>
    /// Mixed contract extensions for code that does not its own contracts
    /// </summary>
    //TODO: Move to Thycotic.Utility
    public static class MixedContractExtensions
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
                throw new ApplicationException(message);
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
                throw new ApplicationException(message);
            }

            Contract.Assume(lhs >= rhs);

            return lhs; 
        }
    }
}
