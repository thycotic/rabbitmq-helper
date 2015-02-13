using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;
using Thycotic.TempAppCore;
using Thycotic.TempAppCore.Engine;

namespace Thycotic.SecretServerEngine2.Web.Controllers
{
    [RoutePrefix("api/EngineAuthentication")]
    public class EngineAuthenticationController : ApiController
    {
        public static void CreateSymmetricKeyAndIv(out SymmetricKey symmetricKey, out InitializationVector initializationVector)
        {
            int AesKeySize = 256;
            int IvSize = 128;

            using (var aes = new AesCryptoServiceProvider())
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
            //var asymmetricEncryptor = new AsymmetricEncryptor();
            //var saltedSymmetricKey = saltProvider.Salt(symmetricKey.Value, SALT_LENGTH);
            //var encryptedSymmetricKey = asymmetricEncryptor.EncryptWithPublicKey(new PublicKey(Convert.FromBase64String(publicKey)), saltedSymmetricKey);
            //var saltedInitializationVector = saltProvider.Salt(initializationVector.Value, SALT_LENGTH);
            //var encryptedInitializationVector = asymmetricEncryptor.EncryptWithPublicKey(new PublicKey(Convert.FromBase64String(publicKey)), saltedInitializationVector);
            double versionNum;
            var canParse = double.TryParse(version, out versionNum);

            return new EngineAuthenticationResult
            {
                //SymmetricKey = encryptedSymmetricKey,
                SymmetricKey = symmetricKey.Value,
                //InitializationVector = encryptedInitializationVector
                InitializationVector = initializationVector.Value
            };
        }

        private static ConcurrentDictionary<string, EngineAuthenticationResult> _approvedRequests = new ConcurrentDictionary<string, EngineAuthenticationResult>();

        [HttpPost]
        [Route("Authenticate")]
        public Task<EngineAuthenticationResult> Authenticate(EngineAuthenticationRequest request)
        {
            //TODO: Validate client

            var result = _approvedRequests.GetOrAdd(request.ExchangeName, key => GetClientKey(request.PublicKey, request.Version));

            return Task.FromResult(result);
        }
    }
}
