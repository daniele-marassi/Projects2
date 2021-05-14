using Mair.DS.Models.Base;
using Mair.DS.Models.Entities;
using Mair.DS.Models.Entities.Auth;
using Mair.DS.Models.Entities.Automation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Mair.DS.Data.Context
{
    public class AutomationContext : DbContext
    {
        public AutomationContext(DbContextOptions<AutomationContext> options)
            : base(options)
        {

        }

        public override int SaveChanges()
        {

            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is EntityBaseWithDates 
                    && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((EntityBaseWithDates)entityEntry.Entity).LastUpdated = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                    ((EntityBaseWithDates)entityEntry.Entity).Created = DateTime.Now;
            }
            
            return base.SaveChanges();
        }

        public DbSet<Node> Nodes { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<SimulatedConnector> SimulatedConnector { get; set; }

        public DbSet<Authentication> Authentications { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<RolePath> RolePaths { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }
    }
}
