using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorerDatabase_MichalRac.EntityFramework
{
    public class UserModel
    {
        [Key][StringLength(5)] 
        public int Id { get; set; }
        
        [Required] 
        public string Login { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string IP { get; set; }

    }
}
