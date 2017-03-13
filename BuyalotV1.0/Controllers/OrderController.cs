using BuyalotV1._0.DbConnection;
using BuyalotV1._0.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuyalotV1._0.Controllers
{
    public class OrderController : Controller
    {
        DataContext db = new DataContext();
        // GET: Order
        public ActionResult Index()
        {
            CheckOuts();
           
            int uid = Convert.ToInt32(Session["userID"]);
           
            
            if(uid > 0)
            {
                var query = (from o in db.OrderModelSet
                             where o.customerID == uid
                             join od in db.OrderDetailsModelSet
                             on o.orderID equals od.orderID
                             join p in db.ProductModelSet
                             on od.productID equals p.productID
                             // where o.customerID == uid
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
        [ValidateAntiForgeryToken]
        public ActionResult CheckOuts()
        {

            try
            {
                /*
            * Check if the user has logged on and redirect the user to login if not so
            * Send the user to registration page if not registered
            * Proceed to checkout if all is well
            */
                int uid = Convert.ToInt32(Session["userID"]);

                //Gettting the cuurrent shopping cart
                var currentShoppingCart = Session["Cart"];
                decimal totalprice = 0;

                if (Session["userID"] != null)
                {
                    
                    OrderModel newOrder = new OrderModel();
                    newOrder.customerID = uid;
                    newOrder.orderDate = DateTime.Now;

                    DateTime shippingDate = AddWorkdays(DateTime.Now, 5);
                    newOrder.shippingDate = shippingDate;


                    //get the customer address
                    var getAddress = (from a in db.AddressModelSet
                                      where a.customerID == uid
                                      select a).ToList();


                    foreach (var address in getAddress)
                    {
                        string addr = address.address;
                        string city = address.city;
                        string postCode = address.postalCode;

                        newOrder.shippingAddress = addr + ", " + city + ", " + postCode;
                    }
                    //order.shippingAddress = 
                    newOrder.status = "In Process";
                    //get total price
                    foreach (var item in (List<Item>)currentShoppingCart)
                    {
                        totalprice = totalprice + (item.Prdcts.price + item.Quantity);
                        newOrder.totalPrice = totalprice;
                    }
                    db.OrderModelSet.Add(newOrder);
                    db.SaveChanges();




                    //get the orderId of the current user
                    var getOrderID = (from o in db.OrderModelSet
                                      where o.customerID == uid
                                      select o).ToList();
                    int orderID = 0;
                    foreach (var o in getOrderID)
                    {

                        orderID = o.orderID;
                    }


                    // var listOrderDetails = new List<OrderDetailsModel>();

                    foreach (var item in (List<Item>)currentShoppingCart)
                    {
                        var orderDetails = new OrderDetailModel();
                        orderDetails.orderID = orderID;
                        orderDetails.productID = item.Prdcts.productID;
                        orderDetails.quantityOrdered = item.Quantity;
                        orderDetails.priceEach = item.Prdcts.price;

                        db.OrderDetailsModelSet.Add(orderDetails);
                        db.SaveChanges();


                        var query = (from p in db.ProductModelSet
                                     where p.productID == item.Prdcts.productID
                                     join od in db.OrderDetailsModelSet
                                     on p.productID equals od.productID
                                     join o in db.OrderModelSet
                                     on od.orderID equals o.orderID
                                     where o.customerID == uid
                                     select od
                                ).ToList();

                        
                        ProductModel pm = db.ProductModelSet.Find(item.Prdcts.productID);

                        foreach (var item2 in query)
                        {
                            int qInStock = pm.quantityInStock - item2.quantityOrdered;

                            pm.quantityInStock = qInStock;
                        }

                        db.Entry(pm).State = EntityState.Modified;

                        db.SaveChanges();
                    }

                   
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {
                
            }


            //return RedirectToAction("Login", "Account" );

            //  Response.Redirect("app/Views/Account/Login.html");
            //var getOrder = db.OrderModelSet.Select(x => x).Where(x => x.customerID == uid).ToList();

            return View("Index");

        }

        public static DateTime AddWorkdays(DateTime originalDate, int workDays)
        {
            DateTime tmpDate = originalDate;
            while (workDays > 0)
            {
                tmpDate = tmpDate.AddDays(1);
                if (tmpDate.DayOfWeek < DayOfWeek.Saturday ||
                    tmpDate.DayOfWeek > DayOfWeek.Sunday)
                    workDays--;
            }
            return tmpDate;
        }

        public ActionResult Payment()
        {
            return View();
        }

       [HttpPost]
        public ActionResult Payment(PaymentBillingViewModel payBillModel)
        {
            bool paid = false;
            var errors = ModelState
                   .Where(x => x.Value.Errors.Count > 0)
                   .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();

            int uid = Convert.ToInt32(Session["userID"]);
            decimal totalPrice = Convert.ToDecimal(Session["totalPriceFromOrder"]);
            if(Session["userID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        PaymentModel payment = new PaymentModel();
                        BillingModel billing = new BillingModel();
                        OrderModel order = new OrderModel();


                        payment.customerID = uid;
                        payment.paymentDate = DateTime.Now;
                        payment.paymentType = "Online Payment";
                        payment.totalPrice = totalPrice;

                        billing.customerID = uid;
                        billing.cardType = payBillModel.cardType;
                        billing.expDate = payBillModel.expDate;
                        billing.cardHolderName = payBillModel.cardHolderName;
                        billing.cardNumber = payBillModel.cardNumber;
                        billing.cvv = payBillModel.cvv;

                        db.BillingModelSet.Add(billing);
                        db.PaymentModelSet.Add(payment);


                        db.SaveChanges();
                        paid = true;

                        if (paid)
                        {
                            return RedirectToAction("Index", "Order");
                        }


                    }
                }
                catch (Exception)
                {

                    //Response.Write(ex.Message);
                }
            }
                

            return View(payBillModel);
        }

    }
}