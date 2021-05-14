
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Configuration;
using EURIS.Entities;

namespace EURISTest.DAL
{
    public class LocalDbContext : DbContext
    {

        public LocalDbContext() : base("LocalDbEntities")
        {
            Database.SetInitializer<LocalDbContext>(new LocalDbInitializer());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<ProductsCatalog> ProductsCatalogs { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}