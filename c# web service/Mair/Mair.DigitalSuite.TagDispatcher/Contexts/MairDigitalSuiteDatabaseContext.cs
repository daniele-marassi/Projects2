using Mair.DigitalSuite.TagDispatcher.Models.Entities.Automation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.TagDispatcher.Contexts
{
    public class MairDigitalSuiteDatabaseContext : DbContext
    {
        private static string _conectionString = String.Empty;
        public MairDigitalSuiteDatabaseContext(string conectionString)
        {
            _conectionString = conectionString;
        }

        public DbSet<PlcData> PlcData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conectionString);
        }
    }

}
