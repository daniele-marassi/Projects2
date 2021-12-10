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

        public DbSet<SuppModels.User> Users { get; set; }
        public DbSet<SuppModels.Authentication> Authentications { get; set; }
        public DbSet<SuppModels.UserRoleType> UserRoleTypes { get; set; }
        public DbSet<SuppModels.UserRole> UserRoles { get; set; }
        public DbSet<SuppModels.GoogleAccount> GoogleAccounts { get; set; }
        public DbSet<SuppModels.GoogleAuth> GoogleAuths { get; set; }
        public DbSet<SuppModels.MediaConfiguration> MediaConfigurations { get; set; }
        public DbSet<SuppModels.Media> Media { get; set; }
        public DbSet<SuppModels.WebSpeech> WebSpeeches { get; set; }
        public DbSet<SuppModels.ExecutionQueue> ExecutionQueues { get; set; }
        public DbSet<SuppModels.Song> Songs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SuppModels.User>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SuppModels.Authentication>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SuppModels.UserRoleType>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SuppModels.UserRole>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SuppModels.GoogleAccount>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SuppModels.GoogleAuth>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SuppModels.MediaConfiguration>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SuppModels.Media>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SuppModels.WebSpeech>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SuppModels.ExecutionQueue>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SuppModels.Song>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

        }
    }
}
