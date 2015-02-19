using System;
using System.Windows;

namespace Thycotic.DistributedEngine.LogViewer
{
    public static class UiHelpers
    {
        public static void InvokeOnUiThread(this IViewModel viewModel, Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}