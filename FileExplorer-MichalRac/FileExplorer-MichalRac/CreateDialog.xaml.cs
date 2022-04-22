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
using System.Windows.Shapes;
using System.IO;

namespace FileExplorer_MichalRac
{
    /// <summary>
    /// Interaction logic for CreateDialog.xaml
    /// </summary>
    public partial class CreateDialog : Window
    {
        private string path;
        private Action<string> onFileCreated;

        public CreateDialog(string argPath, Action<string> argOnFileCreated)
        {
            Top = 200;
            path = argPath;
            onFileCreated = argOnFileCreated;

            InitializeComponent();
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fullPath = path + "\\" + cdName.Text;

                var attributes = new FileAttributes();
                if (rCB.IsChecked == true) attributes = attributes | FileAttributes.ReadOnly;
                if (aCB.IsChecked == true) attributes = attributes | FileAttributes.Archive;
                if (sCB.IsChecked == true) attributes = attributes | FileAttributes.System;
                if (hCB.IsChecked == true) attributes = attributes | FileAttributes.Hidden;

                if (fRadio.IsChecked == true)
                {
                    fullPath += ".txt";
                    File.CreateText(fullPath).Dispose();

                }
                else if (dRadio.IsChecked == true)
                {
                    Directory.CreateDirectory(fullPath);
                }

                File.SetAttributes(fullPath, attributes);
                onFileCreated?.Invoke(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR! Unexpected operation!");
            }
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
