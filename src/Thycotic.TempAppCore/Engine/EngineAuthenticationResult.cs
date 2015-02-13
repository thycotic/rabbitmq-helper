namespace Thycotic.TempAppCore.Engine
{
    public class EngineAuthenticationResult
    {
        public byte[] SymmetricKey { get; set; }
        public byte[] InitializationVector { get; set; }
    }
}