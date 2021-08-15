using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contexts
{
    public class SuppDatabaseContext: DbContext
    {
        public SuppDatabaseContext(DbContextOptions<SuppDatabaseContext> options) : base(options)
        {

        }

        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Authentication> Authentications { get; set; }
        public DbSet<Models.UserRoleType> UserRoleTypes { get; set; }
        public DbSet<Models.UserRole> UserRoles { get; set; }
        public DbSet<Models.GoogleDriveAccount> GoogleDriveAccounts { get; set; }
        public DbSet<Models.GoogleDriveAuth> GoogleDriveAuths { get; set; }
        public DbSet<Models.MediaConfiguration> MediaConfigurations { get; set; }
        public DbSet<Models.Media> Media { get; set; }
        public DbSet<Models.WebSpeech> WebSpeeches { get; set; }
        public DbSet<Models.ExecutionQueue> ExecutionQueues { get; set; }
        public DbSet<Models.Song> Songs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.User>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Models.Authentication>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Models.UserRoleType>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Models.UserRole>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Models.GoogleDriveAccount>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Models.GoogleDriveAuth>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Models.MediaConfiguration>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Models.Media>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Models.WebSpeech>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Models.ExecutionQueue>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Models.Song>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

        }
    }
}
