using FileExplorerBusinessLogic_MichalRac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer_MichalRac.MVVM
{
    public class MetadataViewModel : ViewModelBase
    {
        private MetadataDto dto;
        public MetadataViewModel()
        {
            dto = new MetadataDto();
        }

        public MetadataViewModel(MetadataDto dto)
        {
            this.dto = dto;
        }

        private void UpdateContext()
        {
            using (var fileManager = new FileManager())
            {
                fileManager.CreateOrUpdate(dto);
            }
        }

        public int ID
        {
            get { return dto.Id; }
            set
            {
                dto.Id = value; 
                NotifyPropertyChanged();
                UpdateContext();
            }
        }

        public string FullPath
        {
            get { return dto.FullPath; }
            set
            {
                dto.FullPath = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }

        public string Contributor
        {
            get { return dto.Contributor; }
            set
            {
                dto.Contributor = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }

        public string Coverage
        {
            get { return dto.Coverage; }
            set
            {
                dto.Coverage = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }
        public string Creator
        {
            get { return dto.Creator; }
            set
            {
                dto.Creator = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }
        public string Date
        {
            get { return dto.Date; }
            set
            {
                dto.Date = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }
        public string Description
        {
            get { return dto.Description; }
            set
            {
                dto.Description = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }
        public string Format
        {
            get { return dto.Format; }
            set
            {
                dto.Format = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }
        public string Identifier
        {
            get { return dto.Identifier; }
            set
            {
                dto.Identifier = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }
        public string Language
        {
            get { return dto.Language; }
            set
            {
                dto.Language = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }
        public string Publisher
        {
            get { return dto.Publisher; }
            set
            {
                dto.Publisher = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }
        public string Relation
        {
            get { return dto.Relation; }
            set
            {
                dto.Relation = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }
        public string Rights
        {
            get { return dto.Rights; }
            set
            {
                dto.Rights = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }
        public string Source
        {
            get { return dto.Source; }
            set
            {
                dto.Source = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }
        public string Subject
        {
            get { return dto.Subject; }
            set
            {
                dto.Subject = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }
        public string Title
        {
            get { return dto.Title; }
            set
            {
                dto.Title = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }
        public string Type
        {
            get { return dto.Type; }
            set
            {
                dto.Type = value;
                NotifyPropertyChanged();
                UpdateContext();
            }
        }

    }
}
