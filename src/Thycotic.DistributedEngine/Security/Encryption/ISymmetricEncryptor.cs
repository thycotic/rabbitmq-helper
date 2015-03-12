using System.IO;

namespace Thycotic.DistributedEngine.Security.Encryption
{
    interface ISymmetricEncryptor
    {
        byte[] Encrypt(byte[] bytes, SymmetricKey key, InitializationVector iv);

        byte[] Decrypt(byte[] bytes, SymmetricKey key, InitializationVector iv);

        EncryptedPrivateKey EncryptWithPassword(string password, InitializationVector iv, PrivateKey data);

        PrivateKey DecryptWithPassword(string password, InitializationVector iv, EncryptedPrivateKey data);

        void Encrypt(Stream inputStream, Stream outputStream, SymmetricKey key, InitializationVector iv);

        void Decrypt(Stream inputStream, Stream outputStream, SymmetricKey key, InitializationVector iv);
    }
}
