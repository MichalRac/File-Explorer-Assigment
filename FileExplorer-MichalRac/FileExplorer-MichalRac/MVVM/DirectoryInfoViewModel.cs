namespace FileExplorer_MichalRac.MVVM
{
    using FileExplorer_MichalRac.Commands;
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

        public void SortRecursive(SortingSettings sortingSettings)
        {
            IOrderedEnumerable<FileSystemInfoViewModel> sorted = null;
            switch (sortingSettings.Direction)
            {
                case Direction.Ascending:
                    switch (sortingSettings.SortBy)
                    {
                        case SortBy.Name:
                            sorted = Items.OrderBy(i => i.Caption);
                            break;
                        case SortBy.Extension:
                            sorted = Items.OrderBy(i => Path.GetExtension(i.FullPath));
                            break;
                        case SortBy.Size:
                            sorted = Items.OrderBy(i => i.GetDirectorySizeRecursive());
                            break;
                        case SortBy.ModifyDate:
                            sorted = Items.OrderBy(i => i.LastWriteTime);
                            break;
                    }
                    break;
                case Direction.Descending:
                    switch (sortingSettings.SortBy)
                    {
                        case SortBy.Name:
                            sorted = Items.OrderByDescending(i => i.Caption);
                            break;
                        case SortBy.Extension:
                            sorted = Items.OrderByDescending(i => Path.GetExtension(i.FullPath));
                            break;
                        case SortBy.Size:
                            sorted = Items.OrderByDescending(i => i.GetDirectorySizeRecursive());
                            break;
                        case SortBy.ModifyDate:
                            sorted = Items.OrderByDescending(i => i.LastWriteTime);
                            break;
                    }
                    break;
            }

            if(sorted != null)
            {
                List<FileSystemInfoViewModel> temp = new List<FileSystemInfoViewModel>();
                temp.AddRange(sorted);

                Items.Clear();

                foreach (var item in temp)
                {
                    if(item is DirectoryInfoViewModel divm)
                        Items.Add(divm);
                }

                foreach (var item in temp)
                {
                    if (item is FileInfoViewModel fivm)
                        Items.Add(fivm);
                }

            }

            foreach (var itemViewModel in Items)
            {
                if(itemViewModel is DirectoryInfoViewModel divm)
                {
                    divm.SortRecursive(sortingSettings);
                }
            }
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

        public override long GetDirectorySizeRecursive()
        {
            long total = 0;

            foreach(var item in Items)
            {
                total += item.GetDirectorySizeRecursive();
            }

            return total;
        }

    }
}
