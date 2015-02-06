using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Thycotic.SecretServerEngine2.LogViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<LogEntry> _entries = new ObservableCollection<LogEntry>();

        public ObservableCollection<LogEntry> LogEntries
        {
            get { return _entries; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LastNameCM_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

    public class LogEntry
    {
        public DateTime Date { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
    }
}
