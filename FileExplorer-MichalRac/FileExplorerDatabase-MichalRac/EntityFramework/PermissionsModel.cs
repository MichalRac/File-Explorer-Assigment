using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorerDatabase_MichalRac.EntityFramework
{
    public class PermissionsModel
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Access { get; set; }
    }
}
