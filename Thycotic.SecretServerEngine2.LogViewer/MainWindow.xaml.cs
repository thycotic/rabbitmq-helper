using System;
using System.Collections;
using System.Dynamic;
using System.Windows;
using Thycotic.SecretServerEngine2.LogViewer.Views;


namespace Thycotic.SecretServerEngine2.LogViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var dataProvider = new LogDataProvider("Log4Net");
                
            var viewModel = new MainWindowViewModel();
            viewModel.Initialize(dataProvider);

            LogCorrelations.SelectionChanged += (sender, args) =>
            {
                System.Diagnostics.Trace.TraceInformation("");
            };

            LogItemsInCorrelation.SelectionChanged += (sender, args) =>
            {
                System.Diagnostics.Trace.TraceInformation("");

            };


            DataContext = viewModel;
        }

    
    }
}
