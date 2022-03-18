using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

namespace FileExplorer_MichalRac
{
    public class DirectoryTreeViewItem
    {
        public string? Title { get; set; }
        public string? FullPath { get; set; }
        public ObservableCollection<DirectoryTreeViewItem> Items { get; set; }

        public DirectoryTreeViewItem(string path, bool isDirectory)
        {
            try
            {
                Title = Path.GetFileName(path);
                FullPath = path;
                Items = isDirectory 
                    ? ReadFilesFromDirectory(path) 
                    : new ObservableCollection<DirectoryTreeViewItem>();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                throw;
            }
        }

        public ObservableCollection<DirectoryTreeViewItem> ReadFilesFromDirectory(string path)
        {
            var result = new ObservableCollection<DirectoryTreeViewItem>();
            var directoryPaths = Directory.GetDirectories(path);

            foreach (var directoryPath in directoryPaths)
            {
                var dtvi = new DirectoryTreeViewItem(directoryPath, true);
                result.Add(dtvi);
            }

            var filePaths = Directory.GetFiles(path);
            foreach(var filePath in filePaths)
            {
                result.Add(new DirectoryTreeViewItem(filePath, false));
            }

            return result;
        }
    }
}
