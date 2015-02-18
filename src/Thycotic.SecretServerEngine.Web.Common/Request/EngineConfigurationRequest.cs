using System;

namespace Thycotic.SecretServerEngine.Web.Common.Request
{
    [Obsolete("Needs to live on SS side as nuget")]
    public class EngineConfigurationRequest
    {
        public string FriendlyName { get; set; }
        public Guid IdentityGuid { get; set; }
        public string PublicKey { get; set; }
        public double Version { get; set; }
        
    }
}