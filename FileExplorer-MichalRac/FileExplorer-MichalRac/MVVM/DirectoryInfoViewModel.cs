namespace FileExplorer_MichalRac.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DirectoryInfoViewModel : FileSystemInfoViewModel
    {
        public Exception Exception { get; private set; }
        public ObservableCollection<FileSystemInfoViewModel> Items { get; private set; } = new ObservableCollection<FileSystemInfoViewModel>();

        public bool Open(string path)
        {
            bool result = false;
            try
            {
                foreach(var dirName in Directory.GetDirectories(path))
                {
                    var dirInfo = new DirectoryInfo(dirName);
                    var itemViewModel = new DirectoryInfoViewModel();
                    itemViewModel.Model = dirInfo;
                    Items.Add(itemViewModel);
                }
                foreach (var fileName in Directory.GetFiles(path))
                {
                    var fileInfo = new FileInfo(path);
                    var itemViewModel = new FileInfoViewModel();
                    itemViewModel.Model = fileInfo;
                    Items.Add(itemViewModel);
                }
                result = true;
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
            return result;
        }
    }
}
