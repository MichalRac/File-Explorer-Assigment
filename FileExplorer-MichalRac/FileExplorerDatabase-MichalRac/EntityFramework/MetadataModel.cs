using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorerDatabase_MichalRac.EntityFramework
{
    public class MetadataModel
    {
        [Key]
        public string CreatorID { get; set; }
        public DateTime CreationTime { set; get; }
        public string Description { get; set; }
        public string History { get; set; }
    }
}
