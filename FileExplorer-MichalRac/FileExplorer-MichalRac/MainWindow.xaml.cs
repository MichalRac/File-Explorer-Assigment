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
using FileExplorer_MichalRac.MVVM;
using System.Globalization;

namespace FileExplorer_MichalRac
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoadedTextFile loadedTextFile = new LoadedTextFile();
        public FileExplorer FileExplorer { get; private set; }

        public MainWindow()
        {
            Top = 100;

            InitializeComponent();
            FileExplorer = new FileExplorer();
            DataContext = FileExplorer;

            dirView.SelectedItemChanged += DirView_SelectedItemChanged;
            FileExplorer.PropertyChanged += FileExplorer_PropertyChanged;
        }

        private void DirView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var divm = (FileSystemInfoViewModel)dirView.SelectedItem;
            var path = divm.FullPath;

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
            var textBlock = ((FrameworkElement)sender);
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
            var contextMenu = ((FrameworkElement)sender).ContextMenu;
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
            //var dtvi = (DirectoryTreeViewItem)dirView.Items[0];
/*            var divm = (DirectoryInfoViewModel)dirView.Items[0];
            divm.Update();
*/        }

        private void DeleteFile(object sender, RoutedEventArgs e)
        {
            var path = ((FrameworkElement)sender).Tag.ToString();
            var divm = FileExplorer.Root;

            if (divm.FindRecursive(path, out var fsivm, out _) && fsivm is DirectoryInfoViewModel targetDivm)
            {
                targetDivm.Dispose();
            }

            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                else if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (System.Exception ex)
            {
                if (ex is System.UnauthorizedAccessException)
                {
                    System.Windows.MessageBox.Show("Unauthorized for removing readonly files");
                }
                else
                {
                    throw;
                }
            }
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void FileExplorer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(FileExplorer.Lang))
            {
                CultureResources.ChangeCulture(CultureInfo.CurrentUICulture);
            }
        }
    }
}
