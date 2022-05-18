namespace FileExplorer_MichalRac.MVVM
{
    using FileExplorer_MichalRac.Asyncs;
    using FileExplorer_MichalRac.Commands;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

    public class DirectoryInfoViewModel : FileSystemInfoViewModel
    {
        public Exception Exception { get; private set; }
        public FileSystemWatcher Watcher { get; private set; }
        public DispatchedObservableCollection<FileSystemInfoViewModel> Items { get; private set; } = new DispatchedObservableCollection<FileSystemInfoViewModel>();

        public DirectoryInfoViewModel(string argFullPath, ViewModelBase owner) : base(argFullPath, owner) { }

        ~DirectoryInfoViewModel()
        {
            Dispose();
        }

        public bool Open(string path)
        {
            bool result = false;
            foreach (var dirName in Directory.GetDirectories(path))
            {
                var dirInfo = new DirectoryInfo(dirName);
                var itemViewModel = new DirectoryInfoViewModel(dirName, this);
                itemViewModel.Model = dirInfo;

                try
                {
                    if (itemViewModel.Open(dirName))
                    {
                        Items.Add(itemViewModel);
                    }

                }
                catch (Exception ex)
                {
                    Exception = ex;
                }
            }
            foreach (var fileName in Directory.GetFiles(path))
            {
                var fileInfo = new FileInfo(fileName);
                var itemViewModel = new FileInfoViewModel(fileName, this);
                itemViewModel.Model = fileInfo;
                Items.Add(itemViewModel);

                itemViewModel.Setup(fileName);
            }
            result = true;

            if(Watcher == null && result == true)
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
            if(Watcher != null)
            {
                Watcher.Created -= OnFileSystemChanged;
                Watcher.Renamed -= OnFileSystemChanged;
                Watcher.Deleted -= OnFileSystemChanged;
                Watcher.Changed -= OnFileSystemChanged;
                Watcher.Error -= Watcher_Error;
                Watcher.EnableRaisingEvents = false;
                Watcher = null;
            }
        }

        public async void SortRecursive(SortingSettings sortingSettings)
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


            var subFolders = new List<DirectoryInfoViewModel>();
            foreach (var item in Items)
            {
                if(item is DirectoryInfoViewModel divm)
                {
                    subFolders.Add(divm);
                }
            }

            Task[] taskArray = new Task[subFolders.Count];

            for (int i = 0; i < subFolders.Count; i++)
            {
                var currentLoopCache = i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    Debug.WriteLine($"ThreadId {Thread.CurrentThread.ManagedThreadId}, sorting directory: {subFolders[currentLoopCache].Caption}");
                    subFolders[currentLoopCache].SortRecursive(sortingSettings);
                    Debug.WriteLine($"Directory: {subFolders[currentLoopCache].Caption} - Sorted");
                });
            }             
            Task.WaitAll(taskArray);
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

        private void Root_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "StatusMessage" && sender is FileSystemInfoViewModel viewModel)
                StatusMessage = viewModel.StatusMessage;
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in args.NewItems.Cast<FileSystemInfoViewModel>())
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in args.NewItems.Cast<FileSystemInfoViewModel>())
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                    }
                    break;
            }
        }

        private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "StatusMessage" && sender is FileSystemInfoViewModel viewModel)
                this.StatusMessage = viewModel.StatusMessage;
        }
    }
}
