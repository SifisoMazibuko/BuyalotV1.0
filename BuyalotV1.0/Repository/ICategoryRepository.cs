using System.Collections.Generic;
using BuyalotV1._0.Models;

namespace BuyalotV1._0.Repository
{
    public interface ICategoryRepository
    {
        List<ProductCategoryModel> findAll();
        ProductCategoryModel find(int id);
    }
}
