namespace Thycotic.SecretServerEngine2.Web.Common.Response
{
    public class EngineAuthenticationResponse
    {
        public byte[] SymmetricKey { get; set; }
        public byte[] InitializationVector { get; set; }
    }
}