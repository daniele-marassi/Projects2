using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EURISTest.Models
{
    public class ProductModels : EURIS.Entities.Product
    {
        public string CodeAndDescription { get; set; }
    }
}