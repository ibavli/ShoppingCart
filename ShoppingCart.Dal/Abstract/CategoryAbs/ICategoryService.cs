using ShoppingCart.Entities.CategoryEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Dal.Abstract.CategoryAbs
{
    public interface ICategoryService
    {
        bool SaveCategory(Category category);

        Category GetById(int Id);

        List<Category> GetCategories();
    }
}

