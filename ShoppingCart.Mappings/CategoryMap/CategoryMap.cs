using ShoppingCart.Entities.CategoryEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Mappings.CategoryMap
{
    public class CategoryMap : BaseEntityMap<Category>
    {
        public CategoryMap()
        {
            ToTable("Category");
        }
    }
}
