using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorerBusinessLogic_MichalRac
{
    public class AccessDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FileFullPath { get; set; }
        public bool Access { get; set; }

    }
}
