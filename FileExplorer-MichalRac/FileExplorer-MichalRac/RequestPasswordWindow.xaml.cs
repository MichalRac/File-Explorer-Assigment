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
    /// Interaction logic for RequestPasswordWindow.xaml
    /// </summary>
    public partial class RequestPasswordWindow : Window
    {
        private Action<string> OnPasswordChosen;
        public RequestPasswordWindow(Action<string> onPasswordChosen)
        {
            Top = 200;
            InitializeComponent();
            OnPasswordChosen = onPasswordChosen;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if(passBox.Text != null)
            {
                OnPasswordChosen?.Invoke(passBox.Text);
                this.Close();
            }
        }
    }
}