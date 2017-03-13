

using BuyalotV1._0.DbConnection;
using BuyalotV1._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BuyalotV1._0.Controllers
{
    public class ProductsWishlistController : Controller
    {
        private DataContext Context { get; set; }
        private bool _DisposeContext = false;


        public ProductsWishlistController()
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
        // GET: ProductsWishlist
        public ActionResult Index()
        {
            ViewBag.ProductList = Context.ProductModelSet.ToList();
            return View();
        }

        public ActionResult Wish()
        {
            try
            {
                ViewBag.wish = Session["WishCart"];
            }
            catch (Exception)
            {

                ViewBag.ErrorWishMessage = "No Items";
            }
           

            return View();
        }
        private int itemExist(int id)
        {
            List<Item> cart = (List<Item>)Session["WishCart"];
            int counter = cart.Count;
            for (int i = 0; i < counter; i++)
            {
                if (cart[i].Prdcts.productID == id)
                {
                    return i;
                }
                ViewData["CartWishCount"] = cart.Count;
            }
            return -1;

        }

        private int itemExistInCart(int id)
        {
            List<Item> cart = (List<Item>)Session["Cart"];
            int counter = cart.Count;
            for (int i = 0; i < counter; i++)
            {
                if (cart[i].Prdcts.productID == id)
                {
                    return i;
                }
               // ViewData["CartWishCount"] = cart.Count;
            }
            return -1;

        }
        public ActionResult wishAddToCart(int id)
        {

            bool delete = false;
            if (Session["Cart"] == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item(Context.ProductModelSet.Find(id), 1));
                delete = true;
                Session["Cart"] = cart;
                //Session["cartCounter"] = cart.Count;

                if (delete)
                {
                    List<Item> wishCart = (List<Item>)Session["WishCart"];
                    int index = itemExist(id);


                    wishCart.RemoveAt(index);

                    Session["WishCart"] = wishCart;
                    if(wishCart.Count < 1)
                    {
                        Session["WishCounter"] = 0;
                    }

                   

                }
                Session["cartCounter"] = cart.Count;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["Cart"];

                int index = itemExistInCart(id);

                if (index == -1)
                {
                    cart.Add(new Item(Context.ProductModelSet.Find(id), 1));
                    delete = true;
                    if (delete)
                    {
                        List<Item> wishCart = (List<Item>)Session["WishCart"];
                        int itemID = itemExist(id);

                        try
                        {
                            wishCart.RemoveAt(itemID);
                        }
                        catch (IndexOutOfRangeException)
                        {

                            
                        }
                        

                        Session["WishCart"] = wishCart;
                        if (wishCart.Count < 1)
                        {
                            Session["WishCounter"] = 0;
                        }

                    }
                    Session["cartCounter"] = cart.Count;
                }
                else
                {
                    //var cartQ = 0;
                    cart[index].Quantity++;
                    delete = true;
                    if (delete)
                    {
                        List<Item> wishCart = (List<Item>)Session["WishCart"];
                        int itemID = itemExist(id);
                        //cart[itemID].Quantity = cart[itemID].Quantity + wishCart[itemID].Quantity1;
                        ////cartQ = cartQ + wishCart.Count;
                        //Session["cartCounter"] = cart[itemID].Quantity;
                        wishCart.RemoveAt(itemID);

                        Session["WishCart"] = wishCart;
                        if (wishCart.Count < 1)
                        {
                            Session["WishCounter"] = 0;
                        }

                    }
                    //cartQ = cartQ + cart[index].Quantity;
                  

                }
               
                Session["Cart"] = cart;
                //ViewData["CartCount"] = cart.Count;
                ViewData["CartWishCount"] = cart.Count;
            }

            return RedirectToAction("Wish", "ProductsWishlist");
        }
        public ActionResult AddToWishlist(int id)
        {
           
            if (Session["WishCart"] == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item(Context.ProductModelSet.Find(id), 1));

                Session["WishCart"] = cart;
                //Session["WishCounter"] = cart.Count;
             
            }
            else
            {
                List<Item> cart = (List<Item>)Session["WishCart"];

                int index = itemExist(id);

                if (index == -1)
                {
                    cart.Add(new Item(Context.ProductModelSet.Find(id), 1));
                    ///Session["WishCounter"] = cart.Count;
                }
                else
                {
                    cart[index].Quantity1++;

                    //if (Session["WishCounter"] == null)
                    //{
                    //    Session["WishCounter"] = cart.Count;
                    //}
                    //else
                    //{

                    //    var session = Session["cartCounter"];
                    //    Session["cartCounter"] = Convert.ToInt32(session) + cart[index].Quantity1;
                    //}

                }

                Session["WishCart"] = cart;
                ViewData["CartWishCount"] = cart.Count;
            }

            return View("AddToWishlist");
        }
        public ActionResult DeleteWish(int id)
        {
            List<Item> cart = (List<Item>)Session["WishCart"];
            int index = itemExist(id);


            cart.RemoveAt(index);

            if (cart.Count < 1)
            {
                Session["WishCounter"] = 0;
            }

            List<Item> cart1 = (List<Item>)Session["Cart"];

            try
            {
                Session["cartCounter"] = cart1.Count;
            }
            catch (NullReferenceException)
            {

               
            }
           
            Session["WishCart"] = cart;
            ViewData["CartWishCount"] = cart.Count;
            return View("AddToWishlist");
        }
        public ActionResult UpdateQuantity1(FormCollection fc)
        {
            string[] quantities = fc.GetValues("txtQuantity1");
            List<Item> cart = (List<Item>)Session["WishCart"];
            int counter = cart.Count;

            for (int i = 0; i < counter; i++)
            {
                cart[i].Quantity = Convert.ToInt32(quantities[i]);
                Session["WishCart"] = cart;
                //ViewData["CartCount"] = cart.Count;
                ViewData["CartWishCount"] = cart.Count;
            }



            return View("AddToWishlist");
        }

        public ActionResult UpdateWishCounter()
        {

            List<Item> cart = (List<Item>)Session["WishCart"];
            int counter = cart.Count;

            for (int i = 0; i < counter; i++)
                cart[i].Quantity1++;
            Session["WishCart"] = cart;
            ViewData["CartWishCount"] = cart.Count;

            return View("AddToWishlist");
        }    
            
    }
}