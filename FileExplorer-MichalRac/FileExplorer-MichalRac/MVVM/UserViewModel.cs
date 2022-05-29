using FileExplorerBusinessLogic_MichalRac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer_MichalRac.MVVM
{
    public class UsersViewModel : ViewModelBase
    {
        private List<GridUserViewModel> users;

        public List<GridUserViewModel> Users
        {
            get
            {
                return users;
            }
            set
            {
                if (users != value || users != null)
                {
                    users = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public UsersViewModel()
        {
            using (FileManager manager = new FileManager())
            {
                List<GridUserViewModel> usersViewModel = new List<GridUserViewModel>();
                var users = manager.GetUsers();
                foreach (var user in users)
                {
                    GridUserViewModel viewModel = new GridUserViewModel(user);
                    usersViewModel.Add(viewModel);
                }

                Users = usersViewModel;
            }
        }

    }
}
