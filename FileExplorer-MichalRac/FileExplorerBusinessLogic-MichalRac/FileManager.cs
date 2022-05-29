using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using FileExplorerDatabase_MichalRac;
using FileExplorerDatabase_MichalRac.EntityFramework;

namespace FileExplorerBusinessLogic_MichalRac
{
    public class FileManager : IDisposable
    {
        public static string HostUserName => Environment.GetEnvironmentVariable("UserName");

        public FileExplorerDBContext dbContext { get; }

        public FileManager()
        {
            dbContext = new FileExplorerDBContext();
            dbContext.Database.EnsureCreated();
        }

        public bool VerifyDB()
        {
            return false;
        }

        public bool CheckInitUser()
        {
            return dbContext.Users.Select(user => user.Login == HostUserName).FirstOrDefault();
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
                    Ip = user.IP,
                    IsHost = user.IsHost,

                });
            }
            return users;
        }

        public void CreateUser(UserDto dto)
        {
            if (dto.Login == null || dto.Password == null || dto.Ip == null)
            {
                return;
            }

            dbContext.Users.Add(new UserModel()
            {
                Id = dto.Id,
                Login = dto.Login,
                Password = dto.Password,
                IP = dto.Ip,
                IsHost = dto.IsHost,
            });

            try
            {
                dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public UserModel GetUser(string login)
        {
            return dbContext.Users.Where(user => user.Login == login).Single();
        }

        public UserModel GetUser(UserDto dto)
        {
            return dbContext.Users.Where(user => user.Login == dto.Login).Single();
        }

        public bool UserExists(string login)
        {
            if (login == null)
            {
                return false;
            }
            return dbContext.Users.Select(u => u.Login == login).FirstOrDefault();
        }


        public bool SetAsHost(UserDto dto)
        {
            var user = GetUser(dto);
            if (user != null)
            {
                user.IsHost = true;
                dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool IsHost(UserDto dto)
        {
            return GetUser(dto).IsHost;
        }

        public bool IsUserLogedInAsAdministratorInWindows()
        {
            return WindowsIdentity.GetCurrent().IsAuthenticated;
        }

        public bool AttemptLogin(string login, string password)
        {
            var user = dbContext.Users.Where(user => user.Login == login).Single();
            if (user != null)
            {
                return user.Password == password;
            }
            else
            {
                CreateUser(new UserDto() { Id = dbContext.Users.Count() - 1, Login = login, Password = password, Ip = "<random password>" });
                return true;
            }
            return false;
        }

        public bool UpdateUser(UserDto newDto)
        {
            var user = GetUser(newDto);
            if(user != null)
            {
                user.Password = newDto.Password;
                user.IP = newDto.Ip;
                user.IsHost = newDto.IsHost;
                dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public void CreateOrUpdate(UserDto user)
        {
            if (UserExists(user.Login))
                UpdateUser(user);
            else
                CreateUser(user);
        }


        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
