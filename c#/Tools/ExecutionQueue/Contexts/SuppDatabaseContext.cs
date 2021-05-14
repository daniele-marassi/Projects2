using System.Data.Entity;

namespace Tools.ExecutionQueue.Contexts
{
    public class SuppDatabaseContext : DbContext
    {
        public SuppDatabaseContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<Models.ExecutionQueue> ExecutionQueues { get; set; }
    }
}
