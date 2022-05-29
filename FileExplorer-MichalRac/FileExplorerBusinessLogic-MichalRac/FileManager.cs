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

        #region User
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

        #endregion

        #region Metadata

        public bool CreateMetadata(MetadataDto metadata)
        {
            dbContext.Metadata.Add(new MetadataModel()
            {
                Id = metadata.Id,
                FullPath = metadata.FullPath,
                Contributor = metadata.Contributor,
                Coverage = metadata.Coverage,
                Creator = metadata.Creator,
                Date = metadata.Date,
                Description = metadata.Description,
                Format = metadata.Format,
                Identifier = metadata.Identifier,
                Language = metadata.Language,
                Publisher = metadata.Publisher,
                Relation = metadata.Relation,
                Rights = metadata.Rights,
                Source = metadata.Source,
                Subject = metadata.Subject,
                Title = metadata.Title,
                Type = metadata.Type,
            });
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return true;
        }

        public bool UpdateMetadata(MetadataDto dto)
        {
            var metadata = GetMetadata(dto);
            if(metadata != null)
            {
                metadata.FullPath = dto.FullPath;
                metadata.Contributor = dto.Contributor;
                metadata.Coverage = dto.Coverage;
                metadata.Creator = dto.Creator;
                metadata.Date = dto.Date;
                metadata.Description = dto.Description;
                metadata.Format = dto.Format;
                metadata.Identifier = dto.Identifier;
                metadata.Language = dto.Language;
                metadata.Publisher = dto.Publisher;
                metadata.Relation = dto.Relation;
                metadata.Rights = dto.Rights;
                metadata.Source = dto.Source;
                metadata.Subject = dto.Subject;
                metadata.Title = dto.Title;
                metadata.Type = dto.Type;

                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return true;
            }

            return false;
        }

        public MetadataDto ConvertMetadataToDto(MetadataModel model)
        {
            return new MetadataDto()
            {
                Id = model.Id,
                FullPath = model.FullPath,
                Contributor = model.Contributor,
                Coverage = model.Coverage,
                Creator = model.Creator,
                Date = model.Date,
                Description = model.Description,
                Format = model.Format,
                Identifier = model.Identifier,
                Language = model.Language,
                Publisher = model.Publisher,
                Relation = model.Relation,
                Rights = model.Rights,
                Source = model.Source,
                Subject = model.Subject,
                Title = model.Title,
                Type = model.Type,
            };
        }

        public MetadataModel GetMetadata(string fullPath)
        {
            return dbContext.Metadata.Where(data => data.FullPath == fullPath).FirstOrDefault();
        }


        public MetadataModel GetMetadata(MetadataDto metadata)
        {
            return dbContext.Metadata.Where(data => data.FullPath == metadata.FullPath).FirstOrDefault();
        }

        public bool MetadataExists(string fullpath)
        {
            if (fullpath == null)
            {
                return false;
            }
            return dbContext.Metadata.Select(m => m.FullPath == fullpath).FirstOrDefault();
        }


        public void CreateOrUpdate(MetadataDto metadata)
        {
            if (MetadataExists(metadata.FullPath))
            {
                UpdateMetadata(metadata);
            }
            else
            {
                CreateMetadata(metadata);
            }

        }

        #endregion

        #region Access

        public void CreateOrUpdate(AccessDto access)
        {
            if(AccessExists(access.FileFullPath, access.UserId))
            {
                UpdateAccess(access);
            }
            else
            {
                CreateAccess(access);
            }
        }

        public bool AccessExists(string fullpath, string userId)
        {
            if (fullpath == null || userId == null)
            {
                return false;
            }
            return dbContext.Access.Select(m => m.FileFullPath == fullpath && m.UserId == userId).FirstOrDefault();
        }

        public bool CreateAccess(AccessDto access)
        {
            dbContext.Access.Add(new AccessModel()
            {
                Id = dbContext.Access.Count() + 1,
                UserId = access.UserId,
                FileFullPath = access.FileFullPath,
                Access = access.Access,
            });
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return true;
        }

        public bool UpdateAccess(AccessDto access)
        {
            var accessModel = GetAccess(access);
            if (accessModel != null)
            {
                accessModel.FileFullPath = access.FileFullPath;
                accessModel.UserId = access.UserId;
                accessModel.Access = access.Access;

                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return true;
            }

            return false;
        }

        public AccessModel GetAccess(string fullPath, string userId)
        {
            return dbContext.Access.Where(data => data.FileFullPath == fullPath && data.UserId == userId).FirstOrDefault();
        }


        public AccessModel GetAccess(AccessDto metadata)
        {
            return dbContext.Access.Where(data => data.FileFullPath == metadata.FileFullPath).FirstOrDefault();
        }


        #endregion

        #region Notifications

        public void CreateOrUpdate(NotificationDto notification)
        {
            if (NotificationExists(notification.FullPath))
            {
                UpdateNotification(notification);
            }
            else
            {
                CreateNotification(notification);
            }
        }

        private bool CreateNotification(NotificationDto notification)
        {
            dbContext.Notifications.Add(new NotificationsModel()
            {
                Id = dbContext.Notifications.Count() + 1,
                CreationTime = notification.CreationTime,
                LastModified = notification.LastModified,
            });
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return true;
        }

        private bool UpdateNotification(NotificationDto notification)
        {
            var notificationModel = GetNotification(notification);
            if (notificationModel != null)
            {
                notificationModel.FullPath = notification.FullPath;
                notificationModel.LastModified = notification.LastModified;
                notificationModel.CreationTime = notification.CreationTime;

                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                return true;
            }

            return false;
        }

        private NotificationsModel GetNotification(NotificationDto notification)
        {
            throw new NotImplementedException();
        }

        private bool NotificationExists(string fullPath)
        {
            if (fullPath == null)
            {
                return false;
            }
            return dbContext.Notifications.Select(m => m.FullPath == fullPath).FirstOrDefault();
        }


        #endregion

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
