namespace FileExplorer_MichalRac.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class DirectoryInfoViewModel : FileSystemInfoViewModel
    {
        public Exception Exception { get; private set; }
        public FileSystemWatcher Watcher { get; private set; }
        public ObservableCollection<FileSystemInfoViewModel> Items { get; private set; } = new ObservableCollection<FileSystemInfoViewModel>();

        public DirectoryInfoViewModel(string argFullPath) : base(argFullPath) { }
        ~DirectoryInfoViewModel()
        {
            Dispose();
        }

        public bool Open(string path)
        {
            bool result = false;
            try
            {
                foreach(var dirName in Directory.GetDirectories(path))
                {
                    var dirInfo = new DirectoryInfo(dirName);
                    var itemViewModel = new DirectoryInfoViewModel(dirName);
                    itemViewModel.Model = dirInfo;
                    Items.Add(itemViewModel);

                    itemViewModel.Open(dirName);
                }
                foreach (var fileName in Directory.GetFiles(path))
                {
                    var fileInfo = new FileInfo(fileName);
                    var itemViewModel = new FileInfoViewModel(fileName);
                    itemViewModel.Model = fileInfo;
                    Items.Add(itemViewModel);

                    itemViewModel.Setup(fileName);
                }
                result = true;
            }
            catch (Exception ex)
            {
                Exception = ex;
            }

            if(Watcher == null)
            {
                Watcher = new FileSystemWatcher(path);
                Watcher.Created += OnFileSystemChanged;
                Watcher.Renamed += OnFileSystemChanged;
                Watcher.Deleted += OnFileSystemChanged;
                Watcher.Changed += OnFileSystemChanged;
                Watcher.Error += Watcher_Error;
                Watcher.EnableRaisingEvents = true;
            }

            return result;
        }

        public void Dispose()
        {
            Watcher.Created -= OnFileSystemChanged;
            Watcher.Renamed -= OnFileSystemChanged;
            Watcher.Deleted -= OnFileSystemChanged;
            Watcher.Changed -= OnFileSystemChanged;
            Watcher.Error -= Watcher_Error;
            Watcher.EnableRaisingEvents = false;
            Watcher = null;
        }

        private void OnFileSystemChanged(object sender, FileSystemEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => OnFileSystemChanged(e));
        }

        private void OnFileSystemChanged(FileSystemEventArgs e)
        {
            // TODO would be more efficient and better for user if it just updated affected files instead of full reload
            Items.Clear();
            var path = e.FullPath.Substring(0, e.FullPath.Count() - e.Name.Count() - 1);
            Open(path);
        }


        private void Watcher_Error(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        public bool FindRecursive(string path, out FileSystemInfoViewModel result, out DirectoryInfoViewModel parent)
        {
            result = default;
            parent = default;
            foreach (var item in Items)
            {
                if (item.FullPath == path)
                {
                    parent = this;
                    result = item;
                    return true;
                }
                else
                {
                    if(item is DirectoryInfoViewModel divm 
                        && divm.FindRecursive(path, out result, out parent))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
