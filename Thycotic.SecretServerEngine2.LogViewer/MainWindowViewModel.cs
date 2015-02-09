using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Thycotic.SecretServerEngine2.LogViewer
{
    public class MainWindowViewModel : INotifyPropertyChanged, IViewModel
    {
        private readonly LogLevel[] _logLevels =
        {
            new LogLevel
            {
                Name = "All",
                Value = "*"
            },
            new LogLevel
            {
                Name = "Information",
                Value = "INFO"
            },
            new LogLevel
            {
                Name = "Warning",
                Value = "WARN"
            },
            new LogLevel
            {
                Name = "Error",
                Value = "ERROR"
            }
        };

        public LogLevel[] LogLevels
        {
            get { return _logLevels; }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private LogLevel _logLevel;
        private bool _autoRefresh;
        private int _refreshIntervalSeconds;
       
        private readonly RoutedEventHandler _onLogLevelChange;
        private readonly RoutedEventHandler _onAutoRefreshChange;
        private readonly RoutedEventHandler _onRefreshIntervalChange;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public ObservableCollection<LogEntry> LogEntries { get; private set; }

        public LogLevel LogLevel
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
            get {return  _refreshIntervalSeconds; }
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
            InitializeDefaults();

            _onLogLevelChange += (sender, args) => ReactToAutoRefresh();
            _onAutoRefreshChange += (sender, args) => ReactToAutoRefresh();
            _onRefreshIntervalChange += (sender, args) => ReactToAutoRefresh();

            Task.Factory.StartNew(ReactToAutoRefresh);

        }

        private void InitializeDefaults()
        {
            _logLevel = _logLevels[0];
            _autoRefresh = true;
            _refreshIntervalSeconds = 15;
            LogEntries = new ObservableCollection<LogEntry>();

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
            this.InvokeOnUiThread(() => LogEntries.Clear());

            Enumerable.Range(0, 100).ToList().ForEach(i =>
            {
                var random = new Random(Guid.NewGuid().GetHashCode());

                this.InvokeOnUiThread(() => LogEntries.Add(new LogEntry
                {
                    Date = DateTime.Now - TimeSpan.FromMinutes(i),
                    Level = _logLevel.Value == "*" ? LogLevels[random.Next(1, LogLevels.Count())].Value : _logLevel.Value,
                    Correlation = Guid.NewGuid()
                }));
            });
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