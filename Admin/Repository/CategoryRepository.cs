using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.Models;
using Admin.DbConnection;
using Admin.Repository;

namespace Admin.Buyalot.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private DataContext modelDbEntities = new DataContext();

        public ProductCategoryModel find(int id)
        {
            return modelDbEntities.ProductCategoryModelSet.Find(id);
        }

        public List<ProductCategoryModel> findAll()
        {
            return modelDbEntities.ProductCategoryModelSet.ToList();
        }
    }
}