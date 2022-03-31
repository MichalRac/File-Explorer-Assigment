namespace FileExplorer_MichalRac.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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

        public FileExplorer()
        {
            NotifyPropertyChanged(nameof(Root));
            NotifyPropertyChanged(nameof(Lang));
        }

        public void OpenRoot(string path)
        {
            Root = new DirectoryInfoViewModel(path);
            Root.Open(path);
        }
    }
}
