namespace FileExplorer_MichalRac.MVVM
{
    using FileExplorer_MichalRac.Commands;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;

    public class FileInfoViewModel : FileSystemInfoViewModel
    {
        public string IconPath { get; private set; }
        public RelayCommand OpenFileCommand { get; private set; }

        public FileInfoViewModel(string argFullPath, ViewModelBase owner) : base(argFullPath, owner) 
        {
            OpenFileCommand = new RelayCommand(OpenFileExecute, OpenFileCanExecute); ;
        }

        public void Setup(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            switch (extension)
            {
                case ".txt":
                    IconPath = "Assets/text-file.png";
                    break;
                case ".pdf":
                    IconPath = "Assets/pdf-file.png";
                    break;
                default:
                    IconPath = "Assets/unknown-file.png";
                    break;
            }
        }

        public override long GetDirectorySizeRecursive()
        {
            return new FileInfo(FullPath).Length;
        }

        private void OpenFileExecute(object parameter)
        {
            OwnerExplorer.OpenFileCommand.Execute(parameter);
        }

        private bool OpenFileCanExecute(object parameter)
        {
            return OwnerExplorer.OpenFileCommand.CanExecute(parameter);
        }
    }
}
