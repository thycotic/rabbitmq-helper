using System.IdentityModel.Selectors;
using Thycotic.Logging;

namespace Thycotic.MemoryMq.Pipeline.Service.Security
{
    /// <summary>
    /// Custom username and password validator
    /// </summary>
    public class CustomUserNamePasswordValidator : UserNamePasswordValidator
    {
        private readonly ILogWriter _log = Log.Get(typeof(CustomUserNamePasswordValidator));

        /// <summary>
        /// When overridden in a derived class, validates the specified username and password.
        /// </summary>
        /// <param name="userName">The username to validate.</param>
        /// <param name="password">The password to validate.</param>
        public override void Validate(string userName, string password)
        {
            _log.Warn(string.Format("Accepting a pipeline client {0} without actually checking the credentials supplied", userName));
        }
    }
}
