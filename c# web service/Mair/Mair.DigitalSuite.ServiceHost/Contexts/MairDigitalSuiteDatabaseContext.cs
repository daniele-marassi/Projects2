using Mair.DigitalSuite.ServiceHost.Models.Base;
using Mair.DigitalSuite.ServiceHost.Models.Entities.Auth;
using Mair.DigitalSuite.ServiceHost.Models.Entities.Automation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Contexts
{
    public class MairDigitalSuiteDatabaseContext: DbContext
    {
        public MairDigitalSuiteDatabaseContext(DbContextOptions<MairDigitalSuiteDatabaseContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Authentication> Authentications { get; set; }
        public DbSet<UserRoleType> UserRoleTypes { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Models.Entities.Automation.Timer> Timers { get; set; }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is EntityBaseWithDates && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((EntityBaseWithDates)entityEntry.Entity).LastUpdated = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((EntityBaseWithDates)entityEntry.Entity).Created = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is EntityBaseWithDates && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((EntityBaseWithDates)entityEntry.Entity).LastUpdated = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((EntityBaseWithDates)entityEntry.Entity).Created = DateTime.Now;
                }
            }

            return base.SaveChangesAsync();
        }
        
    }
}
