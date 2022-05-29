using FileExplorer_MichalRac.MVVM;
using FileExplorerBusinessLogic_MichalRac;
using FileExplorerDatabase_MichalRac.EntityFramework;
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
    /// Interaction logic for MetadataWindow.xaml
    /// </summary>
    public partial class MetadataWindow : Window
    {
        private MetadataViewModel metadataModel;

        private Action<MetadataViewModel> onClosed;

        public MetadataWindow(MetadataDto dto, Action<MetadataViewModel> argOnClosed)
        {
            InitializeComponent();
            onClosed = argOnClosed;

            metadataModel = new MetadataViewModel();
            metadataModel.FullPath = dto.FullPath;
            metadataModel.Contributor = dto.Contributor;
            metadataModel.Coverage = dto.Coverage;
            metadataModel.Creator = dto.Creator;
            metadataModel.Date = dto.Date;
            metadataModel.Description = dto.Description;
            metadataModel.Format = dto.Format;
            metadataModel.Identifier = dto.Identifier;
            metadataModel.Language = dto.Language;
            metadataModel.Publisher = dto.Publisher;
            metadataModel.Relation = dto.Relation;
            metadataModel.Rights = dto.Rights;
            metadataModel.Source = dto.Source;
            metadataModel.Subject = dto.Subject;
            metadataModel.Title = dto.Title;
            metadataModel.Type = dto.Type;

            DataContext = metadataModel;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            onClosed?.Invoke(metadataModel);
            this.Close();
        }
    }
}
