namespace Thycotic.SecretServerEngine2.Web.Models
{
    public class EngineAuthenticationRequest
    {
        public string ExchangeName { get; set; }
        public string PublicKey { get; set; }
        public string Version { get; set; }
    }
}