using FileExplorer_MichalRac.Commands;
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
    /// Interaction logic for SortDialog.xaml
    /// </summary>
    public partial class SortDialog : Window
    {
        private SortingSettings sortingSettings;
        private Action<SortingSettings> OnOkayButtonClicked;

        public SortDialog(SortingSettings initSettings, Action<SortingSettings> onOkayButtonClicked)
        {
            Top = 200;

            sortingSettings = initSettings;
            DataContext = sortingSettings;

            InitializeComponent();

            OnOkayButtonClicked = onOkayButtonClicked;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();

            var sortingSettings = new SortingSettings()
            {
                Direction = ReadDirection(),
                SortBy = ReadSortingMethod(),
            };

            OnOkayButtonClicked.Invoke(sortingSettings);
        }

        private Direction ReadDirection()
        {
            return ascRadio.IsChecked.Value 
                ? Direction.Ascending 
                : Direction.Descending;
        }

        private SortBy ReadSortingMethod()
        {
            if (nameRadio.IsChecked.Value) return SortBy.Name;
            else if (extensionRadio.IsChecked.Value) return SortBy.Extension;
            else if (sizeRadio.IsChecked.Value) return SortBy.Size;
            else if (modifyRadio.IsChecked.Value) return SortBy.ModifyDate;
            else
                throw new NotImplementedException();
            
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
