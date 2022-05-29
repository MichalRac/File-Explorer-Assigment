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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        private Action<string, string> OnAttemptLogin;
        public LoginWindow(Action<string, string> onAttemptLogin)
        {
            Top = 200;
            InitializeComponent();
            OnAttemptLogin = onAttemptLogin;

            loginBox.Text = FileManager.HostUserName;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (passBox.Text != null && passBox.Text != null)
            {
                OnAttemptLogin?.Invoke(loginBox.Text, passBox.Text);
                this.Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Fill in all required data");
            }
        }
    }
}
