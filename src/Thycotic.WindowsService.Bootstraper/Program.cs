using System;
using System.IO;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace Thycotic.WindowsService.Bootstraper
{
    public class Program
    {
        static ManagementObject GetManagementObject(ManagementPath computerPath)
        {
            var path = computerPath;
            var managementObject = new ManagementObject(path);
            return managementObject;
        }

        private static IWin32Service GetService(string serviceName)
        {
            var computerPath = Win32Service.GetLocalServiceManagementPath(serviceName);
            var managementObject = GetManagementObject(new ManagementPath(computerPath));

            Task.Delay(TimeSpan.FromSeconds(5)).Wait();

            managementObject.Scope.Connect();

            return new Win32Service(managementObject);

        }

        private static void InteractiveWithService(string serviceName, Action<IWin32Service> action)
        {
            try
            {
                using (var win32Service = GetService(serviceName))
                {
                    action.Invoke(win32Service);
                }

            }
            catch (Exception ex)
            {
                //TODO: Log?
                throw ex;
            }
        }

        private static string GetServiceState(string serviceName)
        {
            try
            {
                using (var win32Service = GetService(serviceName))
                {
                    return win32Service.State;
                }

            }
            catch (Exception ex)
            {
                //TODO: Log?
                throw ex;
            }
        }

        static void Main(string[] args)
        {
            const string serviceName = "Thycotic.DistributedEngine.Service";

            InteractiveWithService(serviceName, service =>
            {
                service.StopService();
            });

            while (GetServiceState(serviceName) != "Stopped")
            {
                Task.Delay(TimeSpan.FromSeconds(5)).Wait();
            }

            Console.WriteLine("Service stopped");


            //Directory.CreateDirectory("dummyaction");
            //File.Create(Path.Combine("dummyaction", Guid.NewGuid().ToString()));
            //Task.Delay(TimeSpan.FromSeconds(15)).Wait();

            //Configuration configuration = System.Configuration.ConfigurationManager.OpenExeConfiguration(Path.Combine(parentDirectory.ToString(), "SecretServerAgentService.exe"));
            //configuration.AppSettings.Settings["RPCAgentVersion"].Value = args[0];
            //configuration.Save();

            InteractiveWithService(serviceName, service =>
            {
                service.StartService();

            });

            while (GetServiceState(serviceName) != "Running")
            {
                Task.Delay(TimeSpan.FromSeconds(5)).Wait();
            }

            Console.WriteLine("Service started");
        }
    }
}
