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

        public DbSet<Supp.Models.User> Users { get; set; }
        public DbSet<Supp.Models.Authentication> Authentications { get; set; }
        public DbSet<Supp.Models.UserRoleType> UserRoleTypes { get; set; }
        public DbSet<Supp.Models.UserRole> UserRoles { get; set; }
        public DbSet<Supp.Models.GoogleAccount> GoogleAccounts { get; set; }
        public DbSet<Supp.Models.GoogleAuth> GoogleAuths { get; set; }
        public DbSet<Supp.Models.MediaConfiguration> MediaConfigurations { get; set; }
        public DbSet<Supp.Models.Media> Media { get; set; }
        public DbSet<Supp.Models.WebSpeech> WebSpeeches { get; set; }
        public DbSet<Supp.Models.ExecutionQueue> ExecutionQueues { get; set; }
        public DbSet<Supp.Models.Song> Songs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supp.Models.User>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Supp.Models.Authentication>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Supp.Models.UserRoleType>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Supp.Models.UserRole>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Supp.Models.GoogleAccount>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Supp.Models.GoogleAuth>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Supp.Models.MediaConfiguration>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Supp.Models.Media>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Supp.Models.WebSpeech>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Supp.Models.ExecutionQueue>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Supp.Models.Song>()
                .Property(b => b.InsDateTime)
                .HasDefaultValueSql("getdate()");

        }
    }
}
