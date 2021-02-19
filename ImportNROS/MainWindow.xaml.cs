using System;
using System.Windows;
using ImportBL;
using ImportBL.Interfaces;
using Microsoft.Win32;

namespace ImportNROS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IMainWorker _worker;

        public MainWindow()
        {
            _worker = DiContainer.Container.GetInstance<IMainWorker>();
            _worker.StateChanged += WriteLogs;

            InitializeComponent();
        }

        private void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Excel document (*xls, *.xlsx)|*.xlsx;*.xls"
            };

            if (fileDialog.ShowDialog() == true)
            {
                FilePathBox.Text = fileDialog.FileName;
                RunButton.IsEnabled = true;
            }
        }

        private async void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if (_worker.IsFree)
            {
                LogBox.Text = string.Empty;
                await _worker.Run(FilePathBox.Text);
            }
        }

        private void WriteLogs(object sender, EventArgs e)
        {
            LogBox.Text += _worker.GetNewLogs();
            ProgressBar.Value = _worker.Progress;
        }
    }
}
