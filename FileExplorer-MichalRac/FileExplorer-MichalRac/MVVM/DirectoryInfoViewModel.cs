namespace FileExplorer_MichalRac.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DirectoryInfoViewModel : FileSystemInfoViewModel
    {
        public ObservableCollection<FileSystemInfoViewModel> Items { get; private set; } = new ObservableCollection<FileSystemInfoViewModel>();
    }
}
