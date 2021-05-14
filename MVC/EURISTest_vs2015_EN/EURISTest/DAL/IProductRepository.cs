using EURIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EURISTest.DAL
{
    public interface IProductRepository : IDisposable
    {
        IEnumerable<Product> GetProducts();
        Product GetProductByCode(string code);
        void InsertProduct(Product product);
        void DeleteProduct(string code);
        void UpdateProduct(Product product);
        void Save();
    }
}