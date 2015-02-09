using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Thycotic.SecretServerEngine2.LogViewer.Models;

namespace Thycotic.SecretServerEngine2.LogViewer.Views
{
    public class MainWindowViewModel : INotifyPropertyChanged, IViewModel
    {
        private LogDataProvider _dataProvider;

        private readonly LogLevelViewModel[] _logLevels =
        {
            new LogLevelViewModel
            {
                Name = "All",
                Value = "*"
            },
            new LogLevelViewModel
            {
                Name = "Information",
                Value = "INFO"
            },
            new LogLevelViewModel
            {
                Name = "Warning",
                Value = "WARN"
            },
            new LogLevelViewModel
            {
                Name = "Error",
                Value = "ERROR"
            }
        };

        public LogLevelViewModel[] LogLevels
        {
            get { return _logLevels; }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private LogLevelViewModel _logLevel;
        private bool _autoRefresh;
        private int _refreshIntervalSeconds;

        private readonly RoutedEventHandler _onLogLevelChange;
        private readonly RoutedEventHandler _onAutoRefreshChange;
        private readonly RoutedEventHandler _onRefreshIntervalChange;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public ObservableCollection<LogEntry> LogEntries { get; private set; }
        public LogEntry SelectedCorrelation { get; private set; }

        public ObservableCollection<LogEntry> LogItemsInCorrelation { get; private set; }
        public LogEntry SelectedLogEntry { get; private set; }
        
        public LogLevelViewModel LogLevel
        {
            get { return _logLevel; }
            set
            {
                if (value == _logLevel)
                {
                    return;
                }

                _logLevel = value;
                OnPropertyChanged("LogLevel");
                _onLogLevelChange(this, new RoutedEventArgs());
            }
        }

        public bool IsAutoRefresh
        {
            get { return _autoRefresh; }
            set
            {
                if (value == _autoRefresh)
                {
                    return;
                }

                _autoRefresh = value;
                OnPropertyChanged("IsAutoRefresh");
                _onAutoRefreshChange(this, new RoutedEventArgs());
            }
        }

        public int RefreshIntervalSeconds
        {
            get { return _refreshIntervalSeconds; }
            set
            {
                if (value == _refreshIntervalSeconds)
                {
                    return;
                }

                //refresh at least every 5 seconds
                if (value < 15)
                {
                    return;
                }

                _refreshIntervalSeconds = value;
                OnPropertyChanged("RefreshIntervalSeconds");
                _onRefreshIntervalChange(this, new RoutedEventArgs());
            }
        }

       
        public MainWindowViewModel()
        {
            _onLogLevelChange += (sender, args) => ReactToAutoRefresh();
            _onAutoRefreshChange += (sender, args) => ReactToAutoRefresh();
            _onRefreshIntervalChange += (sender, args) => ReactToAutoRefresh();

        }

        public void Initialize(LogDataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            _logLevel = _logLevels[0];
            _autoRefresh = true;
            _refreshIntervalSeconds = 15;
            LogEntries = new ObservableCollection<LogEntry>();

            Task.Factory.StartNew(ReactToAutoRefresh);

        }

        private void ReactToAutoRefresh()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Token.WaitHandle.WaitOne();
            }

            if (!_autoRefresh) return;

            _cts = new CancellationTokenSource();

            Task.Factory.StartNew(GetLatestLogEntries, _cts.Token)
                .ContinueWith(t => Thread.Sleep(TimeSpan.FromSeconds(_refreshIntervalSeconds)), _cts.Token)
                .ContinueWith(t => ReactToAutoRefresh(), _cts.Token);
        }

        private void GetLatestLogEntries()
        {
            var results = _dataProvider.GetLatestCorrelations(_logLevel.Value).ToList();

            //clear the existing log
            this.InvokeOnUiThread(() => LogEntries.Clear());

            this.InvokeOnUiThread(() => results.ForEach(le => LogEntries.Add(le)));

        }


        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}