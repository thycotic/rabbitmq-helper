using System;
using System.ComponentModel;
using System.Windows;
using Thycotic.InstallerRunner.Views;
using Thycotic.Logging;

namespace Thycotic.InstallerRunner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            ConfigureLogging();
            
            _viewModel = new MainWindowViewModel();
            _viewModel.Initialize();

            DataContext = _viewModel;

            var installationTask = _viewModel.Install();

            installationTask.ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    var message =
                        string.Format("{0}\n\nPlease review the log output and install.log for more information",
                            task.Exception.Message);
                    MessageBox.Show(message, "Lack of success ;(", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    //if (MessageBox.Show("Installation completed.\n\nClose installer?", "Success!",
                    //    MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                    //{
                        Application.Current.Dispatcher.Invoke(Close);
                    //}
                }
            });


        }
        private static void ConfigureLogging()
        {
            Log.Configure();
            Log.AttachRecentEventsMemoryAppender();

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        
            _viewModel.Dispose();
        }
    }
}
