namespace FileExplorer_MichalRac.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FileSystemInfoViewModel : ViewModelBase
    {
        public ViewModelBase Owner { get; private set; }
        public string FullPath { get; }

        public FileSystemInfoViewModel(string argFullPath, ViewModelBase owner)
        {
            FullPath = argFullPath;
            Owner = owner;
        }

        public string Caption { get; set; }

        private FileSystemInfo _fileSystemInfo;
        public FileSystemInfo Model
        {
            get { return _fileSystemInfo; }
            set
            {
                if(_fileSystemInfo != value)
                {
                    _fileSystemInfo = value;

                    Caption = value.Name;
                    CreationTime = value.CreationTime;
                    CreationTimeUTC = value.CreationTimeUtc;
                    LastWriteTime = value.LastWriteTime;
                    LastWriteTimeUTC = value.LastWriteTimeUtc;
                    LastAccessTime = value.LastAccessTime;
                    LastAccessTimeUTC = value.LastAccessTimeUtc;

                    NotifyPropertyChanged();
                }
            }
        }
        public FileExplorer OwnerExplorer
        {
            get
            {
                var owner = Owner;
                while (owner is DirectoryInfoViewModel ownerDirectory)
                {
                    if (ownerDirectory.Owner is FileExplorer explorer)
                        return explorer;
                    owner = ownerDirectory.Owner;
                }
                return null;
            }
        }

        private DateTime _creationTime;
        public DateTime CreationTime
        {
            get { return _creationTime; }
            set
            {
                if(_creationTime != value )
                {
                    _creationTime = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private DateTime _creationTimeUTC;
        public DateTime CreationTimeUTC
        {
            get { return _creationTimeUTC; }
            set
            {
                if (_creationTimeUTC != value)
                {
                    _creationTimeUTC = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private DateTime _lastWriteTime;
        public DateTime LastWriteTime
        {
            get { return _lastWriteTime; }
            set
            {
                if (_lastWriteTime != value)
                {
                    _lastWriteTime = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private DateTime _lastWriteTimeUTC;
        public DateTime LastWriteTimeUTC
        {
            get { return _lastWriteTimeUTC; }
            set
            {
                if (_lastWriteTimeUTC != value)
                {
                    _lastWriteTimeUTC = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private DateTime _lastAccessTime;
        public DateTime LastAccessTime
        {
            get { return _lastAccessTime; }
            set
            {
                if (_lastAccessTime != value)
                {
                    _lastAccessTime = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private DateTime _lastAccessTimeUTC;
        public DateTime LastAccessTimeUTC
        {
            get { return _lastAccessTimeUTC; }
            set
            {
                if (_lastAccessTimeUTC != value)
                {
                    _lastAccessTimeUTC = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string statusMessage;
        public string StatusMessage
        {
            get
            {
                return statusMessage;
            }
            protected set
            {
                statusMessage = value;
                NotifyPropertyChanged();
            }
        }

        public virtual long GetDirectorySizeRecursive()
        {
            return 0;
        }
    }
}
