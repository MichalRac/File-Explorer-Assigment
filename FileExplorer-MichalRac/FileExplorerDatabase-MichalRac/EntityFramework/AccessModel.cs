using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorerDatabase_MichalRac.EntityFramework
{
    public class AccessModel
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public string FileFullPath { get; set; }
        public bool Access { get; set; }
    }
}
