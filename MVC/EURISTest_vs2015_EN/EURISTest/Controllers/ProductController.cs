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
using System.Threading;
using System.Globalization;
using EURISTest.Common;

namespace EURISTest.Controllers
{
    public class ProductController : Controller
    {
        private ICatalogRepository catalogRepository;
        private IProductRepository productRepository;
        private IProductsCatalogRepository productsCatalogRepository;

        public ProductController()
        {
            this.catalogRepository = new CatalogRepository(new DAL.LocalDbContext());
            this.productRepository = new ProductRepository(new DAL.LocalDbContext());
            this.productsCatalogRepository = new ProductsCatalogRepository(new DAL.LocalDbContext());
        }

        //
        // GET: /Product/

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
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

            var products = from s in productRepository.GetProducts()
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Code.ToUpper().Contains(searchString.ToUpper().Trim())
                                       || s.Description.ToUpper().Contains(searchString.ToUpper().Trim()));
            }
            switch (sortOrder)
            {
                case "Description":
                    products = products.OrderBy(s => s.Description);
                    break;
                default:  // Code ascending 
                    products = products.OrderBy(s => s.Code);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Product/Details/5

        public ActionResult Details(string code = null)
        {
            Product product = productRepository.GetProductByCode(code);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            return View(new Product { Code =String.Empty, Description = String.Empty, Price = 0.0m });
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code, Description, Price")]Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var newId = productRepository.GetProducts().Select(_ => _.Id).Max() + 1;
                    //product.Id = newId;
                    productRepository.InsertProduct(product);
                    productRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(product);
        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(string code = null)
        {
            Product Product = productRepository.GetProductByCode(code);
            return View(Product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code, Description, Price")]Product product)
        {
            try
            {
                productRepository.UpdateProduct(product);
                productRepository.Save();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Product)entry.Entity;
                var databaseValues = (Product)entry.GetDatabaseValues().ToObject();

                if (databaseValues.Code != clientValues.Code)
                    ModelState.AddModelError("Code", "Current value: "
                        + databaseValues.Code);
                if (databaseValues.Description != clientValues.Description)
                    ModelState.AddModelError("Description", "Current value: "
                        + databaseValues.Description);
                if (databaseValues.Price != clientValues.Price)
                    ModelState.AddModelError("Price", "Current value: "
                        + databaseValues.Price);

                ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                    + "was modified by another user after you got the original value. The "
                    + "edit operation was canceled and the current values in the database "
                    + "have been displayed. If you still want to edit this record, click "
                    + "the Save button again. Otherwise click the Back to List hyperlink.");
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            return View(product);
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(string code, bool? concurrencyError)
        {
            Product product = productRepository.GetProductByCode(code);
            var countCatalogsToProduct = productsCatalogRepository.GetProductsCatalogs().Where(_ => _.ProductCode == code).Count();

            if (concurrencyError.GetValueOrDefault())
            {
                if (product == null)
                {
                    var message = "The record you attempted to delete "
                        + "was deleted by another user after you got the original values. "
                        + "Click the Back to List hyperlink.";
                    ViewBag.ConcurrencyErrorMessage = message;
                    ModelState.AddModelError(string.Empty, message);
                }
                else if (countCatalogsToProduct > 0)
                {
                    var message = "The record you attempted to delete "
                        + "is utilized. "
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

            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Product product)
        {
            try
            {
                var countCatalogsToProduct = productsCatalogRepository.GetProductsCatalogs().Where(_ => _.ProductCode == product.Code).Count();

                if (countCatalogsToProduct > 0)
                {
                    throw new DbUpdateConcurrencyException();
                }
                else
                {
                    productRepository.DeleteProduct(product.Code);
                    productRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { code = product.Code, concurrencyError = true });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(product);
            }
        }

        protected override void Dispose(bool disposing)
        {
            productRepository.Dispose();
            base.Dispose(disposing);
        }

    }
}
