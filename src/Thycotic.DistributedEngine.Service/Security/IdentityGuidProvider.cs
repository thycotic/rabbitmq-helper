using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Thycotic.Logging;
using Thycotic.Utility.Reflection;

namespace Thycotic.DistributedEngine.Service.Security
{
    /// <summary>
    /// Identifier provider
    /// </summary>
    public class IdentityGuidProvider : IIdentityGuidProvider
    {
        private const string DataDirectoryName = "data";

        private readonly Lazy<Guid> _identityGuid;
        private readonly AssemblyEntryPointProvider _assemblyEntryPointProvider = new AssemblyEntryPointProvider();

        private readonly ILogWriter _log = Log.Get(typeof(IdentityGuidProvider));
        
        /// <summary>
        /// Gets the identity unique identifier.
        /// </summary>
        /// <value>
        /// The identity unique identifier.
        /// </value>
        public Guid IdentityGuid
        {
            get { return _identityGuid.Value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityGuidProvider"/> class.
        /// </summary>
        public IdentityGuidProvider()
        {
            _identityGuid = new Lazy<Guid>(RestoreIdentityGuid);
        }

        private string GetPersistPath()
        {
            var path = Path.Combine(_assemblyEntryPointProvider.GetAssemblyDirectory(GetType()), DataDirectoryName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return Path.Combine(path, "identityGuid.json");
        }

        private byte[] Protect(Guid identityGuid)
        {
            try
            {
                _log.Debug(string.Format("Protecting identifier as user {0} (Domain: {1})", Environment.UserName, Environment.UserDomainName));

                var unencryptedBytes = Encoding.UTF8.GetBytes(identityGuid.ToString());

                // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted 
                //  only by the same current user. 
                return ProtectedData.Protect(unencryptedBytes, null, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException ex)
            {
                throw new ApplicationException("Could not encrypt bytes", ex);
            }
        }

        private Guid Unprotect(byte[] encryptedBytes)
        {
            try
            {
                _log.Debug(string.Format("Un-protecting identifier as user {0} (Domain: {1})", Environment.UserName, Environment.UserDomainName));

                //Decrypt the data using DataProtectionScope.CurrentUser. 
                var unencryptedBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);

                return new Guid(Encoding.UTF8.GetString(unencryptedBytes));


            }
            catch (CryptographicException ex)
            {
                throw new ApplicationException("Could not un-encrypt bytes", ex);
            }
        }

        private Guid RestoreIdentityGuid()
        {
            var path = GetPersistPath();
            if (!File.Exists(path))
            {
                _log.Warn("No existing identity found. Generating new");

                var newIdentityGuid = Guid.NewGuid();

                PersistIdentityGuid(newIdentityGuid);

                return newIdentityGuid;
            }

            _log.Info("Restoring identity from disk...");

            string guidString;

            using (var fs = File.Open(path, FileMode.Open))
            using (var fr = new StreamReader(fs))
            {
                guidString = fr.ReadToEnd();
            }

            try
            {
                var encryptedBytes = Convert.FromBase64String(guidString);

                return Unprotect(encryptedBytes);

            }
            catch (Exception ex)
            {
                _log.Warn("Could not restore identity. Generating new", ex);

                var newIdentityGuid = Guid.NewGuid();

                PersistIdentityGuid(newIdentityGuid);

                return newIdentityGuid;
            }
        }

        private void PersistIdentityGuid(Guid identityGuid)
        {

            var path = GetPersistPath();
            if (File.Exists(path))
            {
                //delete any previous snapshots
                File.Delete(path);
            }

            var encryptedBytes = Protect(identityGuid);

            using (var fs = File.Open(path, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {

                sw.Write(Convert.ToBase64String(encryptedBytes));
            }
        }
    }
}
