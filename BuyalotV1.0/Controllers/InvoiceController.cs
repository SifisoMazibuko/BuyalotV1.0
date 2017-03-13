using BuyalotV1._0.DbConnection;
using BuyalotV1._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuyalotV1._0.Controllers
{
    public class InvoiceController : Controller
    {
        private DataContext Context { get; set; }
        private bool _DisposeContext = false;


        public InvoiceController()
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
        // GET: Invoice
        public ActionResult Invoice()
        {
            InvoiceReceipt();
            int uid = Convert.ToInt32(Session["userID"]);


            if (uid > 0)
            {
                var invq = (from o in Context.OrderModelSet
                            where o.customerID == uid
                            join od in Context.OrderDetailsModelSet
                            on o.orderID equals od.orderID
                            join p in Context.ProductModelSet
                            on od.productID equals p.productID
                            // where o.customerID == uid
                            select new InvoiceViewModel
                            {                               
                                OrderID = o.orderID,
                                Date = DateTime.Now,
                                Amount = od.priceEach,
                                Items = p.productName
                            }).ToList();

                ViewBag.Invc = invq;

                // }
                //return RedirectToAction("Login", "Account");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [HttpPost]
        public ActionResult InvoiceReceipt()
        {

            int uid = Convert.ToInt32(Session["userID"]);
            if (Session["userID"] != null)
            {
                //get the orderId of the current user
                var getOrderID = (from o in Context.OrderModelSet
                                  where o.customerID == uid
                                  select o).ToList();
                int orderID = 0;
                foreach (var o in getOrderID)
                {

                    orderID = o.orderID;
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}