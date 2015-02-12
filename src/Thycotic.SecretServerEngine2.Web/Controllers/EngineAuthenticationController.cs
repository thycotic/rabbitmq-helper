using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;
using Thycotic.SecretServerEngine2.Web.Models;
using Thycotic.TempAppCore;

namespace Thycotic.SecretServerEngine2.Web.Controllers
{
    [RoutePrefix("api/EngineAuthentication")]
    public class EngineAuthenticationController : ApiController
    {
        
        public static void CreatePublicAndPrivateKeys(out PublicKey publicKey, out PrivateKey privateKey)
        {
            const int RsaSecurityKeySize = 2048;
            const CspProviderFlags flags = CspProviderFlags.UseMachineKeyStore;
            var cspParameters = new CspParameters { Flags = flags };

            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider(RsaSecurityKeySize, cspParameters))
            {
                privateKey = new PrivateKey(provider.ExportCspBlob(true));
                publicKey = new PublicKey(provider.ExportCspBlob(false));
            }
        }


        public static void CreateSymmetricKeyAndIv(out SymmetricKey symmetricKey, out InitializationVector initializationVector)
        {
            int AesKeySize = 256;
            int IvSize = 128;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = IvSize;
                aes.KeySize = AesKeySize;
                aes.GenerateIV();
                aes.GenerateKey();
                symmetricKey = new SymmetricKey(aes.Key);
                initializationVector = new InitializationVector(aes.IV);
            }
        }

        public static EngineAuthenticationResult GetClientKey(string publicKey, string version)
        {
            const int SALT_LENGTH = 8;

            var saltProvider = new ByteSaltProvider();

            SymmetricKey symmetricKey;
            InitializationVector initializationVector;
            CreateSymmetricKeyAndIv(out symmetricKey, out initializationVector);
            //_openAgentConnectionProvider.AddClient(new Client
            //{
            //    PublicKey = publicKey,
            //    Name = name,
            //    SymmetricKey = symmetricKey.Value,
            //    InitalizationVector = initializationVector.Value
            //}, callback);
            var asymmetricEncryptor = new AsymmetricEncryptor();
            var saltedSymmetricKey = saltProvider.Salt(symmetricKey.Value, SALT_LENGTH);
            var encryptedSymmetricKey = asymmetricEncryptor.EncryptWithPublicKey(new PublicKey(Convert.FromBase64String(publicKey)), saltedSymmetricKey);
            var saltedInitializationVector = saltProvider.Salt(initializationVector.Value, SALT_LENGTH);
            var encryptedInitializationVector = asymmetricEncryptor.EncryptWithPublicKey(new PublicKey(Convert.FromBase64String(publicKey)), saltedInitializationVector);
            double versionNum;
            var canParse = double.TryParse(version, out versionNum);

            return new EngineAuthenticationResult
            {
                EncryptedSymmetricKey = encryptedSymmetricKey,
                EncryptedInitializationVector = encryptedInitializationVector
            };
        }

        [HttpPost]
        [Route("GetNewKey")]
        public Task<EngineAuthenticationRequest> GetNewKey()
        {
            PrivateKey privateKey;
            PublicKey publicKey;
            CreatePublicAndPrivateKeys(out publicKey, out privateKey);

            return Task.FromResult(new EngineAuthenticationRequest{ PublicKey = Convert.ToBase64String(publicKey.Value) });
        }

        [HttpPost]
        [Route("Authenticate")]
        public Task<EngineAuthenticationResult> Authenticate(EngineAuthenticationRequest request)
        {
            return Task.FromResult(GetClientKey(request.PublicKey, request.Version));
        }
    }
}
