using System;

namespace Thycotic.Messages.Areas.ActiveDirectory.Response
{
    /// <summary>
    /// Response for Domain distinguished name resolution.
    /// </summary>
    public class FullyQualifiedDomainNameResponse
    {
        /// <summary>
        /// The Domain's fully qualified name.
        /// </summary>
        public string FullyQualifiedDomainName { get; set; }

        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// True, if the operation was successful. Otherwise, False.
        /// </summary>
        public bool Success
        {
            get
            {
                return String.IsNullOrEmpty(this.ErrorMessage);
            }
        }
    }
}