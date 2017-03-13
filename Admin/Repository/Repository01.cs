using Admin.DbConnection;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Repository
{
    public class Repository01 
    {
        private DataContext modelDbEntities = new DataContext();
        public List<ProductCategoryModel> GetCategories()
        {
            List<ProductCategoryModel> categories = new List<ProductCategoryModel>();

            categories.Add(new ProductCategoryModel { prodCategoryID = 1, categoryName = "Computers" });
            categories.Add(new ProductCategoryModel { prodCategoryID = 2, categoryName = "Laptops" });
            categories.Add(new ProductCategoryModel { prodCategoryID = 3, categoryName = "Mobile Phones" });
            categories.Add(new ProductCategoryModel { prodCategoryID = 4, categoryName = "Gadgets" });
            categories.Add(new ProductCategoryModel { prodCategoryID = 5, categoryName = "TV & Audio" });
            return categories;
        }
        
    }
}