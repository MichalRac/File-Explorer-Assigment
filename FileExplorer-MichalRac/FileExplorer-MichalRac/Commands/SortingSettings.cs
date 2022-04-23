using FileExplorer_MichalRac.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer_MichalRac.Commands
{
    public class SortingSettings : ViewModelBase
    {
        private SortBy sortBy = SortBy.Name;
        public SortBy SortBy
        {
            get
            {
                return sortBy;
            }
            set
            {
                sortBy = value;
                NotifyPropertyChanged();
            }
        }

        private Direction direction = Direction.Ascending;
        public Direction Direction
        {
            get
            {
                return direction;
            }
            set
            {
                 direction = value;
                NotifyPropertyChanged();
            }
        }

        public SortingSettings()
        {
            NotifyPropertyChanged(nameof(SortBy));
            NotifyPropertyChanged(nameof(Direction));
        }

    }
}
