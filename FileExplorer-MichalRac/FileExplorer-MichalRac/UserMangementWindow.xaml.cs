using FileExplorer_MichalRac.MVVM;
using FileExplorerBusinessLogic_MichalRac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FileExplorer_MichalRac
{
    /// <summary>
    /// Interaction logic for UserMangementWindow.xaml
    /// </summary>
    public partial class UserMangementWindow : Window
    {
        public UserMangementWindow()
        {
            InitializeComponent();
            UsersViewModel model = new UsersViewModel();
            DataContext = model;
        }

        private void DG1_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            UserDto user = new UserDto();
            int id = 1;

            using (FileManager manager = new FileManager())
            {
                id = manager.GetUsers().Count();
            }

            user.Id = id;
            user.Ip = "<random IP>";

            GridUserViewModel model = new GridUserViewModel(user);

            e.NewItem = model;
        }



        private void DG1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if ((e.EditAction == DataGridEditAction.Commit) == false) return;

            UserDto model = e.Row.DataContext as UserDto;

            using (FileManager manager = new FileManager())
            {
                if (manager.GetUser(model.Login) != null)
                {
                    manager.UpdateUser(model);
                }
                else
                {
                    manager.CreateUser(model);
                }
            }
        }
    }
}
