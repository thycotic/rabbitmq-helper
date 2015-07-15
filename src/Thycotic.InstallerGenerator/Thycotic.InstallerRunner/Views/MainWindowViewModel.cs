using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;
using Thycotic.Logging.LogTail;
using Thycotic.Utility.OS;
using Thycotic.Utility.Reflection;

namespace Thycotic.InstallerRunner.Views
{
    public class MainWindowViewModel : INotifyPropertyChanged, IViewModel, IDisposable
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly IRecentLogEntryProvider _recentLogEntryProvider = new RecentLogEntryProvider();
        private readonly StringBuilder _logOutput = new StringBuilder();

        private readonly ILogWriter _log = Log.Get(typeof(MainWindowViewModel));
        private bool _showLog;
        private string _latestStatus;
        private Task _monitorTask;
        private Task _installationTask;
        private bool _isInstallationSuccessful;


        public event PropertyChangedEventHandler PropertyChanged;

        public bool ShowLog
        {
            get
            {
                return _showLog;
            }
            set
            {
                if (_showLog == value)
                {
                    return;
                }

                _showLog = value;
                OnPropertyChanged("ShowLog");
            }
        }



        public string LatestStatus
        {
            get
            {
                return _latestStatus;
            }
            set
            {
                if (_latestStatus == value)
                {
                    return;
                }

                _latestStatus = value;
                OnPropertyChanged("LatestStatus");
            }
        }

        public bool IsInstalling
        {
            get { return _installationTask != null && !_installationTask.IsCompleted; }
        }

        public bool IsInstallationSuccessful
        {
            get
            {
                return _isInstallationSuccessful;
            }
            set
            {
                if (_isInstallationSuccessful == value)
                {
                    return;
                }

                _isInstallationSuccessful = value;
                OnPropertyChanged("IsInstallationSuccessful");
            }
        }

        public string LogOutput
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return _logOutput.ToString();
            }
        }

        public void Initialize()
        {
            LatestStatus = "Initializing...";
        }

        public Task Install()
        {

            _monitorTask = Task.Factory.StartNew(MonitorLog, _cts.Token);
            _installationTask =
                Task.Factory.StartNew(RunMsi, _cts.Token).ContinueWith(task =>
                {
                    //done installing, notify UI
                    OnPropertyChanged("IsInstalling");
                });

            //installation starting
            OnPropertyChanged("IsInstalling");

            return _installationTask;
        }

        private void MonitorLog()
        {
            Task.Factory
                .StartNew(() =>
                {
                    lock (_recentLogEntryProvider)
                    {
                        var entries = _recentLogEntryProvider.GetEntries().Select(le => le.Message).ToList();

                        if (!entries.Any())
                        {
                            return;
                        }

                        entries.ForEach(s => _logOutput.AppendLine(s));
                        _recentLogEntryProvider.Clear();

                        OnPropertyChanged("LogOutput");
                    }
                }, _cts.Token)
                .ContinueWith(task => Task.Delay(TimeSpan.FromSeconds(5)).Wait(), _cts.Token)
                .ContinueWith(task => MonitorLog(), _cts.Token);
        }

        private void RunMsi()
        {
            var assemblyEntrypointProvider = new AssemblyEntryPointProvider();

            var workingPath = assemblyEntrypointProvider.GetAssemblyDirectory(GetType());

       

            var appSettings = ConfigurationManager.AppSettings;

            var msiFileName = appSettings[ConfigurationKeys.MsiFileName];

            var msiPath = Path.Combine(workingPath, msiFileName);

            if (!File.Exists(msiPath))
            {
                throw new FileNotFoundException(msiPath);
            }

            var fileInfo = new FileInfo(msiPath);

            LatestStatus = string.Format("Please wait while we install {0}", fileInfo.Name);

            var parameters = new Dictionary<string, string>();

            _log.Info("Extracting MSI parameters");

            //copy all parameters in the app.config to local dictionary
            appSettings.AllKeys.Where(k => k != ConfigurationKeys.MsiFileName)
                .ToList()
                .ForEach(k =>
                {
                    _log.Info(string.Format("{0} = {1}", k, appSettings[k]));

                    parameters.Add(k, appSettings[k]);
                });


            var processRunner = new ExternalProcessRunner();


            _log.Info(string.Format("Log file will be saved in {0}", Path.Combine(workingPath, "log")));

            var coreMsiParameters = string.Format(@"/i ""{0}"" /log log\install.log", msiPath);
            var serviceParameters = string.Join(" ",
                parameters.Select(p => string.Format(@"{0}=""{1}""", p.Key, p.Value)));

            var processInfo = new ProcessStartInfo("msiexec")
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = workingPath,
                Arguments = string.Join(" ", coreMsiParameters, serviceParameters)
            };

            _log.Info("Installing MSI");

            try
            {
                processRunner.EstimatedProcessDuration = TimeSpan.FromMinutes(10);
                processRunner.Run(processInfo);

                _log.Info("Installation completed");

                LatestStatus = "Installation completed";

                IsInstallationSuccessful = true;

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex.InnerException);

                LatestStatus = "Installation failed";

                IsInstallationSuccessful = false;
            }

            
            //give the user three seconds to digest what happened
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();

        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Dispose()
        {
            _cts.Cancel();

            _monitorTask.Wait(TimeSpan.FromSeconds(30));
            _installationTask.Wait(TimeSpan.FromSeconds(30));
        }
    }
}
