using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Thycotic.SecretServerEngine.CertificateHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load your certificate from file
            string input;
            if (args != null && args.Any())
            {
                input = args.First();
            }
            else
            {
                Console.WriteLine("Please enter the full path to the file");
                input = Console.ReadLine();
            }
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(input.Trim()) || !input.Contains("\\") || !input.EndsWith(".pfx"))
            {
                Console.Clear();
                Console.WriteLine("Invalid File Path");
                Console.WriteLine("Press any key to start over");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            var pathList = input.Split('\\').ToList();
            var originalFileName = pathList.Last();
            if (!originalFileName.EndsWith(".pfx"))
            {
                Console.Clear();
                Console.WriteLine("File must be a .pfx!");
                Console.WriteLine("Press any key to start over");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            var keyFileName = string.Join(".", originalFileName.Split('\\').First(), "key");
            var pemFileName = string.Join(".", originalFileName.Split('\\').First(), "pem");
            pathList.RemoveAt(pathList.IndexOf(pathList.Last()));

            var filePath = string.Join("\\", pathList);
            if (!Directory.Exists(filePath))
            {
                Console.Clear();
                Console.WriteLine("Invalid Directory!");
                Console.WriteLine("Press any key to start over");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            if (!File.Exists(string.Join("\\", filePath, originalFileName)))
            {
                Console.Clear();
                Console.WriteLine("Invalid File!");
                Console.WriteLine("Press any key to start over");
                Console.ReadKey();
                Console.Clear();
                return;
            }
            var pfx = new X509Certificate2(string.Join("\\", filePath, originalFileName), "test", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

            // Now you have your private key in binary form as you wanted
            // You can use rsa.ExportParameters() or rsa.ExportCspBlob() to get you bytes
            // depending on format you need them in
            var rsa = (RSACryptoServiceProvider)pfx.PrivateKey;

            // Just for lulz, let's write out the PEM representation of the private key
            // using Bouncy Castle, so that we are 100% sure that the result is exaclty the same as:
            // openssl pkcs12 -in filename.pfx -nocerts -out privateKey.pem
            // openssl.exe rsa -in privateKey.pem -out private.pem

            // You should of course dispose of / close the streams properly. I'm skipping this part for brevity
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var pemWriter = new PemWriter(streamWriter);
                    var keyPair = DotNetUtilities.GetRsaKeyPair(rsa);
                    pemWriter.WriteObject(keyPair.Private);
                    streamWriter.Flush();
                    File.WriteAllBytes(string.Join("\\", filePath, keyFileName), memoryStream.GetBuffer());
                }
            }

            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var pemWriter = new PemWriter(streamWriter);
                    pemWriter.WriteObject(DotNetUtilities.FromX509Certificate(pfx));
                    streamWriter.Flush();
                    File.WriteAllBytes(string.Join("\\", filePath, pemFileName), memoryStream.GetBuffer());
                }
            }
        }
    }
}
