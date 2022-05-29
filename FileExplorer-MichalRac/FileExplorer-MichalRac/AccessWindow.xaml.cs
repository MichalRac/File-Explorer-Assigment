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
    /// Interaction logic for AccessWindow.xaml
    /// </summary>
    public partial class AccessWindow : Window
    {
        private string fullpath;
        private List<CheckBox> checkboxes = new List<CheckBox>();
        public AccessWindow(string fullPath)
        {
            InitializeComponent();

            this.fullpath = fullPath;
            using(FileManager fm = new FileManager())
            {
                var users = fm.GetUsers();
            
                foreach(var user in users)
                {
                    var checkbox = new CheckBox();
                    checkbox.Content = user.Login;
                    content.Children.Add(checkbox);
                    if(fm.AccessExists(fullpath, user.Login))
                    {
                        checkbox.IsChecked = fm.GetAccess(fullPath, user.Login).Access;
                    }

                    checkboxes.Add(checkbox);
                }
            }
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            using (FileManager fm = new FileManager())
            {
                foreach(var checkbox in checkboxes)
                {
                    fm.CreateOrUpdate(new AccessDto() { FileFullPath = fullpath, Access = checkbox.IsChecked.Value, UserId = (string)checkbox.Content });
                }
            }


            this.Close();
        }
    }
}
