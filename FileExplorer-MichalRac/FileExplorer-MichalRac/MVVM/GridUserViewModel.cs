using FileExplorerBusinessLogic_MichalRac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer_MichalRac.MVVM
{
    public class GridUserViewModel : ViewModelBase
    {
        public GridUserViewModel()
        {
            dto = new UserDto();
        }

        public GridUserViewModel(UserDto dto)
        {
            this.dto = dto;
        }

        private UserDto dto;

        private void UpdateContext()
        {
            using (var fileManager = new FileManager())
            {
                fileManager.CreateOrUpdate(dto);
            }
        }
        public int Id
        {
            get { return dto.Id; }
            set
            {
                dto.Id = value; NotifyPropertyChanged();
                UpdateContext();
            }
        }

        public string Login
        {
            get
            {
                return dto.Login;
            }
            set
            {
                if (value != null && dto.Login != value)
                {
                    dto.Login = value;

                    NotifyPropertyChanged();
                    UpdateContext();
                }
            }
        }

        public string Password
        {
            get { return dto.Password; }
            set
            {
                if (value != null && dto.Password != value)
                {
                    dto.Password = value.ToString();
                    NotifyPropertyChanged();
                    UpdateContext();
                }
            }
        }

        public string Ip
        {
            get
            {
                return dto.Ip;
            }
            set
            {
                if (value != null && dto.Ip != value)
                    dto.Ip = value; NotifyPropertyChanged();
                UpdateContext();
            }
        }

        public bool IsHost
        {
            get
            {
                return dto.IsHost;
            }
            set
            {
                if (value != null && dto.IsHost != value)
                    NotifyPropertyChanged();
                UpdateContext();
            }
        }

    }
}
