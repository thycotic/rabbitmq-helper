using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thycotic.SecretServerEngine2.LogViewer.Models;
using Thycotic.SecretServerEngine2.LogViewer.Views;


namespace Thycotic.SecretServerEngine2.LogViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();

            var dataProvider = new LogDataProvider("Log4Net");
                
            var viewModel = new MainWindowViewModel();
            viewModel.Initialize(dataProvider);
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
