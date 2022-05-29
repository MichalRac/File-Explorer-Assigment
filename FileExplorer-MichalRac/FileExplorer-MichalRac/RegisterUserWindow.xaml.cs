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
    /// Interaction logic for RegisterUserWindow.xaml
    /// </summary>
    public partial class RegisterUserWindow : Window
    {
        private Action<string, string, string> OnUserRegistered;
        public RegisterUserWindow(Action<string, string, string> onUserRegistered)
        {
            Top = 200;
            InitializeComponent();
            OnUserRegistered = onUserRegistered;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (passBox.Text != null && passBox.Text != null && ipBox.Text != null)
            {
                OnUserRegistered?.Invoke(loginBox.Text, passBox.Text, ipBox.Text);
                this.Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Fill in all required data");
            }
        }
    }
}
