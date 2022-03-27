namespace FileExplorer_MichalRac.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FileExplorer : ViewModelBase
    {
        public DirectoryInfoViewModel Root { get; set; }

        public void OpenRoot(string path)
        {
            Root = new DirectoryInfoViewModel();
            Root.Open(path);
        }
    }
}
