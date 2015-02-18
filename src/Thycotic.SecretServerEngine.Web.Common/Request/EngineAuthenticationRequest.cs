using System;

namespace Thycotic.SecretServerEngine.Web.Common.Request
{
    [Obsolete("Needs to live on SS side as nuget")]
    public class EngineAuthenticationRequest
    {
        public Guid IdentityGuid { get; set; }
        public string PublicKey { get; set; }
        public double Version { get; set; }

        public string ExchangeName { get; set; }
    }
}