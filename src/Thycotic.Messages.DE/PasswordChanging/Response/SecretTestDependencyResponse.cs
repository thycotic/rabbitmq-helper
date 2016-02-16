using System.Collections.Generic;

namespace Thycotic.Messages.PasswordChanging.Response
{
    /// <summary>
    /// Secret Test Dependency response
    /// </summary>
    public class SecretTestDependencyResponse
    {
        /// <summary>
        /// Gets or sets the Secret Id
        /// </summary>
        public int SecretId { get; set; }
        
        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public List<string[]> Messages { get; set; }
    }
}
