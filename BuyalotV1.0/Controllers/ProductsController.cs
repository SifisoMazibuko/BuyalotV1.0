using BuyalotV1._0.DbConnection;
using BuyalotV1._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BuyalotV1._0.Controllers;

namespace BuyalotV1._0.Controllers
{
    public class ProductsController : Controller
    {
        private DataContext Context { get; set; }
        private bool _DisposeContext = false;
        public int tempProdId;


        public ProductsController()
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

        // GET: Product
        public ActionResult Index()
        {
            ViewBag.ProductList = Context.ProductModelSet.ToList();
            return View();
        }

        public ActionResult Cart()
        {
            try
            {
                ViewBag.cart = Session["Cart"];
            }
            catch (Exception)
            {

               
            }
           

            ViewBag.av = Session["avail"];
            return View();
        }

        public ActionResult Details(int? id, string searchString)
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


            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    HomeController hmC = new HomeController();

            //    try
            //    {
            //        return hmC.Index(searchString);
            //    }
            //    catch (InvalidOperationException)
            //    {

                    
            //    }
                
            //    //return 
            //}
            return View(product);
        }

        public ActionResult ViewProducts()
        {
            var product = (from p in Context.ProductModelSet
                           select p).ToList();
            return View(product);
        }

        

        public ActionResult AddProduct()
        {

            DataContext db = new DataContext();

            //IEnumerable<SelectListItem> items = new SelectList(db.ProductCategoryModelSet, "prodCategoryID", "categoryName");
            //ViewBag.prodCategoryID = items;

            ProductModel pro = new ProductModel();
            return View(pro);
        }


        [HttpPost]
        public ActionResult AddProduct(ProductModel product, HttpPostedFileBase upload)
        {
            var db = new DataContext();

            if (upload != null)
            {
                product.productImage = new byte[upload.ContentLength];
                upload.InputStream.Read(product.productImage, 0, upload.ContentLength);
            }
            db.ProductModelSet.Add(product);
            db.SaveChanges();

            return View(product);
        }


        public ActionResult populateDropDown()
        {
            List<SelectListItem> CustIDlistItems = new List<SelectListItem>();

            var getCategory = Context.ProductCategoryModelSet.ToList();
            foreach (var category in getCategory)
            {
                CustIDlistItems.Add(new SelectListItem
                {
                    Text = category.categoryName,
                    Value = category.prodCategoryID.ToString()
                });
            }

            ViewBag.CatList = CustIDlistItems;
            return View();
        }

        private int itemExist(int id)
        {
            List<Item> cart = (List<Item>)Session["Cart"];
            int counter = cart.Count;
            for (int i = 0; i < counter; i++)
            {
                if (cart[i].Prdcts.productID == id)
                {
                    return i;
                }
                ViewData["CartCount"] = cart.Count;
            }
            return -1;

        }

        //[Authorize(Roles = "CustomerViewModel")]
        public ActionResult AddToCart(int id)
        {
           
            if (Session["Cart"] == null)
            {
                List<Item> cart = new List<Item>();
                

                cart.Add(new Item(Context.ProductModelSet.Find(id), 1));
              
                Session["Cart"] = cart;
               // Session["cartCounter"] = cart.Count;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["Cart"];

                int index = itemExist(id);

                if (index == -1)
                {
                    cart.Add(new Item(Context.ProductModelSet.Find(id), 1));
                    //Session["cartCounter"] = cart.Count;
                }
                else
                {
                    var getProduct = Context.ProductModelSet.Find(id);
                   
                    if (cart[index].Quantity < getProduct.quantityInStock)
                    {
                        cart[index].Quantity++;
                        Session["avail"] = null;
                    }
                    else
                    {
                        ViewBag.Availability = "we only have " + getProduct.quantityInStock + " " + getProduct.productName + " available in stock";
                        Session["avail"] = ViewBag.Availability;
                    }
                   

                    //if (Session["cartCounter"] == null)
                    //{
                    //    Session["cartCounter"] = cart.Count;
                    //}
                    //else
                    //{

                    //    var session = Session["cartCounter"];
                    //    Session["cartCounter"] = Convert.ToInt32(session) + cart[index].Quantity;
                    //}

                }

                Session["Cart"] = cart;
                ViewData["CartCount"] = cart.Count;
            }
          

                return View("AddToCart");
        }

        public ActionResult DeleteCart(int id)
        {
            List<Item> cart = (List<Item>)Session["Cart"];
            int index = itemExist(id);


            cart.RemoveAt(index);

            if(cart.Count < 1)
            {
                Session["cartCounter"] = 0;
            }

            Session["Cart"] = cart;

            return View("AddToCart");
        }

        public ActionResult UpdateQuantity(FormCollection fc)
        {
           

            string[] quantities = fc.GetValues("txtQuantity");

            string[] pids = fc.GetValues("pid");
            List<Item> cart = (List<Item>)Session["Cart"];
            int counter = cart.Count;

            for (int i = 0; i < counter; i++)
            {
                tempProdId = Convert.ToInt32(Session["tempProdId"]);
                var getProduct = Context.ProductModelSet.Find(Convert.ToInt32(pids[i]));


                if (Convert.ToInt32(quantities[i]) <= getProduct.quantityInStock)
                {
                    cart[i].Quantity = Convert.ToInt32(quantities[i]);
                    Session["avail"] = null;
                }
                else
                {
                    ViewBag.Availability = "we only have " + getProduct.quantityInStock + " " + getProduct.productName + " available in stock";
                    Session["avail"] = ViewBag.Availability;
                }


                Session["Cart"] = cart;
                ViewData["CartCount"] = cart.Count;
            }



            return View("AddToCart");
        }

        public ActionResult UpdatecartCounter()
        {

            List<Item> cart = (List<Item>)Session["Cart"];
            int counter = cart.Count;

            for (int i = 0; i < counter; i++)
                cart[i].Quantity++;
            Session["Cart"] = cart;
            ViewData["CartCount"] = cart.Count;

            return View("AddToCart");
        }

        //[HttpGet]
        //public ActionResult Payment()
        //{
        //    return View();
        //}
        [HttpPost]
        public ActionResult Payment()
        {
            string MERCHANT = "PHONE"
             , CURRENCY = "ZAR"
             , COUNTRY = "ZA"
             , REFERENCE = "TEST-001-001"
             , AMOUNT = "100.00"
             , PRIVATEKEY = "3912CB99-20A0-4F97-8D20-D9900F67A6A2"
             , FINAL_CONSISTENT_KEY = "";

            SHA512 SHA512HashCreator = SHA512.Create();

            StringBuilder concatenatedString = new StringBuilder();
            concatenatedString.Append(MERCHANT);
            concatenatedString.Append(CURRENCY);
            concatenatedString.Append(COUNTRY);
            concatenatedString.Append(REFERENCE);
            concatenatedString.Append(AMOUNT);
            concatenatedString.Append(PRIVATEKEY);

            byte[] EncryptedData = SHA512HashCreator.ComputeHash(Encoding.UTF8.GetBytes(concatenatedString.ToString()));

            StringBuilder CONSISTENT_KEY = new StringBuilder();

            for (int i = 0; i < EncryptedData.Length; i++)
            {
                CONSISTENT_KEY.Append(EncryptedData[i].ToString("X2"));
            }

            FINAL_CONSISTENT_KEY = CONSISTENT_KEY.ToString().ToUpper();
            return View();
        }

    }
}