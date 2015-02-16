using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thycotic.SecretServerEngine2.Configuration
{
    /// <summary>
    /// Possible configuration endpoints
    /// </summary>
    public static class EndPoints
    {
        /// <summary>
        /// The get configuration
        /// </summary>
        public const string GetConfiguration = "api/EngineAuthentication/GetConfiguration";

        /// <summary>
        /// The authenticate
        /// </summary>
        public const string Authenticate = "api/EngineAuthentication/Authenticate";
    }
}
