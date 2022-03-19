using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer_MichalRac
{
    public class LoadedTextFile
    {
        public string LoadedText { get; private set; } = string.Empty;

        public void TryLoadNewText(string? path)
        {
            if (path == null)
            {
                return;
            }

            try
            {
                using (var textReader = File.OpenText(path))
                {
                    LoadedText = textReader.ReadToEnd();

                    if(LoadedText == string.Empty)
                    {
                        LoadedText = "<no text found>";
                    }
                }
            }
            catch (System.Exception)
            {
                LoadedText = string.Empty;
                return;
            }
        }
    }
}
