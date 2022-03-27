using System.Windows;
//using System.Windows.Forms;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;

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
            Topmost = true;

            InitializeComponent();
            this.DataContext = this;

            dirView.SelectedItemChanged += DirView_SelectedItemChanged;
        }

        private void DirView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var dtvi = (DirectoryTreeViewItem)dirView.SelectedItem;
            var path = dtvi.FullPath;

            string rash = string.Empty;

            var attributes = File.GetAttributes(path);
            rash += attributes.HasFlag(FileAttributes.ReadOnly) ? "r" : "-";
            rash += attributes.HasFlag(FileAttributes.Archive) ? "a" : "-";
            rash += attributes.HasFlag(FileAttributes.System) ? "s" : "-";
            rash += attributes.HasFlag(FileAttributes.Hidden) ? "h" : "-";

            rashText.Text = rash;
        }

        // TODO Saving path in a tag is a dirty hack, to look into creating a derived class from TextBlock that would have a special field for caching and accessing it
        private void TextBlock_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var textBlock = ((TextBlock)sender);
            var path = textBlock.Tag.ToString();
            var contextMenu = new ContextMenu();
            textBlock.ContextMenu = contextMenu;

            var isDirectory = Directory.Exists(path);
            if (isDirectory)
            {
                var miCreate = new MenuItem() { Header = "Create", Tag = textBlock.Tag };
                miCreate.Click += CreateFile;
                contextMenu.Items.Add(miCreate);
            }
            else
            {
                var miOpen = new MenuItem() { Header = "Open", Tag = textBlock.Tag };
                miOpen.Click += OpenFile;
                contextMenu.Items.Add(miOpen);
            }

            var miDelete = new MenuItem() { Header = "Delete", Tag = textBlock.Tag };
            miDelete.Click += DeleteFile;
            contextMenu.Items.Add(miDelete);

            contextMenu.IsOpen = true;
        }

        private void TextBlock_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            var contextMenu = ((TextBlock)sender).ContextMenu;
            foreach (var item in contextMenu.Items)
            {
                ((MenuItem)item).Click -= OpenFile;
                ((MenuItem)item).Click -= CreateFile;
                ((MenuItem)item).Click -= DeleteFile;
            }
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            var path = ((MenuItem)sender).Tag.ToString();
            loadedTextFile.TryLoadNewText(path);
            fText.Text = loadedTextFile.LoadedText;
        }

        private void CreateFile(object sender, RoutedEventArgs e)
        {
            var path = ((MenuItem)sender).Tag.ToString();
            new CreateDialog(path, FileCreated).Show();
        }

        private void FileCreated(string path)
        {
            var dtvi = (DirectoryTreeViewItem)dirView.Items[0];

            if(dtvi.FullPath == path)
            {
                dtvi.Refresh();
            }
            else if (dtvi.FindRecursive(path, out var _, out var parent))
            {
                parent.Refresh();
            }
        }

        private void DeleteFile(object sender, RoutedEventArgs e)
        {
            var path = ((MenuItem)sender).Tag.ToString();
            var dtvi = (DirectoryTreeViewItem)dirView.Items[0];

            if(dtvi.FindRecursive(path, out var target, out var parent))
            {                
                parent.Items.Remove(target);
            }

            Directory.Delete(path, true);
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog() { Description = "Select directory to open" };

            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var path = dlg.SelectedPath;
                var fileExplorer = new MVVM.FileExplorer();
                fileExplorer.OpenRoot(path);
                DataContext = fileExplorer;
            }
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
