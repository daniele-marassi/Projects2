using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EURIS.Service;
using EURIS.Entities;
using System.Data;
using EURISTest.DAL;
using PagedList;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace EURISTest.Controllers
{
    public class CatalogController : Controller
    {
        private ICatalogRepository catalogRepository;
        private IProductRepository productRepository;
        private IProductsCatalogRepository productsCatalogRepository;

        public CatalogController()
        {
            this.catalogRepository = new CatalogRepository(new DAL.LocalDbContext());
            this.productRepository = new ProductRepository(new DAL.LocalDbContext());
            this.productsCatalogRepository = new ProductsCatalogRepository(new DAL.LocalDbContext());
        }

        //
        // GET: /Catalog/

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page, bool? concurrencyError = false)
        {
            if (concurrencyError.GetValueOrDefault())
            {
                var message = "No data, action not allowed";
                ViewBag.ConcurrencyErrorMessage = message;
                ModelState.AddModelError(string.Empty, message);
            }
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Description" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var Catalogs = from s in catalogRepository.GetCatalogs()
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                Catalogs = Catalogs.Where(s => s.Code.ToUpper().Contains(searchString.ToUpper().Trim())
                                       || s.Description.ToUpper().Contains(searchString.ToUpper().Trim()));
            }
            switch (sortOrder)
            {
                case "Description":
                    Catalogs = Catalogs.OrderBy(s => s.Description);
                    break;
                default:  // Code ascending 
                    Catalogs = Catalogs.OrderBy(s => s.Code);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(Catalogs.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Catalog/Details/5

        public ActionResult Details(string code = null)
        {
            Catalog catalog = catalogRepository.GetCatalogByCode(code);
            if (catalog == null)
            {
                return HttpNotFound();
            }

            return View(catalog);
        }

        //
        // GET: /Catalog/Create

        public ActionResult Create()
        {
            EURISTest.Models.CatalogModels _catalog = new EURISTest.Models.CatalogModels() { };
            try
            {
                Catalog catalog = new Catalog() { Code = String.Empty, Description = String.Empty, ProductsCatalog = null };

                IProductRepository productRepository = new ProductRepository(new DAL.LocalDbContext());

                var selectedProducts = new List<string>() { };

                var productModels = new List<Models.ProductModels>() { };
                var products = productRepository.GetProducts().ToList().OrderBy(_ => _.Code);

                foreach (var product in products)
                {
                    var descriptionLength = product.Description.Length;
                    var lenght = 0;
                    var codeAndDescription = String.Empty;
                    if (descriptionLength > 50)
                    {
                        lenght = 50;
                        codeAndDescription = product.Code + " - " + product.Description.Substring(0, lenght) + "...";
                    }
                    else
                    {
                        lenght = descriptionLength;
                        codeAndDescription = product.Code + " - " + product.Description.Substring(0, lenght);
                    }
                    productModels.Add(new Models.ProductModels() { Code = product.Code, Description = product.Description, ProductsCatalog = product.ProductsCatalog, CodeAndDescription = codeAndDescription });
                }

                MultiSelectList productsCodesList = new MultiSelectList(productModels, "Code", "CodeAndDescription", selectedProducts);

                _catalog.Code = catalog.Code;
                _catalog.Description = catalog.Description;
                _catalog.ProductsCodesList = productsCodesList;
                _catalog.ProductsCatalog = catalog.ProductsCatalog;
                _catalog.SelectedProductsCodes = selectedProducts;

            }
            catch (DataException ex)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                return RedirectToAction("Index", new { concurrencyError = true });
            }
            return View(_catalog);
        }

        //
        // POST: /Catalog/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code, Description, SelectedProductsCodes, CodeAndDescription")]EURISTest.Models.CatalogModels catalog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (catalog.Code == null || catalog.Code == String.Empty)
                    {
                        throw new DataException();
                    }

                    var productModels = new List<Models.ProductModels>() { };
                    var products = productRepository.GetProducts().ToList().OrderBy(_ => _.Code);

                    foreach (var product in products)
                    {
                        var descriptionLength = catalog.Description.Length;
                        var lenght = 0;
                        var codeAndDescription = String.Empty;
                        if (descriptionLength > 50)
                        {
                            lenght = 50;
                            codeAndDescription = product.Code + " - " + product.Description.Substring(0, lenght) + "...";
                        }
                        else
                        {
                            lenght = descriptionLength;
                            codeAndDescription = product.Code + " - " + product.Description.Substring(0, lenght);
                        }
                        productModels.Add(new Models.ProductModels() { Code = product.Code, Description = product.Description, ProductsCatalog = product.ProductsCatalog, CodeAndDescription = codeAndDescription });
                        
                    }

                    MultiSelectList productsCodesList = new MultiSelectList(productModels, "Code", "CodeAndDescription", catalog.SelectedProductsCodes);

                    catalog.ProductsCodesList = productsCodesList;

                    Catalog _catalog = new Catalog() { };

                    _catalog.Code = catalog.Code;
                    _catalog.Description = catalog.Description;
                    _catalog.ProductsCatalog = catalog.ProductsCatalog;

                    catalogRepository.InsertCatalog(_catalog);
                    catalogRepository.Save();

                    productsCatalogRepository.DeleteProductsCatalogByCatalogCode(catalog.Code);
                    productsCatalogRepository.Save();

                    List<ProductsCatalog> productsCatalogs = new List<ProductsCatalog>() { };
                    var productsCatalogIds = productsCatalogRepository.GetProductsCatalogs().Select(_ => _.Id).ToList();
                    var lastProductsCatalogId = productsCatalogIds.Count > 0 ? productsCatalogIds.Max() : 0;
                    if (catalog.SelectedProductsCodes != null)
                    {
                        foreach (var producteCode in catalog.SelectedProductsCodes)
                        {
                            lastProductsCatalogId++;
                            productsCatalogs.Add(new ProductsCatalog { Id = (lastProductsCatalogId), CatalogCode = catalog.Code, ProductCode = producteCode });
                        }

                        productsCatalogRepository.InsertProductsCatalog(productsCatalogs);
                        productsCatalogRepository.Save();
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(catalog);
        }

        //
        // GET: /Catalog/Edit/5

        public ActionResult Edit(string code = null)
        {
            Catalog catalog = catalogRepository.GetCatalogByCode(code);

            EURISTest.Models.CatalogModels _catalog = new EURISTest.Models.CatalogModels() { };

            IProductRepository productRepository = new ProductRepository(new DAL.LocalDbContext());

            var selectedProducts = catalog.ProductsCatalog.Select(_ => _.ProductCode).ToList();

            var productModels = new List<Models.ProductModels>() { };
            var products = productRepository.GetProducts().ToList().OrderBy(_ => _.Code);

            foreach (var product in products)
            {
                var descriptionLength = product.Description.Length;
                var lenght = 0;
                var codeAndDescription = String.Empty;
                if (descriptionLength > 50)
                {
                    lenght = 50;
                    codeAndDescription = product.Code + " - " + product.Description.Substring(0, lenght) + "...";
                }
                else
                {
                    lenght = descriptionLength;
                    codeAndDescription = product.Code + " - " + product.Description.Substring(0, lenght);
                }
                productModels.Add(new Models.ProductModels() { Code = product.Code, Description = product.Description, ProductsCatalog = product.ProductsCatalog, CodeAndDescription = codeAndDescription });
            }

            MultiSelectList productsCodesList = new MultiSelectList(productModels, "Code", "CodeAndDescription", selectedProducts);

            _catalog.Code = catalog.Code;
            _catalog.Description = catalog.Description;
            _catalog.ProductsCodesList = productsCodesList;
            _catalog.ProductsCatalog = catalog.ProductsCatalog;
            _catalog.SelectedProductsCodes = selectedProducts;

            return View(_catalog);
        }

        //
        // POST: /Catalog/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code, Description, SelectedProductsCodes, CodeAndDescription")]EURISTest.Models.CatalogModels catalog)
        {
            try
            {
                if (catalog.Code == null || catalog.Code == String.Empty)
                {
                    throw new DataException();
                }

                Catalog _catalog = new Catalog() { };

                _catalog.Code = catalog.Code;
                _catalog.Description = catalog.Description;
                _catalog.ProductsCatalog = catalog.ProductsCatalog;

                catalogRepository.UpdateCatalog(_catalog);
                catalogRepository.Save();

                productsCatalogRepository.DeleteProductsCatalogByCatalogCode(catalog.Code);
                productsCatalogRepository.Save();

                List<ProductsCatalog> productsCatalogs = new List<ProductsCatalog>() {  };
                var productsCatalogIds = productsCatalogRepository.GetProductsCatalogs().Select(_ => _.Id).ToList();
                var lastProductsCatalogId = productsCatalogIds.Count > 0 ? productsCatalogIds.Max() : 0;
                if (catalog.SelectedProductsCodes != null)
                {
                    foreach (var producteCode in catalog.SelectedProductsCodes)
                    {
                        lastProductsCatalogId++;
                        productsCatalogs.Add(new ProductsCatalog { Id = (lastProductsCatalogId), CatalogCode = catalog.Code, ProductCode = producteCode });
                    }

                    productsCatalogRepository.InsertProductsCatalog(productsCatalogs);
                    productsCatalogRepository.Save();
                }

                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Catalog)entry.Entity;
                var databaseValues = (Catalog)entry.GetDatabaseValues().ToObject();

                if (databaseValues.Code != clientValues.Code)
                    ModelState.AddModelError("Code", "Current value: "
                        + databaseValues.Code);
                if (databaseValues.Description != clientValues.Description)
                    ModelState.AddModelError("Description", "Current value: "
                        + databaseValues.Description);
                ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                    + "was modified by another user after you got the original value. The "
                    + "edit operation was canceled and the current values in the database "
                    + "have been displayed. If you still want to edit this record, click "
                    + "the Save button again. Otherwise click the Back to List hyperlink.");
            }
            catch (DataException ex /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            return View(catalog);
        }

        //
        // GET: /Catalog/Delete/5

        public ActionResult Delete(string code, bool? concurrencyError)
        {
            Catalog catalog = catalogRepository.GetCatalogByCode(code);

            if (concurrencyError.GetValueOrDefault())
            {
                if (catalog == null)
                {
                    var message = "The record you attempted to delete "
                        + "was deleted by another user after you got the original values. "
                        + "Click the Back to List hyperlink.";
                    ViewBag.ConcurrencyErrorMessage = message;
                    ModelState.AddModelError(string.Empty, message);
                }
                else
                {
                    var message = "The record you attempted to delete "
                        + "was modified by another user after you got the original values. "
                        + "The delete operation was canceled and the current values in the "
                        + "database have been displayed. If you still want to delete this "
                        + "record, click the Delete button again. Otherwise "
                        + "click the Back to List hyperlink.";
                    ViewBag.ConcurrencyErrorMessage = message;
                    ModelState.AddModelError(string.Empty, message);
                }
            }

            return View(catalog);
        }

        //
        // POST: /Catalog/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Catalog catalog)
        {
            try
            {
                catalogRepository.DeleteCatalog(catalog.Code);
                catalogRepository.Save();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(catalog);
            }
        }

        protected override void Dispose(bool disposing)
        {
            catalogRepository.Dispose();
            base.Dispose(disposing);
        }

    }
}
