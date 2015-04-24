using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;

namespace Thycotic.WindowsService.Bootstraper
{
    //TODO: Improve checking for service state
    //TODO: Hook into engine

    public static class ServiceStates
    {
        public const string Stopped = "Stopped";
        public const string Running = "Running";
    }

    public class Program
    {
        private static ManagementObject GetManagementObject(ManagementPath computerPath)
        {
            var path = computerPath;
            var managementObject = new ManagementObject(path);
            return managementObject;
        }

        private static IWin32Service GetService(string serviceName)
        {
            var computerPath = Win32Service.GetLocalServiceManagementPath(serviceName);
            var managementObject = GetManagementObject(new ManagementPath(computerPath));

            //Task.Delay(TimeSpan.FromSeconds(5)).Wait();

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

            Log.Configure();

            var log = Log.Get(typeof(Program));

            var cts = new CancellationTokenSource();

            using (LogCorrelation.Create())
            {

                try
                {
                    var workingPath = Directory.GetCurrentDirectory();

                    var serviceName = args[0];

                    if (string.IsNullOrEmpty(serviceName))
                    {
                        throw new ApplicationException("Service name is required");
                    }

                    var msiPath = args[1];

                    if (!File.Exists(msiPath))
                    {
                        throw new FileNotFoundException(string.Format("MSI does not exist at {0}", msiPath));
                    }

                    log.Info(string.Format("Running bootstrap process for {0} with {1}", serviceName, msiPath));
                    
                    InteractiveWithService(serviceName, service =>
                    {
                        log.Info("Stopping service");
                        service.StopService();
                    });

                    while (!cts.Token.IsCancellationRequested)
                    {
                        if (GetServiceState(serviceName) == ServiceStates.Stopped)
                        {
                            log.Info("Service stopped");
                            break;
                        }

                        Task.Delay(TimeSpan.FromSeconds(5), cts.Token).Wait(cts.Token);
                    }

                    CleanDirectory(workingPath);
                    //recreate the log path that was just cleaned up
                    CreateDirectory(Path.Combine(workingPath, "log"));

                    var processInfo = new ProcessStartInfo("msiexec")
                    {
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        WorkingDirectory = workingPath,
                        Arguments = string.Format(@"/i {0} /qn /log log\SSDEUpdate.log", msiPath)
                    };

                    log.Info(string.Format("Running MSI with arguments: {0}", processInfo.Arguments));

             
                    Process process = null;

                    var task = Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            process = Process.Start(processInfo);
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException("Could not start process", ex);
                        }


                        if (process == null)
                        {
                            throw new ApplicationException("Process could not start");
                        }

                        process.WaitForExit();

                    }, cts.Token);

                    //wait for 30 seconds for process to complete
                    task.Wait(TimeSpan.FromSeconds(30));

                    //there was an exception, rethrow it
                    if (task.Exception != null)
                    {
                        throw task.Exception;
                    }

                    if (process != null)
                    {
                        if (!process.HasExited)
                        {
                            log.Warn("Process has not exited. Forcing exit");
                            process.Kill();
                        }

                        //process didn't exit correctly, extract output and throw
                        if (process.ExitCode != 0)
                        {
                            var output = process.StandardOutput.ReadToEnd();

                            throw new ApplicationException("Process failed", new Exception(output));
                        }
                    }

                    log.Info("MSI finished");

                    //Configuration configuration = System.Configuration.ConfigurationManager.OpenExeConfiguration(Path.Combine(parentDirectory.ToString(), "SecretServerAgentService.exe"));
                    //configuration.AppSettings.Settings["RPCAgentVersion"].Value = args[0];
                    //configuration.Save();

                    InteractiveWithService(serviceName, service =>
                    {
                        log.Info("Starting service");
                        service.StartService();

                    });

                    while (!cts.Token.IsCancellationRequested)
                    {
                        if (GetServiceState(serviceName) == ServiceStates.Running)
                        {
                            log.Info("Service running");
                            break;
                        }

                        Task.Delay(TimeSpan.FromSeconds(5), cts.Token).Wait(cts.Token);
                    }

                }
                catch (Exception ex)
                {
                    log.Error("Failed to bootstrap", ex);
                }
            }
        }

        private static void CleanDirectory(string path)
        {
            var directoryInfo = new DirectoryInfo(path);

            directoryInfo.GetFiles().ToList().ForEach(f => f.Delete());

            directoryInfo.GetDirectories().ToList().ForEach(d => d.Delete(true));
        }

        private static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}
