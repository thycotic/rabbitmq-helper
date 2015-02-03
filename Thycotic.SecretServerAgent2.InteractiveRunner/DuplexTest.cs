using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thycotic.SecretServerAgent2.InteractiveRunner
{
    /*
    internal static class Program
    {
        static void Main(string[] args)
        {
            ServiceHost _host = null;

            Task.Factory.StartNew(() =>
            {
                var serviceBinding = new NetTcpBinding(SecurityMode.TransportWithMessageCredential);
                serviceBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
                serviceBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

                _host = new ServiceHost(typeof(MemoryMqService));
                _host.AddServiceEndpoint(typeof(IMemoryMqService), serviceBinding, GetBaseAddress());
                _host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new CustomUserNameValidator();
                //_host.Credentials.ServiceCertificate.Certificate = new X509Certificate2("mycertificate.p12", "password");
                _host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;

                _host.Credentials.ServiceCertificate.SetCertificate(
                StoreLocation.LocalMachine,
                StoreName.My,
                X509FindType.FindByThumbprint,
                //"f1faa2aa00f1350edefd9490e3fc95017db3c897"
                "1ec85a6084862addedb77c4a777c86747f488c90");

                _host.Open();

            });


            var microwaveCallback = new MemoryMqCallback();

            var clientBinding = new NetTcpBinding(SecurityMode.TransportWithMessageCredential);
            clientBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            clientBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

            var channelFactory = new DuplexChannelFactory<IMemoryMqService>(microwaveCallback, clientBinding, GetBaseAddress());
            channelFactory.Credentials.UserName.UserName = Guid.NewGuid().ToString();
            channelFactory.Credentials.UserName.Password = Guid.NewGuid().ToString();

            //channelFactory.Credentials.ClientCertificate.SetCertificate(
            //    StoreLocation.LocalMachine,
            //    StoreName.My,
            //    X509FindType.FindByThumbprint,
            //    //"f1faa2aa00f1350edefd9490e3fc95017db3c897"
            //    "1ec85a6084862addedb77c4a777c86747f488c90");

            var memoryMqService = channelFactory.CreateChannel();

            Console.WriteLine("Throw some Hot Pockets in the Microwave");

            memoryMqService.BasicPublish("Hot Pockets");

            Console.WriteLine("Waiting for the food...");
            Console.ReadLine();

            ((IClientChannel)memoryMqService).Close();

            if (_host != null)
            {
                _host.Close();
            }
        }



    }
     * */

   
}
