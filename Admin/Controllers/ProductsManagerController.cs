using Admin.DbConnection;
using Admin.Models;
using Admin.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    
    public class ProductsManagerController : Controller
    {
        
        Repository01 _repository = new Repository01();
        private DataContext Context { get; set; }
        private bool _DisposeContext = false;

        public ProductsManagerController()
        {
            Context = new DataContext();
            _DisposeContext = true;
        }
        protected override void Dispose(bool disposing)
        {
            if (_DisposeContext)
                Context.Dispose();

            base.Dispose(disposing);

        }



        //[Authorize(Roles = "Admin")]
        // GET: ProductsManager
        public ActionResult Index()
        {
            //ViewBag.prodCategoryID = new SelectList(Context.ProductCategoryModelSet, "prodCategoryID", "categoryName");
            var product = (from p in Context.ProductModelSet
                           select p).ToList();
            foreach (var item in product)
            {
                Session["ProdCount"] = product.Count;
            }
            return View(product);
        }

        // GET: ProductsManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel product = Context.ProductModelSet.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        public ActionResult ViewProducts()
        {
            ViewBag.prodCategoryID = new SelectList(Context.ProductCategoryModelSet, "prodCategoryID", "categoryName");
            var product = (from p in Context.ProductModelSet
                           select p).ToList();
            return View(product);
        }
        // GET: ProductsManager/Create
        public ActionResult Create()
        {
            ViewData["Categories"] = _repository.GetCategories();
            ViewBag.prodCategoryID = new SelectList(Context.ProductCategoryModelSet, "prodCategoryID", "categoryName");
            ProductModel pro = new ProductModel();
            return View(pro);
        }

        // POST: ProductsManager/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, ProductModel product, HttpPostedFileBase upload)
        {
            var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                     .ToArray();

            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    product.productImage = new byte[upload.ContentLength];
                    upload.InputStream.Read(product.productImage, 0, upload.ContentLength);
                }
                Context.ProductModelSet.Add(product);
                Context.SaveChanges();
                ViewBag.result = "Product " + product.vendor + " " + product.productName + " Added Succesfully!";
                ViewBag.prodCategoryID = new SelectList(Context.ProductCategoryModelSet, "prodCategoryID", "categoryName", product.productID);

                return RedirectToAction("ViewProducts", "Product");
            }

            return View(product);

        }

        // GET: ProductsManager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.prodCategoryID = new SelectList(Context.ProductCategoryModelSet, "prodCategoryID", "categoryName");
            ProductModel product = Context.ProductModelSet.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductsManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "productID,productImage")] */ProductModel product, FormCollection collection, HttpPostedFileBase upload)
        {
            try
            {
                // TODO: Add update logic here

                if (ModelState.IsValid)
                {
                    if (upload != null)
                    {
                        product.productImage = new byte[upload.ContentLength];
                        upload.InputStream.Read(product.productImage, 0, upload.ContentLength);
                    }

                    Context.Entry(product).State = EntityState.Modified;
                    Context.SaveChanges();
                    ViewBag.result = "Product " + product.vendor + " " + product.productName + " Updated Succesfully!";
                    ViewBag.prodCategoryID = new SelectList(Context.ProductCategoryModelSet, "prodCategoryID", "categoryName", product.productID);
                    return View(product);

                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error", "Error");
            }
        }

        // GET: ProductsManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductModel product = Context.ProductModelSet.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductsManager/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection, HttpPostedFileBase upload)
        {
            try
            {

                ProductModel product = Context.ProductModelSet.Find(id);
                if (upload != null)
                {
                    product.productImage = new byte[upload.ContentLength];
                    upload.InputStream.Read(product.productImage, 0, upload.ContentLength);
                }

                Context.ProductModelSet.Remove(product);
                Context.SaveChanges();
                ViewBag.result = "Product " + product.vendor + " " + product.productName + " Deleted Succesfully!";

                return RedirectToAction("Index");
            }

            catch
            {
                return RedirectToAction("Error", "Error");
            }
        }
    }
}