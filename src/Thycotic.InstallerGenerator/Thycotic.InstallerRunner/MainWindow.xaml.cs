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
            _viewModel = new MainWindowViewModel();
            _viewModel.Initialize();


            InitializeComponent();

            DataContext = _viewModel;

        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            try
            {
                ConfigureLogging();


                var installationTask = _viewModel.Install();

                installationTask.ContinueWith(task =>
                {
                    if (!_viewModel.IsInstallationSuccessful)
                    {
                        const string message = "Please review the log output and install.log for more information";

                        MessageBox.Show(message, "Installation failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An unexpected error has occurred: {0}", ex.Message), "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
