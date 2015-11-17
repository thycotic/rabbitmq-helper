using System;

namespace Thycotic.Messages.Areas.ActiveDirectory.Response
{
    /// <summary>
    /// Response for Domain distinguished name resolution.
    /// </summary>
    public class DomainDistinguishedNameResponse
    {
        /// <summary>
        /// Domain distinguished name
        /// </summary>
        public string DomainDistinguishedName { get; set; }

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
                return !String.IsNullOrEmpty(this.ErrorMessage);
            }
        }
    }
}