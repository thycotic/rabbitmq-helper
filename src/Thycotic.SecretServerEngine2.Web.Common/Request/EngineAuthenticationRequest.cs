using System;

namespace Thycotic.SecretServerEngine2.Web.Common.Request
{
    public class EngineAuthenticationRequest
    {
        public Guid IdentityGuid { get; set; }
        public string PublicKey { get; set; }
        public double Version { get; set; }

        public string ExchangeName { get; set; }
    }
}