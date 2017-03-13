using Admin.DbConnection;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class OrderController : Controller
    {
            private DataContext Context { get; set; }
            private bool _DisposeContext = false;


            public OrderController()
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


            if (uid > 0)
            {
                var query = (from o in Context.OrderModelSet
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
                             }).ToList();

                ViewBag.OrderList = query;

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

                ViewData["OrderCount"] = orderCount;
                Session["OrderCount"] = orderCount;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
    }
}