namespace Thycotic.SecretServerEngine2.Web.Common.Request
{
    public class EngineAuthenticationRequest
    {
        public string ExchangeName { get; set; }
        public string PublicKey { get; set; }
        public double Version { get; set; }
    }
}