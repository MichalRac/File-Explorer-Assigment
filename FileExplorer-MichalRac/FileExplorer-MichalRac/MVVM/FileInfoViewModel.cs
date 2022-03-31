namespace FileExplorer_MichalRac.MVVM
{
    using System.IO;

    public class FileInfoViewModel : FileSystemInfoViewModel
    {
        public string IconPath { get; private set; }

        public FileInfoViewModel(string argFullPath) : base(argFullPath) { }

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

    }
}
