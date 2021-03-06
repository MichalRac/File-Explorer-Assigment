using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorerDatabase_MichalRac.EntityFramework
{
    public class NotificationsModel
    {
        [Key]
        public int Id { get; set; }
        
        public string FullPath { get; set; }

        public DateTime LastModified { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
