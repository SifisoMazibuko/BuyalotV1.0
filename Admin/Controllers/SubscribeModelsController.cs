using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Admin.Models;
using Admin.DbConnection;

namespace Admin.Controllers
{
    public class SubscribeModelsController : Controller
    {
        private DataContext Context { get; set; }
        private bool _DisposeContext = false;


        public SubscribeModelsController()
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
        // GET: SubscribeModels
        public ActionResult Index()
        {           
            return View(Context.SubscribeModelSet.ToList());
        }

        // GET: SubscribeModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscribeModel subscribeModel = Context.SubscribeModelSet.Find(id);
            if (subscribeModel == null)
            {
                return HttpNotFound();
            }
            return View(subscribeModel);
        }

        // GET: SubscribeModels/Create
        public ActionResult Create()
        {
            WebEmailService ws = new WebEmailService();
            ws.Subscribe();

           // return RedirectToAction("Create", "SubscribeModels");
            return View();
        }

        // POST: SubscribeModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "subscribeID,firstName,email")] SubscribeModel subscribeModel)
        {
            if (ModelState.IsValid)
            {             

                Context.SubscribeModelSet.Add(subscribeModel);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subscribeModel);
        }

        // GET: SubscribeModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscribeModel subscribeModel = Context.SubscribeModelSet.Find(id);
            if (subscribeModel == null)
            {
                return HttpNotFound();
            }
            return View(subscribeModel);
        }

        // POST: SubscribeModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "subscribeID,firstName,email")] SubscribeModel subscribeModel)
        {
            if (ModelState.IsValid)
            {
                Context.Entry(subscribeModel).State = EntityState.Modified;
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subscribeModel);
        }

        // GET: SubscribeModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscribeModel subscribeModel = Context.SubscribeModelSet.Find(id);
            if (subscribeModel == null)
            {
                return HttpNotFound();
            }
            return View(subscribeModel);
        }

        // POST: SubscribeModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubscribeModel subscribeModel = Context.SubscribeModelSet.Find(id);
            Context.SubscribeModelSet.Remove(subscribeModel);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }
        
    }
}
