using System;

namespace Thycotic.SecretServerEngine.Web.Common.Response
{
    [Obsolete("Needs to live on SS side as nuget")]
    public class EngineConfigurationResponse : ResponseBase
    {
        public byte[] Configuration { get; set; }
        public bool UpgradeNeeded { get; set; }
    }
}