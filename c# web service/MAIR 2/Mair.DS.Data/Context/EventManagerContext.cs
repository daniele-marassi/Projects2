using Mair.DS.Models.Entities.EventManager;
using Mair.DS.Models.Entities.EventManager.Actions;
using Mair.DS.Models.Entities.EventManager.Conditions;
using Microsoft.EntityFrameworkCore;

namespace Mair.DS.Data.Context
{
    public class EventManagerContext : DbContext
    {
        public EventManagerContext(DbContextOptions<EventManagerContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Action>()
                .HasDiscriminator(ba => ba.ActionTypeId)
                .HasValue<DbAction>(ActionType.Database)
                .HasValue<EmailAction>(ActionType.Email);

            //modelBuilder.Entity<DbAction>()
            //    .HasBaseType<BaseAction>();

            //modelBuilder.Entity<EmailAction>()
            //    .HasBaseType<BaseAction>();

        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<DbAction> DbActions { get; set; }
        public DbSet<DbActionDetail> DbActionDetails { get; set; }
        public DbSet<EmailAction> EmailActions { get; set; }
    }
}
