using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorerBusinessLogic_MichalRac
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Ip { get; set; }
        public string Password { get; set; }
        public bool IsLogged { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsActive { get; set; }

    }
}
