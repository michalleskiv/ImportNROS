using System;
using System.Collections.Generic;
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
using ImportBL;
using Microsoft.Win32;

namespace ImportNROS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWorker _worker = new MainWorker();

        public MainWindow()
        {
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
            //_worker.FilePath = FilePathBox.Text;
            await _worker.Run();
        }
    }
}
