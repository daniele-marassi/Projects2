using EURIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EURISTest.DAL
{
    public interface IProductsCatalogRepository : IDisposable
    {
        IEnumerable<ProductsCatalog> GetProductsCatalogs();
        ProductsCatalog GetProductsCatalogByID(int id);
        void InsertProductsCatalog(List<ProductsCatalog> productsCatalogs);
        void DeleteProductsCatalog(int id);
        void UpdateProductsCatalog(ProductsCatalog ProductsCatalog);
        void DeleteProductsCatalogByCatalogCode(string CatalogCode);
        void Save();
    }
}