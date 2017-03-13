using Admin.DbConnection;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    
    public class HomeController : Controller
    {
        private DataContext Context { get; set; }
        private bool _DisposeContext = false;

        public HomeController()
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
        public ActionResult Index()
        {
            int uid = Convert.ToInt32(Session["userID"]);

            var customer = (from c in Context.CustomerModelSet
                            select c).ToList();
            Session["CusCount"] = customer.Count;

            var category = (from c in Context.ProductCategoryModelSet
                            select c.prodCategoryID).ToList();
            Session["CatCount"] = category.Count;

            var prod = (from c in Context.ProductModelSet
                           select c.productID).ToList();
            Session["ProdCount"] = prod.Count;

            var orderCount = (from o in Context.OrderModelSet
                              where o.customerID == uid
                              join od in Context.OrderDetailsModelSet
                              on o.orderID equals od.orderID
                              join p in Context.ProductModelSet
                              on od.productID equals p.productID
                              select new OrderList
                              {
                                  customerID = o.customerID,
                                  orderDate = o.orderDate,
                                  shippingDate = o.shippingDate,
                                  shippingAddress = o.shippingAddress,
                                  status = o.status,
                                  quantityOrdered = od.quantityOrdered,
                                  priceEach = od.priceEach,
                                  productName = p.productName
                              }).Count();

            Session["OrderCount"] = orderCount;

            var subscriber = (from c in Context.SubscribeModelSet
                               select c.subscribeID).ToList();
            Session["SubCount"] = subscriber.Count;

            ViewBag.prodCategoryID = new SelectList(Context.ProductCategoryModelSet, "prodCategoryID", "categoryName");
            var product = (from p in Context.ProductModelSet
                           select p).ToList();
            return View(product);
        }

        public ActionResult Create()
        {
            
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

    }
}