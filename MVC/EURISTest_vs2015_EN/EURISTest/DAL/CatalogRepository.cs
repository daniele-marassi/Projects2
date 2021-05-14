
using EURIS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EURISTest.DAL
{
    public class CatalogRepository : ICatalogRepository, IDisposable
    {
        private DAL.LocalDbContext context;

        public CatalogRepository(DAL.LocalDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Catalog> GetCatalogs()
        {
            return context.Catalogs.ToList();
        }

        public Catalog GetCatalogByCode(string code)
        {
            return context.Catalogs.Find(code);
        }

        public void InsertCatalog(Catalog catalog)
        {
            context.Catalogs.Add(catalog);
        }

        public void DeleteCatalog(string code)
        {
            Catalog catalog = context.Catalogs.Find(code);
            context.Catalogs.Remove(catalog);
        }

        public void UpdateCatalog(Catalog catalog)
        {
            context.Entry(catalog).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}