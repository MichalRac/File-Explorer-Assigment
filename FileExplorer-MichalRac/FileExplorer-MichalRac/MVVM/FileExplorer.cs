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
        public DirectoryInfoViewModel Root { get; set; }

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
            NotifyPropertyChanged(nameof(Lang));
        }

        public void OpenRoot(string path)
        {
            Root = new DirectoryInfoViewModel();
            Root.Open(path);
        }
    }
}
