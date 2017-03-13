using BuyalotV1._0.DbConnection;
using BuyalotV1._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyalotV1._0.Repository
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