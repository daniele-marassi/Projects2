
using EURISTest.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EURISTest.DAL
{
    public class LocalDbInitializer : IDatabaseInitializer<LocalDbContext>
    {
        public void InitializeDatabase(LocalDbContext context)
        {
            LocalDb localDb = new LocalDb();
            localDb.PopulateDb(context);
        }
    }
}