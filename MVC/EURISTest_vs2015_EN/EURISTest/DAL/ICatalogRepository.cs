using EURIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EURISTest.DAL
{
    public interface ICatalogRepository : IDisposable
    {
        IEnumerable<Catalog> GetCatalogs();
        Catalog GetCatalogByCode(string code);
        void InsertCatalog(Catalog catalog);
        void DeleteCatalog(string code);
        void UpdateCatalog(Catalog catalog);
        void Save();
    }
}