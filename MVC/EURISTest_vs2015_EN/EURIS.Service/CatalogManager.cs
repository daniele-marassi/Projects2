using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EURIS.Entities;
using System.Data.Entity;

namespace EURIS.Service
{
    public class CatalogManager
    {
        LocalDbEntities context = new LocalDbEntities();

        public List<Catalog> GetCatalogs()
        {
            List<Catalog> catalogs = new List<Catalog>();

            catalogs = (from item in context.Catalog
                        select item).ToList();

            return catalogs;
        }
    }
}
