using EURIS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EURISTest.DAL
{
    public class ProductsCatalogRepository : IProductsCatalogRepository, IDisposable
    {
        private DAL.LocalDbContext context;

        public ProductsCatalogRepository(DAL.LocalDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<ProductsCatalog> GetProductsCatalogs()
        {
            return context.ProductsCatalogs.ToList();
        }

        public ProductsCatalog GetProductsCatalogByID(int id)
        {
            return context.ProductsCatalogs.Find(id);
        }

        public void InsertProductsCatalog(List<ProductsCatalog> productsCatalogs)
        {
            foreach (var productsCatalog in productsCatalogs)
            {
                context.ProductsCatalogs.Add(productsCatalog);
            }
        }

        public void DeleteProductsCatalog(int id)
        {
            ProductsCatalog ProductsCatalog = context.ProductsCatalogs.Find(id);
            context.ProductsCatalogs.Remove(ProductsCatalog);
        }

        public void DeleteProductsCatalogByCatalogCode(string catalogCode)
        {
            var productsCatalogs = context.ProductsCatalogs.Where(_ => _.CatalogCode == catalogCode).ToList();
            ProductsCatalog productsCatalog = new ProductsCatalog() { };
            foreach (var item in productsCatalogs)
            {
                productsCatalog = context.ProductsCatalogs.Find(item.Id);
                context.ProductsCatalogs.Remove(productsCatalog);
            }
        }

        public void UpdateProductsCatalog(ProductsCatalog ProductsCatalog)
        {
            context.Entry(ProductsCatalog).State = EntityState.Modified;
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