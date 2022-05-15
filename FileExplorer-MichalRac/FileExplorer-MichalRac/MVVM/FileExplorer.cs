namespace FileExplorer_MichalRac.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using FileExplorer_MichalRac.Commands;

    public class FileExplorer : ViewModelBase
    {
        private DirectoryInfoViewModel root;
        public DirectoryInfoViewModel Root 
        { 
            get { return root; } 
            set 
            {
                if(root != value)
                {
                    root = value;
                    NotifyPropertyChanged(nameof(Root));
                }
            }
        }

        public string Lang
        {
            get { return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName; }
            set
            {
                if( value != null )
                {
                    if( CultureInfo.CurrentUICulture.TwoLetterISOLanguageName != value )
                    {
                        CultureInfo.CurrentUICulture = new CultureInfo( value );
                        NotifyPropertyChanged();
                    }
                }
            }
        }

        public event EventHandler<FileInfoViewModel> OnOpenFileRequest;

        public RelayCommand OpenRootFolderCommand { get; private set; }
        public RelayCommand SortRootFolderCommand { get; private set; }
        public RelayCommand OpenFileCommand { get; private set; }

        private SortingSettings sortingSettings;
        public SortingSettings SortingSettings
        {
            get 
            { 
                if(sortingSettings == null)
                {
                    sortingSettings = new SortingSettings();
                }
                return sortingSettings; 
            }
            set 
            { 
                if(sortingSettings != value)
                {
                    sortingSettings = value;
                }
            }
        }
        private string statusMessage;
        public string StatusMessage { 
            get
            {
                return statusMessage;
            }
            protected set
            {
                statusMessage = value;
                NotifyPropertyChanged();
            }
        }

        public FileExplorer()
        {
            NotifyPropertyChanged(nameof(Root));
            NotifyPropertyChanged(nameof(Lang));

            OpenRootFolderCommand = new RelayCommand(OpenRootFolderExecuteAsync);
            SortRootFolderCommand = new RelayCommand(SortRootFolderExecute, SortRootFolderCanExecute);
            OpenFileCommand = new RelayCommand(OpenFileExecute, OpenFileCanExecute); ;
        }
        public void OpenRoot(string path)
        {
            Root = new DirectoryInfoViewModel(path, this);
            StatusMessage = Strings.Loading;
            Root.Open(path);
            StatusMessage = Strings.Ready;
        }

        private void OpenRootFolderExecute(object parameter)
        {
            var dlg = new FolderBrowserDialog() { Description = Strings.File_Browser_Description };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var path = dlg.SelectedPath;
                OpenRoot(path);
            }
        }

        private async void OpenRootFolderExecuteAsync(object paramter)
        {
            var dlg = new FolderBrowserDialog() { Description = Strings.File_Browser_Description };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                await Task.Factory.StartNew(() =>
                {
                    var path = dlg.SelectedPath;
                    OpenRoot(path);
                });
            }
        }

        private void SortRootFolderExecute(object parameter)
        {
            new SortDialog(SortingSettings, SortDialog_OnOkayButton) { Title = Strings.Sort_Dialog_Description }.Show();
        }

        private void SortDialog_OnOkayButton(SortingSettings argSortingSettings)
        {
            SortingSettings = argSortingSettings;

            Root.SortRecursive(argSortingSettings);
        }

        private bool SortRootFolderCanExecute(object parameter)
        {
            return Root != null 
                && Root.Items != null 
                && Root.Items.Count > 0;
        }

        public static readonly string[] TextFilesExtensions = new string[] { ".txt", ".ini", ".log" };
        public bool OpenFileCanExecute(object parameter)
        {
            if (parameter is FileInfoViewModel viewModel)
            {
                var extension = Path.GetExtension(viewModel.FullPath)?.ToLower();
                return TextFilesExtensions.Contains(extension);
            }
            return false;
        }

        public void OpenFileExecute(object obj)
        {
            OnOpenFileRequest.Invoke(this, (FileInfoViewModel)obj);
        }

        public object GetFileContent(FileInfoViewModel viewModel)
        {
            var extension = Path.GetExtension(viewModel.FullPath)?.ToLower();
            if (TextFilesExtensions.Contains(extension))
            {
                var loadedTextFile = new LoadedTextFile();
                loadedTextFile.TryLoadNewText(viewModel.FullPath);
                var content = loadedTextFile.LoadedText;
                return content;
            }
            return null;
        }

        public void Root_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "StatusMessage" && sender is FileSystemInfoViewModel viewModel)
                StatusMessage = viewModel.StatusMessage;
        }
    }
}
