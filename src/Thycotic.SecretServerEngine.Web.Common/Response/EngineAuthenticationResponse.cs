using System;

namespace Thycotic.SecretServerEngine.Web.Common.Response
{
    [Obsolete("Needs to live on SS side as nuget")]
    public class EngineAuthenticationResponse : ResponseBase
    {
        public byte[] SymmetricKey { get; set; }
        public byte[] InitializationVector { get; set; }
        public bool UpgradeNeeded { get; set; }
     
    }
}