using System;
using System.Data.Entity;

namespace Tools.Songs.Contexts
{
    public class SuppDatabaseContext : DbContext
    {
        public SuppDatabaseContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<Models.Songs> Songs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Models.Songs>()
            //    .Property(_ => _.InsDateTime)
            //    .HasColumnType("datetime");
        }
    }
}
