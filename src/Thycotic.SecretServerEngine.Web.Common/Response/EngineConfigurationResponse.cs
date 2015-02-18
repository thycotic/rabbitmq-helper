namespace Thycotic.SecretServerEngine2.Web.Common.Response
{
    public class EngineConfigurationResponse : ResponseBase
    {
        public byte[] Configuration { get; set; }
        public bool UpgradeNeeded { get; set; }
    }
}