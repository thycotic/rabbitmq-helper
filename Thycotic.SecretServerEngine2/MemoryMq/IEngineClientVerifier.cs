namespace Thycotic.SecretServerEngine2.MemoryMq
{
    public interface IEngineClientVerifier
    {
        void Validate(string agentkey, string ignored);
    }
}