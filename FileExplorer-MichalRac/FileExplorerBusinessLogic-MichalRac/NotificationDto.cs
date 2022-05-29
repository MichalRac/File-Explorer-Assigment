using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorerBusinessLogic_MichalRac
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string FullPath { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime CreationTime { get; set; }

    }
}
