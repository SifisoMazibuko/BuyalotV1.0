using System.Collections.Generic;
using Admin.Models;

namespace Admin.Repository
{
    public interface ICategoryRepository
    {
        List<ProductCategoryModel> findAll();
        ProductCategoryModel find(int id);
    }
}
