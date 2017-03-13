using Admin.DbConnection;
using Admin.Models;
using Admin.Provider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    
    public class CustomerController : Controller
    {
        
        private DataContext db = new DataContext();

        // GET: Customer
        public ActionResult Index()
        {
            var customer = (from c in db.CustomerModelSet
                            select c).ToList();
            ViewBag.me = customer.Count;

            foreach (var item in customer)
            {
                Session["CusCount"] = customer.Count;
            }

            return View(customer);
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerModel customerModel = db.CustomerModelSet.Find(id);
            if (customerModel == null)
            {
                return HttpNotFound();
            }
            return View(customerModel);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
           return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "customerID,firstName,lastName,phone,email,password,confirmPassword,state")] CustomerModel customerModel)
        {
            if (ModelState.IsValid)
            {
                customerModel.password = Cipher.Encrypt(customerModel.password);
                customerModel.confirmPassword = Cipher.Encrypt(customerModel.confirmPassword);
                customerModel.state = "Active";
                db.CustomerModelSet.Add(customerModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                customerModel.state = "Inactive";
            return View(customerModel);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerModel customerModel = db.CustomerModelSet.Find(id);
            if (customerModel == null)
            {
                return HttpNotFound();
            }
            return View(customerModel);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "customerID,firstName,lastName,phone,email,password,confirmPassword,state")] CustomerModel customerModel)
        {
            if (ModelState.IsValid)
            {
                customerModel.password = Cipher.Encrypt(customerModel.password);
                customerModel.confirmPassword = Cipher.Encrypt(customerModel.confirmPassword);
                customerModel.state = "Active";
                db.Entry(customerModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                customerModel.state = "Inactive";
            return View(customerModel);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerModel customerModel = db.CustomerModelSet.Find(id);
            if (customerModel == null)
            {
                return HttpNotFound();
            }
            return View(customerModel);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerModel customerModel = db.CustomerModelSet.Find(id);
            db.CustomerModelSet.Remove(customerModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}