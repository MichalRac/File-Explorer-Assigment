using Microsoft.EntityFrameworkCore;
using FileExplorerDatabase_MichalRac.EntityFramework;
using System.Reflection;

namespace FileExplorerDatabase_MichalRac
{
    public class FileExplorerDBContext : DbContext
    {
        public static readonly string DB_EXTENSION = ".db";
        public static readonly string DBName = "FileExplorerDatabase-MichalRac";
        public static readonly string Path = System.IO.Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DBName, DBName + DB_EXTENSION);

        public DbSet<UserModel> Users { get; set; }
        public DbSet<MetadataModel> Metadata { get; set; }
        public DbSet<NotificationsModel> Notifications { get; set; }
        public DbSet<AccessModel> Access { get; set; }

        public FileExplorerDBContext() : base()
        {
            Directory.CreateDirectory(System.IO.Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DBName));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={Path}");
/*        {
            optionsBuilder.UseSqlite("Filename=" + Path, option =>
            {
                option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            base.OnConfiguring(optionsBuilder);
        }*/

/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToTable("Users");
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Login).IsUnique();
            });
            modelBuilder.Entity<NotificationsModel>().ToTable("Notifications");
            modelBuilder.Entity<PermissionsModel>().ToTable("Permissions");
            modelBuilder.Entity<MetadataModel>().ToTable("Metadatas");

            base.OnModelCreating(modelBuilder);
        }*/
    }
}
