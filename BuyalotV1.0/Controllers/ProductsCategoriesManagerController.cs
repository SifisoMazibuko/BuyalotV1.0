using BuyalotV1._0.DbConnection;
using BuyalotV1._0.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BuyalotV1._0.Controllers
{
    public class ProductsCategoriesManagerController : Controller
    {
        private DataContext Context { get; set; }
        private bool _DisposeContext = false;


        public ProductsCategoriesManagerController()
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
        // GET: ProductCategories
        public ActionResult Index(string searchString)
        {
            var db = new DataContext();

            var product = (from p in db.ProductModelSet
                           join pc in db.ProductCategoryModelSet
                           on p.prodCategoryID equals pc.prodCategoryID
                           select p);


            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(s => s.productName.StartsWith(searchString)
                                       || s.productName.Contains(searchString)
                                       || s.vendor.StartsWith(searchString)
                                       || s.productDescription.StartsWith(searchString)
                                       || s.productDescription.Contains(searchString)
                                       || s.vendor.Contains(searchString)
                                       );

            }

            ViewBag.products = db.ProductModelSet.ToList();
            return View(product);
            //var db = new DataContext();

            //var product = (from p in db.ProductModelSet
            //               select p);


            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    product = product.Where(s => s.productName.StartsWith(searchString)
            //                           || s.productName.Contains(searchString)
            //                           || s.vendor.StartsWith(searchString)
            //                           || s.vendor.Contains(searchString));

            //}
            //ViewBag.products = db.ProductCategoryModelSet.ToList();
            //return View(product);
        }

        // GET: ProductCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategoryModel productCategory = Context.ProductCategoryModelSet.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }


        // GET: ProductCategories/Create
        public ActionResult Create()
        {
            ProductCategoryModel pro = new ProductCategoryModel();
            return View(pro);
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "prodCategoryID,categoryName")] ProductCategoryModel productCategory)
        {
            if (ModelState.IsValid)
            {
                Context.ProductCategoryModelSet.Add(productCategory);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productCategory);
        }

        // GET: ProductCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategoryModel productCategory = Context.ProductCategoryModelSet.Find(id);

            if (productCategory == null)
            {
                return HttpNotFound();
            }

            return View(productCategory);
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "prodCategoryID,categoryName")] ProductCategoryModel productCategory)
        {
            if (ModelState.IsValid)
            {
                Context.Entry(productCategory).State = EntityState.Modified;
                Context.SaveChanges();
                ViewBag.result = "Category " + productCategory.categoryName + " Updated Succesfully!";
                return View(productCategory);
            }

            return RedirectToAction("Index");
        }

        // GET: ProductCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategoryModel productCategory = Context.ProductCategoryModelSet.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductCategoryModel productCategory = Context.ProductCategoryModelSet.Find(id);
            Context.ProductCategoryModelSet.Remove(productCategory);
            Context.SaveChanges();
            ViewBag.result = "Category " + productCategory.categoryName + " Deleted Succesfully!";
            return View(productCategory);
        }

    }
}