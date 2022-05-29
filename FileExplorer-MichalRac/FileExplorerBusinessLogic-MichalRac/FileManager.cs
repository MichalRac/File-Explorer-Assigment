using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileExplorerDatabase_MichalRac;
using FileExplorerDatabase_MichalRac.EntityFramework;

namespace FileExplorerBusinessLogic_MichalRac
{
    public class FileManager : IDisposable
    {
        public FileExplorerDBContext dbContext { get; }

        public FileManager()
        {
            dbContext = new FileExplorerDBContext();
            dbContext.Database.EnsureCreated();
        }

        public List<UserDto> GetUsers()
        {
            var users = new List<UserDto>();
            foreach (var user in dbContext.Users.ToList())
            {
                users.Add(new UserDto()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Password = user.Password,
                    Ip = user.IP
                });
            }
            return users;
        }
        public void CreateUser(UserDto dto)
        {
            if (dto.Login == null || dto.Password == null || dto.Ip == null || dto.Id == null)
            {
                return;
            }

            dbContext.Users.Add(new UserModel()
            {
                Id = dto.Id,
                Login = dto.Login,
                Password = dto.Password,
                IP = dto.Ip
            });

            dbContext.SaveChanges();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
