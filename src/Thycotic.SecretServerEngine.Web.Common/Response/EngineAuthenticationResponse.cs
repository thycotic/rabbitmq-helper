namespace Thycotic.SecretServerEngine.Web.Common.Response
{
    public class EngineAuthenticationResponse : ResponseBase
    {
        public byte[] SymmetricKey { get; set; }
        public byte[] InitializationVector { get; set; }
        public bool UpgradeNeeded { get; set; }
     
    }
}