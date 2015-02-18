using System;
using System.Linq;
using System.Windows.Controls;
using Thycotic.SecretServerEngine.LogViewer.Models;
using Thycotic.SecretServerEngine.LogViewer.Providers;
using Thycotic.SecretServerEngine.LogViewer.Views;


namespace Thycotic.SecretServerEngine.LogViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Func<LogDataProvider> _dataProviderFactory = () => new LogDataProvider("Log4Net");

        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainWindowViewModel();
            viewModel.Initialize(_dataProviderFactory);
            _viewModel = viewModel;
            LogCorrelations.SelectionChanged += OnLogCorrelationChanged;
            LogItemsInCorrelation.SelectionChanged += OnLogItemChanged;

            DataContext = viewModel;
        }

        private void OnLogCorrelationChanged(object sender, SelectionChangedEventArgs args)
        {
            if (args.AddedItems.Count > 0)
            {
                var selectedLogEntry = args.AddedItems.Cast<LogEntry>().First();
                _viewModel.SelectedCorrelation = selectedLogEntry.Correlation;
            }
            else
            {
                _viewModel.SelectedCorrelation = null;
            }
            
        }

        private void OnLogItemChanged(object sender, SelectionChangedEventArgs args)
        {
            if (args.AddedItems.Count > 0)
            {
                var selectedLogEntry = args.AddedItems.Cast<LogEntry>().First();
                _viewModel.SelectedLogEntry = selectedLogEntry;
            }
            else
            {
                _viewModel.SelectedLogEntry = null;
            }
        }
    }
}
