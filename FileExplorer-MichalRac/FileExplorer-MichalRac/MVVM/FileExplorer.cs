namespace FileExplorer_MichalRac.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
        public RelayCommand OpenRootFolderCommand { get; private set; }
        public RelayCommand SortRootFolderCommand { get; private set; }

        public FileExplorer()
        {
            NotifyPropertyChanged(nameof(Root));
            NotifyPropertyChanged(nameof(Lang));

            OpenRootFolderCommand = new RelayCommand(OpenRootFolderExecute);
            SortRootFolderCommand = new RelayCommand(SortRootFolderExecute, SortRootFolderCanExecute);
        }

        public void OpenRoot(string path)
        {
            Root = new DirectoryInfoViewModel(path);
            Root.Open(path);
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

        private void SortRootFolderExecute(object parameter)
        {
            new SortDialog() { Title = Strings.Sort_Dialog_Description }.Show();
        }

        private bool SortRootFolderCanExecute(object parameter)
        {
            return Root != null 
                && Root.Items != null 
                && Root.Items.Count > 0;
        }

    }
}
