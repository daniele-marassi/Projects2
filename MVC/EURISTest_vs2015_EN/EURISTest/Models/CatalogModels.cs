using EURIS.Entities;
using EURISTest.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EURISTest.Models
{
    public class CatalogModels: EURIS.Entities.Catalog
    {
        public List<string> SelectedProductsCodes { get; set; }

        public MultiSelectList ProductsCodesList { get; set; }
    }
}