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
        private LoadedTextFile loadedTextFile = new LoadedTextFile();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            var dlg = new FolderBrowserDialog() { Description = "Select directory to open" };
            dlg.ShowDialog();

            dirView.Items.Add(new DirectoryTreeViewItem(dlg.SelectedPath, true));
        }

        private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // TODO Saving path in a tag is a dirty hack, to look into creating a derived class from TextBlock that would have a special field for caching and accessing it
            var path = ((TextBlock)sender).Tag.ToString();
            loadedTextFile.TryLoadNewText(path);
            fText.Text = loadedTextFile.LoadedText;
        }
    }
}
