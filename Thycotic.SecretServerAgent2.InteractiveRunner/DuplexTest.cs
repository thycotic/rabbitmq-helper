using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Thycotic.SecretServerAgent2.InteractiveRunner
{
    /*
    class Program2
    {
        static void Main(string[] args)
        {
            ServiceHost _host = null;

            Task.Factory.StartNew(() =>
            {
                var serviceBinding = new NetTcpBinding();//SecurityMode.TransportWithMessageCredential);
                //serviceBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
                //serviceBinding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;

                _host = new ServiceHost(typeof(MemoryMqService));
                _host.AddServiceEndpoint(typeof(IMemoryMqService), serviceBinding, GetBaseAddress());

                _host.Credentials.ServiceCertificate.SetCertificate(
                StoreLocation.LocalMachine,
                StoreName.My,
                X509FindType.FindByThumbprint,
                "f1faa2aa00f1350edefd9490e3fc95017db3c897");

                _host.Open();

            });


            var microwaveCallback = new MemoryMqCallback();

            var clientBinding = new NetTcpBinding();//SecurityMode.TransportWithMessageCredential);
            //clientBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            //clientBinding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;

            var channelFactory = new DuplexChannelFactory<IMemoryMqService>(microwaveCallback, clientBinding, GetBaseAddress());


            //channelFactory.Credentials.ClientCertificate.SetCertificate(
            //    StoreLocation.LocalMachine,
            //    StoreName.My,
            //    X509FindType.FindByThumbprint,
            //    "f1faa2aa00f1350edefd9490e3fc95017db3c897");

            IMemoryMqService memoryMqService = channelFactory.CreateChannel();

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

        private static string GetBaseAddress()
        {
            return "net.tcp://THYCOPAIR24.testparent.thycotic.com:8523/Calculator";

        }


    }


    // Service side operation
    [ServiceContract(Namespace = "http://remondo.net/services",
        CallbackContract = typeof(IMemoryMqCallback))]
    public interface IMemoryMqService
    {
        [OperationContract(IsOneWay = true)]
        void BasicPublish(string meal);

        [OperationContract(IsOneWay = true)]
        void BlockingPublish(string meal);
    }

    // Client side callback operation
    [ServiceContract(Namespace = "http://remondo.net/services")]
    public interface IMemoryMqCallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateStatus(string statusMessage);
    }

    public class MemoryMqService : IMemoryMqService
    {
        private IMemoryMqCallback _callbackChannel;
        private int _counter;
        private Timer _timer;

        #region IMicrowaveService Members

        public void BasicPublish(string meal)
        {
            _callbackChannel = OperationContext.Current
                .GetCallbackChannel<IMemoryMqCallback>();

            Console.WriteLine("Microwave Service");
            Console.WriteLine("Let's prepare us some {0}", meal);

            _counter = 999;
            _timer = new Timer(BasicAck, null, 500, 500);
        }

        public void BlockingPublish(string meal)
        {
            _callbackChannel = OperationContext.Current
                .GetCallbackChannel<IMemoryMqCallback>();

            Console.WriteLine("Microwave Service");
            Console.WriteLine("Let's prepare us some {0}", meal);

            _counter = 999;
            _timer = new Timer(BasicAck, null, 500, 500);
        }

        #endregion

        public void BasicAck(Object stateInfo)
        {
            if (_counter <= 0)
            {
                _callbackChannel.UpdateStatus("* Ping *");
                _callbackChannel.UpdateStatus("Bon appÃ©tit");
                _timer.Dispose();
            }
            else
            {
                _callbackChannel.UpdateStatus(_counter.ToString());
                _counter--;
            }
        }
    }

    public class MemoryMqCallback : IMemoryMqCallback
    {
        #region IMicrowaveClientManager Members

        public void UpdateStatus(string statusMessage)
        {
            Console.WriteLine(statusMessage);
        }

        #endregion
    }
     * */

}
