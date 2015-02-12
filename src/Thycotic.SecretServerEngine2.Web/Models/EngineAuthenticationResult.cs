namespace Thycotic.SecretServerEngine2.Web.Models
{
    public class EngineAuthenticationResult
    {
        public byte[] EncryptedSymmetricKey { get; set; }
        public byte[] EncryptedInitializationVector { get; set; }
    }
}