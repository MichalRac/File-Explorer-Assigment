namespace FileExplorer_MichalRac.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using FileExplorer_MichalRac.Commands;

    public class FileExplorer : ViewModelBase
    {
        public static int MaxThreadId = 0;
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
        public RelayCommand ShowUsersCommand { get; private set; }
        public RelayCommand CancelSortingCommand { get; private set; }
        public RelayCommand RegisterUserCommand { get; private set; }
        private CancellationTokenSource sortingCancellationTokenSource;
        private bool isSorting = false;

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
        private string statusUpdate;
        public string StatusUpdate
        {
            get
            {
                return statusUpdate;
            }
            protected set
            {
                statusUpdate = value;
                NotifyPropertyChanged();
            }
        }

        public static Action<string> onStatusUpdate;

        public FileExplorer()
        {
            NotifyPropertyChanged(nameof(Root));
            NotifyPropertyChanged(nameof(Lang));

            OpenRootFolderCommand = new RelayCommand(OpenRootFolderExecuteAsync);
            SortRootFolderCommand = new RelayCommand(SortRootFolderExecute, SortRootFolderCanExecute);
            OpenFileCommand = new RelayCommand(OpenFileExecute, OpenFileCanExecute);
            CancelSortingCommand = new RelayCommand(CancelSortingExecute, CancelSortingCanExecute);
            ShowUsersCommand = new RelayCommand(ShowUsersExecute, ShowUsersCanExecute);
            RegisterUserCommand = new RelayCommand(RegisterUserExecute, RegisterUserCanExcecute);

            onStatusUpdate += (newStatus) => { StatusUpdate = newStatus; };
        }

        private bool RegisterUserCanExcecute(object obj)
        {
            return MainWindow.loggedUser != null && MainWindow.FileManager != null && MainWindow.FileManager.IsHost(MainWindow.loggedUser);
        }

        private void RegisterUserExecute(object obj)
        {
            new RegisterUserWindow((login, pass, ip) =>
            {
                MainWindow.FileManager.CreateUser(new FileExplorerBusinessLogic_MichalRac.UserDto() { Login = login, Password = pass, Ip = ip });
            }).Show();
        }

        private void ShowUsersExecute(object obj)
        {
            new UserMangementWindow().Show();
        }

        private bool ShowUsersCanExecute(object obj) 
        {
            return MainWindow.loggedUser != null && MainWindow.FileManager != null && MainWindow.FileManager.IsHost(MainWindow.loggedUser);
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
            sortingCancellationTokenSource = new CancellationTokenSource();

            var dlg = new FolderBrowserDialog() { Description = Strings.File_Browser_Description };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await Task.Factory.StartNew(() =>
                    {
                        var path = dlg.SelectedPath;
                        OpenRoot(path);
                    }, sortingCancellationTokenSource.Token);

                }
                catch (OperationCanceledException)
                {
                    StatusMessage = Strings.Generic_Cancelled;
                }
                finally
                {
                    sortingCancellationTokenSource.Dispose();
                    sortingCancellationTokenSource = new CancellationTokenSource();
                }
            }
        }

        private void SortRootFolderExecute(object parameter)
        {
            new SortDialog(SortingSettings, SortDialog_OnOkayButton) { Title = Strings.Sort_Dialog_Description }.Show();
        }

        private async void SortDialog_OnOkayButton(SortingSettings argSortingSettings)
        {
            SortingSettings = argSortingSettings;

            sortingCancellationTokenSource = new CancellationTokenSource();

            StatusMessage = Strings.Sorting + Path.GetDirectoryName(root.FullPath);
            isSorting = true;

            await Task.Factory.StartNew(() =>
            {

                try
                {
                    Root.SortRecursive(argSortingSettings, sortingCancellationTokenSource.Token);
                }
                catch (Exception)
                {
                    Console.WriteLine($"\n{nameof(OperationCanceledException)} thrown\n");
                }
                finally
                {
                    sortingCancellationTokenSource.Dispose();
                    sortingCancellationTokenSource = new CancellationTokenSource();

                    StatusMessage = Strings.Ready;
                    Debug.WriteLine($"MaxThreadId: {MaxThreadId}");

                    isSorting = false;
                }

            }, sortingCancellationTokenSource.Token);

        }

        private bool SortRootFolderCanExecute(object parameter)
        {
            return Root != null 
                && Root.Items != null 
                && Root.Items.Count > 0
                && StatusMessage != Strings.Loading;
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

        private bool CancelSortingCanExecute(object obj)
        {
            return sortingCancellationTokenSource != null && isSorting;
        }

        private void CancelSortingExecute(object obj)
        {
            if(sortingCancellationTokenSource != null)
            {
                sortingCancellationTokenSource.Cancel();
                Console.WriteLine("Task cancellation requested.");
            }
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
