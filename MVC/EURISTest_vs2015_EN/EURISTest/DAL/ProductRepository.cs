
using EURIS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EURISTest.DAL
{
    public class ProductRepository : IProductRepository, IDisposable
    {
        private DAL.LocalDbContext context;

        public ProductRepository(DAL.LocalDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Product> GetProducts()
        {
            return context.Products.ToList();
        }

        public Product GetProductByCode(string code)
        {
            return context.Products.Find(code);
        }

        public void InsertProduct(Product product)
        {
            context.Products.Add(product);
        }

        public void DeleteProduct(string code)
        {
            Product product = context.Products.Find(code);
            context.Products.Remove(product);
        }

        public void UpdateProduct(Product product)
        {
            context.Entry(product).State = EntityState.Modified;
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