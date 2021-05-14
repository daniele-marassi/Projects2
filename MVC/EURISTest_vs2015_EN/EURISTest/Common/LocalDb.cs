using EURIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EURISTest.Common
{
    public class LocalDb
    {
        public void PopulateDb(EURISTest.DAL.LocalDbContext context)
        {
            var _catalogs = context.Catalogs;
            var _products = context.Products;
            var _productsCatalogs = context.ProductsCatalogs;

            if (_catalogs.Count() > 0 || _products.Count() > 0 || _productsCatalogs.Count() > 0) return;

            #region Create default data

            var products = new List<Product>
            {
                new Product{Code = "Product1", Description = "Product 1", Price = 1111.11m },
                new Product{Code = "Product2", Description = "Product 2", Price = 2222.11m },
                new Product{Code = "Product3", Description = "Product 3", Price = 3333.11m }
            };
            products.ForEach(_ => context.Products.Add(_));
            context.SaveChanges();

            var catalogs = new List<Catalog>
            {
                new Catalog{Code = "Catalog1", Description = "Catalog 1"},
                new Catalog{Code = "Catalog2", Description = "Catalog 2"},
                new Catalog{Code = "Catalog3", Description = "Catalog 3"},
                new Catalog{Code = "Catalog4", Description = "Catalog 4"}
            };
            catalogs.ForEach(_ => context.Catalogs.Add(_));
            context.SaveChanges();

            var productsCatalogs = new List<ProductsCatalog>
            {
                new ProductsCatalog{Id = 1, CatalogCode = "Catalog1", ProductCode = "Product1" },
                new ProductsCatalog{Id = 2, CatalogCode = "Catalog1", ProductCode = "Product2" },
                new ProductsCatalog{Id = 3, CatalogCode = "Catalog1", ProductCode = "Product3" },
                new ProductsCatalog{Id = 4, CatalogCode = "Catalog2", ProductCode = "Product1" },
                new ProductsCatalog{Id = 5, CatalogCode = "Catalog2", ProductCode = "Product2" },
                new ProductsCatalog{Id = 6, CatalogCode = "Catalog3", ProductCode = "Product2" },
                new ProductsCatalog{Id = 7, CatalogCode = "Catalog4", ProductCode = "Product1" },
                new ProductsCatalog{Id = 8, CatalogCode = "Catalog4", ProductCode = "Product3" },
            };                                                     
            productsCatalogs.ForEach(_ => context.ProductsCatalogs.Add(_));
            context.SaveChanges();

            #endregion
        }
    }
}