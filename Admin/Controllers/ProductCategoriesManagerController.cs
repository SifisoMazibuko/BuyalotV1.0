using Admin.DbConnection;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    
    public class ProductCategoriesManagerController : Controller
    {
        
        private DataContext Context { get; set; }
        private bool _DisposeContext = false;


        public ProductCategoriesManagerController()
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
        public ActionResult Index()
        {
            var category = (from c in Context.ProductCategoryModelSet
                            select c.prodCategoryID).ToList();
            foreach (var item in category)
            {
                Session["CatCount"] = category.Count;
            }
            
            return View(Context.ProductCategoryModelSet.ToList());
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

        //[Authorize(Roles = "Admin")]
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
            //    productCategory.adminID = 1;
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