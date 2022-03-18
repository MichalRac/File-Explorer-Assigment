using System.Windows;
//using System.Windows.Forms;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace FileExplorer_MichalRac
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            var dlg = new FolderBrowserDialog() { Description = "Select directory to open" };
            dlg.ShowDialog();

            dirView.Items.Add(new DirectoryTreeViewItem(dlg.SelectedPath, true));
        }
    }
}
