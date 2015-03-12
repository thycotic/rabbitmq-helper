namespace Thycotic.DistributedEngine.Security.Encryption
{
    interface IAsymmetricEncryptor
    {
        EncryptedSymmetricKey EncryptSymmetricKey(PublicKey publicKey, SymmetricKey key);

        SymmetricKey DecryptSymmetricKey(PrivateKey privateKey, EncryptedSymmetricKey encryptedSymmetricKey);

        byte[] EncryptWithPublicKey(PublicKey publicKey, byte[] data);

        byte[] DecryptWithPrivateKey(PrivateKey privateKey, byte[] data);

        byte[] EncryptWithKey(KeyBase key, byte[] data);

        byte[] DecryptWithKey(KeyBase key, byte[] data);
    }
}
