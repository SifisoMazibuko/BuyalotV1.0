using BuyalotV1._0.DbConnection;
using BuyalotV1._0.Models;
using BuyalotV1._0.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuyalotV1._0.Controllers
{
    public class HomeController : Controller
    {

        private ICategoryRepository iCategoryRepository = new CategoryRepository();
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

        //public ActionResult Index()
        //{
        //    //ViewBag.ProductList = Context.ProductModelSet.ToList();
        //    return View();
        //}

        public ActionResult Index(string searchString)
        {
            var db = new DataContext();

            var product = (from p in db.ProductModelSet
                           where p.quantityInStock > 0
                           join pc in db.ProductCategoryModelSet
                           on p.prodCategoryID equals pc.prodCategoryID
                           select new ProductCatViewModel
                           {
                               productImage = p.productImage,
                               productID = p.productID,
                               price = p.price,
                               productName = p.productName,
                               vendor = p.vendor,
                               quantityInStock = p.quantityInStock,
                               productDescription = p.productDescription,
                               categoryName = pc.categoryName
                           });


            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(s => s.productName.StartsWith(searchString)
                                       || s.productName.Contains(searchString)
                                       || s.vendor.StartsWith(searchString)
                                       || s.productDescription.StartsWith(searchString)
                                       || s.productDescription.Contains(searchString)
                                       || s.vendor.Contains(searchString)
                                       || s.categoryName.Contains(searchString)
                                       || s.categoryName.StartsWith(searchString)
                                       );

            }

            ViewBag.ProductList = db.ProductModelSet.ToList();
            return View(product);
             
        }

        public ActionResult Category(string id, string searchString)
        {
            //var category = iCategoryRepository.find(id);
            //ViewBag.category = category;

            
            if (string.IsNullOrEmpty(searchString))
            {
                var category = (from p in Context.ProductModelSet
                                where p.quantityInStock > 0
                                join pc in Context.ProductCategoryModelSet
                                on p.prodCategoryID equals pc.prodCategoryID
                                where pc.categoryName == id
                                select p).ToList();
                ViewBag.products = category;
            }
            else
            {   

                var category = (from p in Context.ProductModelSet
                                where p.quantityInStock > 0
                                join pc in Context.ProductCategoryModelSet
                               on p.prodCategoryID equals pc.prodCategoryID
                               select new ProductCatViewModel
                               {
                                   productImage = p.productImage,
                                   productID = p.productID,
                                   price = p.price,
                                   productName = p.productName,
                                   vendor = p.vendor,
                                   quantityInStock = p.quantityInStock,
                                   productDescription = p.productDescription,
                                   categoryName = pc.categoryName
                               });


                if (!String.IsNullOrEmpty(searchString))
                {
                    category = category.Where(s => s.productName.StartsWith(searchString)
                                           || s.productName.Contains(searchString)
                                           || s.vendor.StartsWith(searchString)
                                           || s.productDescription.StartsWith(searchString)
                                           || s.productDescription.Contains(searchString)
                                           || s.vendor.Contains(searchString)
                                           || s.categoryName.Contains(searchString)
                                           || s.categoryName.StartsWith(searchString)
                                           
                                           );

                }
                ViewBag.products = category;
            }  

            return View("Category");
        }  

    }
}